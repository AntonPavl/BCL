using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions;
using UserServiceLibrary.Interfaces.Generic;
using UserServiceLibrary.Interfaces.Implementation;

namespace UserServiceLibrary.Interfaces.Implementations
{
    [Serializable]
    public class UserStorageService : IService<User>, IDumpeable<User>
    {

        #region fields

        private readonly IRepository<User> _userStorage;

        private readonly Func<int, int> _idGenerator = (a) => a + 1;

        private readonly IUserValidator _baseValidator = new UserValidator();

        private readonly List<IUserValidator> _validators = new List<IUserValidator>();

        private readonly XMLDump _baseDump = new XMLDump();

        private int _lastId;

        #endregion endfields

        #region ctors
        public UserStorageService(IRepository<User> us,Func<int,int> idGenerator)
        {
            if (us == null || idGenerator == null) throw new ArgumentNullException();
            _validators.Add(_baseValidator);
            this._idGenerator = idGenerator;
            this._userStorage = us;
        }

        public UserStorageService(IRepository<User> us) : this(us, (a) => a + 1) { }

        public UserStorageService() : this(new UserRepository())
        {

        }

        #endregion ctors

        #region props
        public int Count { get { return this._userStorage.Count; } }
        public int GetLastId { get { return _lastId; } }

        #endregion props

        #region methods
        public void AddValidator(IUserValidator uv)
        {
            if (uv == null) throw new ArgumentNullException();
            _validators.Add(uv);
        }
        public bool RemoveValidator(IUserValidator uv)
        {
            if (uv == null) throw new ArgumentNullException();
            return _validators.Remove(uv);
        }
        public int Add(User user)
        {
            if (user == null) throw new ArgumentNullException();
            user.Id = this._idGenerator?.Invoke(this._lastId) ?? default(int);
            if (!this._validators.All(x => x.Validate(user) == true)) throw new ArgumentException();  
            return this._userStorage.Add(user);
        }

        public IEnumerable<int> AddRange(IEnumerable<User> users)
        {
            if (users == null) throw new ArgumentNullException();
            if (users.Distinct().Count() < users.Count()) throw new ArgumentException();
            foreach (var item in users)
            {
                if (!_validators.All(x => x.Validate(item) == true)) throw new ArgumentException();
                item.Id = _idGenerator?.Invoke(_lastId) ?? default(int);
            }
            return this._userStorage.AddRange(users);
        }

        public bool Remove(User user)
        {
            if (user == null) throw new ArgumentNullException();
            return this._userStorage.Remove(user);
        }

        public User Search(User user)
        {
            if (user == null) throw new ArgumentNullException();
            return this._userStorage.Search(user);
        }

        public IEnumerable<User> SearchByPredicate(Func<User, bool> f)
        {
            if (f == null) throw new ArgumentNullException();
            return _userStorage.SearchByPredicate(f);
        }
        public IEnumerable<User> SearchByPredicate(ISearchCriteria<User> sc)
        {
            if (sc == null) throw new ArgumentNullException();
            return _userStorage.SearchByPredicate(sc.Search);
        }

        public void Dump(IDumper<User> d)
        {
            if (d == null) throw new ArgumentNullException();
            d.Dump(_userStorage.GetEntities());
        }
        public void Dump() => Dump(_baseDump);

        public IEnumerable<User> GetEntitiesFromDump(IDumper<User> d)
        {
            if (d == null) throw new ArgumentNullException();
            return d.GetDump();
        }

        #endregion methods
    }
}
