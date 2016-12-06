using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions;
using UserServiceLibrary.Interfaces.Generic;

namespace UserServiceLibrary
{
    public class Slave : MarshalByRefObject
    {
        private readonly IService<User> _service;
        private readonly int _port;

        /// <summary>
        /// Create slave
        /// </summary>
        /// <param name="service"></param>
        public Slave(IService<User> service)
        {
            this._service = service;
            this._port = Int32.Parse(ConfigurationManager.AppSettings["MasterPort"]);
        }
        /// <summary>
        /// Listen master stream
        /// </summary>
        public void Listen()
        {
            TcpListener listener = null;
            Message message = null;
            try
            {
                listener = new TcpListener(IPAddress.Parse(ConfigurationManager.AppSettings["MasterIP"]), this._port);
                listener.Start();
                var bf = new BinaryFormatter();
                while (true)
                {
                    var client = listener.AcceptTcpClient();

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
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                listener.Stop();
            }
        }
        public void Add(User user)
        {
            throw new SlaveOperationException();
        }

        public void Update(User user)
        {
            throw new SlaveOperationException();
        }

        public void Remove(User user)
        {
            throw new SlaveOperationException();
        }

        public IEnumerable<User> Search(User user)
        {
            throw new SlaveOperationException();
        }

        private void EventAction(Message message)
        {
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
