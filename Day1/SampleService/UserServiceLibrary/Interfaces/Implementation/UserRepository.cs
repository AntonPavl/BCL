using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions;
using UserServiceLibrary.Interfaces;
using UserServiceLibrary.Interfaces.Generic;

namespace UserServiceLibrary.Interfaces.Implementations
{
    public class UserRepository : IRepository<User>
    {

        private readonly Func<User, User, User> _equalSearch = (a, b) =>
        {
            return a.Equals(b) ? a : null;
        };

        private readonly List<User> _userList = new List<User>();
        /// <summary>
        /// Create userRepository instance
        /// </summary>
        /// <param name="users">Users</param>
        public UserRepository(IEnumerable<User> users)
        {
            this.AddRange(users);
        }
        /// <summary>
        /// Create userRepository instance
        /// </summary>
        /// <param name="user">user</param>
        public UserRepository(User user)
        {
            this.Add(user);
        }
        /// <summary>
        /// Create userRepository instance
        /// </summary>
        public UserRepository()
        {
        }
        /// <summary>
        /// Get users count
        /// </summary>
        public int Count => this._userList.Count();
        /// <summary>
        /// Add user to repository
        /// </summary>
        /// <param name="user">user</param>
        /// <returns></returns>
        public int Add(User user)
        {
            this._userList.Add(user);
            return user.Id;
        }
        /// <summary>
        /// Add users range to repository
        /// </summary>
        /// <param name="users">users</param>
        /// <returns></returns>
        public IEnumerable<int> AddRange(IEnumerable<User> users)
        {
            var ret = new List<int>();
            foreach (var user in users)
            {
                ret.Add(this.Add(user));
            }

            return ret;
        }
        /// <summary>
        /// True if repository contains user
        /// </summary>
        /// <param name="user">user</param>
        /// <returns></returns>
        public bool Contains(User user)
        {
            return this._userList.Contains(user);
        }
        /// <summary>
        /// Remove user from repository
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Remove(User user)
        {
            return this._userList.Remove(user);
        }
        /// <summary>
        /// Search user in repository
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User Search(User user)
        {
            return this._userList.Find(x => this._equalSearch(x, user) != null);
        }
        /// <summary>
        /// Search users by predicate
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<User> SearchByPredicate(Func<User, bool> search)
        {
            var ret = new List<User>();
            foreach (var elem in this._userList)
            {
                if ( search.Invoke(elem) ) ret.Add(elem);
            }

            return ret;
        }
        /// <summary>
        /// Get all users from repository
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetEntities()
        {
            return _userList;
        }

        public IEnumerator<User> GetEnumerator()
        {
            foreach (var item in _userList)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
