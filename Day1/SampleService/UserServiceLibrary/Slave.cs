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
using UserServiceLibrary.Exceptions;
using UserServiceLibrary.Interfaces.Generic;

namespace UserServiceLibrary
{
    public class Slave : MarshalByRefObject
    {
        private readonly IService<User> _service;
        private readonly int _port;
        private readonly string _ip;
        private object semaphor = new object();
        public ILogger Logger { set; private get; }
        /// <summary>
        /// Create slave with logger
        /// </summary>
        /// <param name="service"></param>
        public Slave(IService<User> service,ILogger logger,string ip)
        {
            if (service == null || logger == null) throw new ArgumentNullException();
            this.Logger = logger;
            this._service = service;
            this._port = Int32.Parse(ConfigurationManager.AppSettings["MasterPort"]);
            this._ip = ip;
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] in Domain[{Thread.GetDomainID()}] has been start");
            new Thread(() => Listen()).Start();
        }
        /// <summary>
        /// Create slave
        /// </summary>
        /// <param name="service"></param>
        public Slave(IService<User> service,string ip) : this(service,LogManager.GetCurrentClassLogger(),ip)
        {
        }
        public void Listen()
        {
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] start Listen");
            Message message = null;
            TcpClient client = null;
            try
            {
                //var port = Int32.Parse(ConfigurationManager.AppSettings["MasterPort"]);
                using (client = new TcpClient(_ip, _port))
                {
                    Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] wait connection");
                    if (client.Connected == true)
                    {
                        var bf = new BinaryFormatter();
                        Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] connected");
                        var stream = client.GetStream();
                        try
                        {
                            while (true)
                            {
                                if (stream.DataAvailable)
                                { 
                                    message = (Message)bf.Deserialize(stream);
                                    Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] do {message.EventStatus}");
                                    EventAction(message);
                                }
                            }
                        }
                        finally
                        {
                            stream.Close();
                        }
                    }
                    else
                    {
                        Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] not connected");
                    }
                }
            }
            catch (SocketException e)
            {
                Logger.Error($"Slave[{Thread.CurrentThread.ManagedThreadId}] {e.StackTrace}");
                Console.WriteLine("SocketException: {0}", e);
            }
        }
        public void Add()
        {
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] Add");
            throw new SlaveOperationException();
        }

        public void Update()
        {
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] Update");
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] Finished Update");
        }

        public void Remove()
        {
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] Remove");
            throw new SlaveOperationException();
        }

        public IEnumerable<User> Search(User user)
        {
            lock (semaphor)
            {
                var ret = new List<User>();
                Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] Search");
                Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] Complete Search");
                ret.Add(_service.Search(user));
                foreach (var item in ret)
                {
                    Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] find {item.FirstName},{item.LastName}");
                }
                return ret;
            }
        }

        private void EventAction(Message message)
        {
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] try to do {message.EventStatus}");
            switch (message.EventStatus)
            {
                case EventStatus.Add:
                    Add();
                    break;

                case EventStatus.Remove:
                    Remove();
                    break;

                case EventStatus.Update:
                    Update();
                    break;

                case EventStatus.Search:
                    Search(message.User);
                    break;

                default:
                    break;
            }
        }
    }
}
