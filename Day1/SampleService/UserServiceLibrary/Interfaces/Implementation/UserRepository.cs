using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions;
using UserServiceLibrary.Interfaces;

namespace UserServiceLibrary.Interfaces.Implementations
{
    public class UserRepository : IUserRepository
    {

        private readonly Func<User, User, User> _equalSearch = (a, b) =>
        {
            return a.Equals(b) ? a : null;
        };

        private readonly List<User> _userList = new List<User>();

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

        public int Add(User user)
        {
            this._userList.Add(user);
            return user.Id;
        }

        public IEnumerable<int> AddRange(IEnumerable<User> users)
        {
            var ret = new List<int>();
            foreach (var user in users)
            {
                ret.Add(this.Add(user));
            }

            return ret;
        }

        public bool Contains(User user)
        {
            return this._userList.Contains(user);
        }

        public bool Remove(User user)
        {
            return this._userList.Remove(user);
        }

        public User Search(User user)
        {
            return this._userList.Find(x => this._equalSearch(x, user) != null);
        }

        public IEnumerable<User> SearchByPredicate(Func<User, bool> search)
        {
            var ret = new List<User>();
            foreach (var elem in this._userList)
            {
                if ( search.Invoke(elem) ) ret.Add(elem);
            }

            return ret;
        }
    }
}
