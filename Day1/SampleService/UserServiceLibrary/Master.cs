using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
        private TcpClient _client;
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
            this._client = new TcpClient(this._ip, this._port);
            Logger.Info($"Master[{Thread.CurrentThread.ManagedThreadId}] in Domain[{Thread.GetDomainID()}] has been start");
        }
        /// <summary>
        /// Create masterUserService 
        /// </summary>
        /// <param name="service"></param>
        public Master(IService<User> service) : this(service,LogManager.GetCurrentClassLogger())
        {
        }
        ~Master()
        {
            _client.Close();
        }
        /// <summary>
        /// Add user to service and send message to slaves
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Add(User user)
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
        /// <summary>
        /// Remove user from service and send message to slaves
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Remove(User user)
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
        /// <summary>
        /// Search user in service and send message to slaves
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IEnumerable<User> Search(User user)
        {
            Logger.Info($"Master[{Thread.CurrentThread.ManagedThreadId}] start send Search message");
            SendMessage(new Message(user, EventStatus.Search));
            Logger.Info($"Master[{Thread.CurrentThread.ManagedThreadId}] end Search message send");
            return null;
        }

        private void SendMessage(Message message)
        {
            var bf = new BinaryFormatter();
            using (NetworkStream stream = _client.GetStream())
            {
                bf.Serialize(stream, message);
            }
        }
    }
}
