using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary
{
    public enum EventStatus
    {
        Add = 0,
        Remove = 1,
        Search = 2,
        Update = 3
    };
    [Serializable]
    public class Message
    {
        public EventStatus EventStatus { get; }
        public User User { get; }

        public Message(User user,EventStatus es)
        {
            this.EventStatus = es;
            this.User = user;
        }
    }
}
