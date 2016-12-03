using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions;
using UserServiceLibrary.Interfaces.Implementation;

namespace UserServiceLibrary.Interfaces.Implementations
{
    [Serializable]
    public class UserStorageService : IUserStorageService
    {

        #region fields

        private readonly IUserRepository _userStorage;

        private readonly Func<int, int> _idGenerator = (a) => a + 1;

        private readonly IUserValidator _baseValidator = new UserValidator();

        private readonly XMLDump _baseDump = new XMLDump();

        private int _lastId;

        #endregion endfields

        #region ctors
        public UserStorageService(IUserRepository us,Func<int,int> idGenerator)
        {
            if (us == null || idGenerator == null) throw new ArgumentNullException();
            Validator = _baseValidator;
            this._idGenerator = idGenerator;
            this._userStorage = us;
        }

        public UserStorageService(IUserRepository us) : this(us, (a) => a + 1) { }

        public UserStorageService() : this(new UserRepository())
        {

        }

        #endregion ctors

        #region props
        public int Count { get { return this._userStorage.Count; } }

        public IUserValidator Validator { get; set; }

        public int GetLastId { get { return _lastId; } }

        #endregion props

        #region methods

        public int Add(User user)
        {
            user.Id = this._idGenerator?.Invoke(this._lastId) ?? default(int);
            if (!this.Validator.Validate(user)) throw new ArgumentException();
            return this._userStorage.Add(user);
        }

        public IEnumerable<int> AddRange(IEnumerable<User> users)
        {
            if (users == null) throw new ArgumentNullException();
            if (users.Distinct().Count() < users.Count()) throw new ArgumentException();
            foreach (var item in users)
            {
                if (!Validator.Validate(item)) throw new ArgumentException();
                item.Id = _idGenerator?.Invoke(_lastId) ?? default(int);
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

        public IEnumerable<User> SearchByPredicate(Func<User, bool> f)
        {
            return _userStorage.SearchByPredicate(f);
        }
        public IEnumerable<User> SearchByPredicate(ISearchCriteria sc)
        {
            return _userStorage.SearchByPredicate(sc.Search);
        }

        public void Dump(IDump d)
        {
            d.Dump(this);
        }
        public void Dump() => Dump(_baseDump);

        #endregion methods
    }
}
