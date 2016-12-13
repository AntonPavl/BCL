using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserServiceLibrary.Interfaces.Generic;
using UserServiceLibrary.Interfaces.Implementations;

namespace UserServiceLibrary
{
    public class Master
    {
        private readonly IService<User> _service;
        private readonly int _port;
        private readonly string _ip;
        private TcpListener _client;
        private List<NetworkStream> slavesStreams;
        private object semaphor = new object();
        private struct slave
        {
            public string ip;
            public string port;
            public slave(string ip,string port)
            {
                this.ip = ip;
                this.port = port;
            }
        }
        private List<slave> slaves = new List<slave>();
        public ILogger Logger { set; private get; }

        /// <summary>
        /// Create masterUserService with logger
        /// </summary>
        /// <param name="service"></param>
        public Master(IService<User> service, ILogger logger)
        {
            if (service == null || logger == null) throw new ArgumentNullException();
            this.Logger = logger;
            this._service = service;
            this._port = Int32.Parse(ConfigurationManager.AppSettings["MasterPort"]);
            this._ip = ConfigurationManager.AppSettings["MasterIP"];
            this.slavesStreams = new List<NetworkStream>();
            GetSlaves();
            this._client = new TcpListener(IPAddress.Any, this._port);
            var t = new Thread(() => StartServer());
            t.Start();
            Logger.Info($"Master[{Thread.CurrentThread.ManagedThreadId}] in Domain[{Thread.GetDomainID()}] has been start");
        }
        /// <summary>
        /// Create masterUserService 
        /// </summary>
        /// <param name="service"></param>
        public Master(IService<User> service) : this(service,LogManager.GetCurrentClassLogger())
        {
        }
        
        /// <summary>
        /// Add user to service and send message to slaves
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Add(User user)
        {
            lock (semaphor)
            {
                Logger.Info($"Master[{Thread.CurrentThread.ManagedThreadId}] Add");
                var result = this._service.Add(user);

                if (result != 0)
                {
                    Logger.Info($"Master[{Thread.CurrentThread.ManagedThreadId}] start send Update message");
                    SendMessage(new Message(null, EventStatus.Update));
                    Logger.Info($"Master[{Thread.CurrentThread.ManagedThreadId}] end Update message send");
                }
                return result;
            }
        }
        /// <summary>
        /// Remove user from service and send message to slaves
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Remove(User user)
        {
            lock (semaphor)
            {
                var result = this._service.Remove(user);

                if (result)
                {
                    Logger.Info($"Master[{Thread.CurrentThread.ManagedThreadId}] start send Update message");
                    SendMessage(new Message(null, EventStatus.Update));
                    Logger.Info($"Master[{Thread.CurrentThread.ManagedThreadId}] end Update message send");
                }

                return result;
            }
        }
        /// <summary>
        /// Search user in service and send message to slaves
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IEnumerable<User> Search(User user)
        {
            lock (semaphor)
            {
                Logger.Info($"Master[{Thread.CurrentThread.ManagedThreadId}] start send Search message");
                SendMessage(new Message(user, EventStatus.Search));
                Logger.Info($"Master[{Thread.CurrentThread.ManagedThreadId}] end Search message send");
                return null;
            }
        }

        private void SendMessage(Message message)
        {

            var bf = new BinaryFormatter();
            //Pool thread?
            lock (semaphor)
            { 
                foreach (var item in slavesStreams)
                {
                    bf.Serialize(item, message);
                    item.Flush();
                }
            }
        }

        private void GetSlaves()
        {
            int i;
            var myappSet = (dynamic)ConfigurationManager.GetSection("Slaves");
            for (i = 0; i < Int32.Parse(myappSet["slavesNum"]); i++)
            {
                var x = myappSet[$"slave{i}"].Split(' ');
                slaves.Add(new slave(x[0], x[1]));
            }
            Logger.Info($"Master[{Thread.CurrentThread.ManagedThreadId}] in Domain[{Thread.GetDomainID()}] known {i} slaves");
        }

        private void StartServer()
        {
            try
            {
                _client.Start();
                var bf = new BinaryFormatter();
                while (true)
                {
                    Logger.Info($"Master[{Thread.CurrentThread.ManagedThreadId}] wait connection");
                    var client = _client.AcceptTcpClient();
                    slavesStreams.Add(client.GetStream());
                    Logger.Info($"Slave connected");
                }
            }
            catch (SocketException e)
            {
                throw new SocketException();
            }
            finally
            {
                foreach (var item in slavesStreams)
                {
                    item.Close();
                }
                _client.Stop();
            }
        }
    }
}
