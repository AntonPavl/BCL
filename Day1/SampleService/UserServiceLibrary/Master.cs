using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Interfaces.Generic;
using UserServiceLibrary.Interfaces.Implementations;

namespace UserServiceLibrary
{
    public class Master
    {
        private readonly IService<User> _service;
        private readonly int port;
        private readonly string ip;
        public Master(IService<User> service)
        {
            this.port = Int32.Parse(ConfigurationManager.AppSettings["MasterPort"]);
            this.ip = ConfigurationManager.AppSettings["MasterIP"];
            this._service = service;
        }
        public int Add(User user)
        {
            var result = this._service.Add(user);

            if (result != 0)
            {
                SendMessage(new Message(null, EventStatus.Update));
            }

            return result;
        }

        public bool Remove(User user)
        {
            var result = this._service.Remove(user);

            if (result)
            {
                SendMessage(new Message(null, EventStatus.Update));
            }

            return result;
        }

        public IEnumerable<User> Search(User user)
        {
             SendMessage(new Message(user, EventStatus.Search));
            return null;
        }

        private void SendMessage(Message message)
        {
            var bf = new BinaryFormatter();
            using (TcpClient client = new TcpClient(this.ip, this.port))
            {
                using (NetworkStream stream = client.GetStream())
                {
                    bf.Serialize(stream, message);
                }
            }
        }
    }
}
