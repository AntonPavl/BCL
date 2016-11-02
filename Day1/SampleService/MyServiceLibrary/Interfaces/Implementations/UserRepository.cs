using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions;

namespace UserServiceLibrary.Interfaces.Implementations
{
    public class UserRepository : IUserRepository
    {
        private List<User> _userList = new List<User>();

        private Func<User,User, User> _equalSearch = (a,b) => 
        {
            return a.Equals(b) ? a : null;
        };

        public UserRepository(IEnumerable<User> users)
        {
            this.AddRange(users);
        }

        public UserRepository(User user)
        {
            this.Add(user);
        }

        public UserRepository()
        {
        }

        public int Count => this._userList.Count();

        public User Add(User user)
        {
            this._userList.Add(user);
            return user;
        }

        public IEnumerable<User> AddRange(IEnumerable<User> users)
        {
            List<User> ret = new List<User>();
            foreach (var user in users)
            {
                ret.Add(this.Add(user));
            }

            return ret;
        }

        public bool Remove(User user)
        {
            return this._userList.Remove(user);
        }

        public User Search(User user)
        {
            return this._userList.Find(x => this._equalSearch(x, user) != null);
        }

        public IEnumerable<User> SearchByPredicate(Func<User,User> search)
        {
            List<User> ret = new List<User>();
            foreach (var elem in this._userList)
            {
                if (search(elem) != null) ret.Add(elem);
            }

            return ret;
        }

        public bool Contains(User user)
        {
            return this._userList.Contains(user);
        }
    }
}
