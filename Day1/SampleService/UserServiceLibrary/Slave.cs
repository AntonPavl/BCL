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
        public ILogger Logger { set; private get; }
        /// <summary>
        /// Create slave with logger
        /// </summary>
        /// <param name="service"></param>
        public Slave(IService<User> service,ILogger logger)
        {
            if (service == null || logger == null) throw new ArgumentNullException();
            this.Logger = logger;
            this._service = service;
            this._port = Int32.Parse(ConfigurationManager.AppSettings["MasterPort"]);
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] in Domain[{Thread.GetDomainID()}] has been start");
            new Thread(() => Listen()).Start();
        }
        /// <summary>
        /// Create slave
        /// </summary>
        /// <param name="service"></param>
        public Slave(IService<User> service) : this(service,LogManager.GetCurrentClassLogger())
        {
        }
        /// <summary>
        /// Listen master stream
        /// </summary>
        //public void Listen()
        //{
        //    Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] start Listen");
        //    TcpListener listener = null;
        //    Message message = null;
        //    try
        //    {
        //        listener = new TcpListener(IPAddress.Parse(ConfigurationManager.AppSettings["MasterIP"]), this._port);
        //        listener.Start();
        //        var bf = new BinaryFormatter();
        //        while (true)
        //        {
        //            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] wait connection");
        //            var client = listener.AcceptTcpClient();
        //            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] connected");
        //            using (NetworkStream stream = client.GetStream())
        //            {
        //                message = (Message)bf.Deserialize(stream);
        //                EventAction(message);
        //            }
        //            client.Close();
        //        }
        //    }
        //    catch (SocketException e)
        //    {
        //        Logger.Error($"Slave[{Thread.CurrentThread.ManagedThreadId}] {e.StackTrace}");
        //        Console.WriteLine("SocketException: {0}", e);
        //    }
        //    finally
        //    {
        //        listener.Stop();
        //    }
        //}
        public void Listen()
        {
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] start Listen");
            TcpListener listener = null;
            Message message = null;
            try
            {
                listener = new TcpListener(IPAddress.Parse(ConfigurationManager.AppSettings["MasterIP"]), this._port);
                listener.Start();
                var bf = new BinaryFormatter();
                while (true)
                {
                    Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] wait connection");
                    var client = listener.AcceptTcpClient();
                    Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] connected");
                    using (NetworkStream stream = client.GetStream())
                    {
                        message = (Message)bf.Deserialize(stream);
                        EventAction(message);
                    }
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Logger.Error($"Slave[{Thread.CurrentThread.ManagedThreadId}] {e.StackTrace}");
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                listener.Stop();
            }
        }
        public void Add(User user)
        {
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] Add");
            throw new SlaveOperationException();
        }

        public void Update(User user)
        {
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] Update");
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] Finished Update");
        }

        public void Remove(User user)
        {
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] Remove");
            throw new SlaveOperationException();
        }

        public IEnumerable<User> Search(User user)
        {
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] Search");
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] Complete Search");
            return null;
        }

        private void EventAction(Message message)
        {
            Logger.Info($"Slave[{Thread.CurrentThread.ManagedThreadId}] try to do {message.EventStatus}");
            switch (message.EventStatus)
            {
                case EventStatus.Add:
                    Add(message.User);
                    break;

                case EventStatus.Remove:
                    Remove(message.User);
                    break;

                case EventStatus.Update:
                    Update(message.User);
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
