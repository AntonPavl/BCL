using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServiceLibrary.Exceptions;

namespace MyServiceLibrary.Interfaces.Implementations
{
    public class UserStorageService : IUserStorageService
    {
        private UserRepository _userStorage;

        public UserStorageService(UserRepository us)
        {
            if (us == null) throw new ArgumentNullException();
            this._userStorage = us;
        }

        public int Count => this._userStorage.Count;

        public User Add(User user)
        {
            this.Validate(user);
            return this._userStorage.Add(user);
        }

        public IEnumerable<User> AddRange(IEnumerable<User> users)
        {
            if (users == null) throw new ArgumentNullException();
            if (users.Distinct().Count() < users.Count()) throw new ArgumentException();
            foreach (var item in users)
            {
                Validate(item);
            }
            return this._userStorage.AddRange(users);
        }

        public bool Remove(User user)
        {
            return this._userStorage.Remove(user);
        }

        public User Search(User user)
        {
            return this._userStorage.Search(user);
        }

        public IEnumerable<User> SearchByPredicate(Func<User, User> f)
        {
            return _userStorage.SearchByPredicate(f);
        }

        private void Validate(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(user.FirstName))
            {
                throw new UserFieldsNullException();
            }

            if (string.IsNullOrEmpty(user.LastName))
            {
                throw new UserFieldsNullException();
            }

            if (this._userStorage.Contains(user))
            {
                throw new UserExistsException();
            }
        }
    }
}
