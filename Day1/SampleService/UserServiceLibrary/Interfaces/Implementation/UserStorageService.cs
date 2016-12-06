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
        /// <summary>
        /// Create userService with id generation from user repository 
        /// </summary>
        /// <param name="us">user repository</param>
        /// <param name="idGenerator">Function with id generation logic</param>
        public UserStorageService(IRepository<User> us,Func<int,int> idGenerator)
        {
            if (us == null || idGenerator == null) throw new ArgumentNullException();
            _validators.Add(_baseValidator);
            this._idGenerator = idGenerator;
            this._userStorage = us;
        }
        /// <summary>
        /// Create userService from user repository, id => id + 1
        /// </summary>
        /// <param name="us">user repository</param>
        public UserStorageService(IRepository<User> us) : this(us, (a) => a + 1) { }

        public UserStorageService() : this(new UserRepository())
        {

        }

        #endregion ctors

        #region props
        /// <summary>
        /// Get users count
        /// </summary>
        public int Count { get { return this._userStorage.Count; } }
        /// <summary>
        /// Get last id from service
        /// </summary>
        public int GetLastId { get { return _lastId; } }

        #endregion props

        #region methods
        /// <summary>
        /// Add validator to validator set
        /// </summary>
        /// <param name="uv">Validator rule</param>
        public void AddValidator(IUserValidator uv)
        {
            if (uv == null) throw new ArgumentNullException();
            _validators.Add(uv);
        }
        /// <summary>
        /// Remove validator from validator set
        /// </summary>
        /// <param name="uv">Validator rule</param>
        /// <returns></returns>
        public bool RemoveValidator(IUserValidator uv)
        {
            if (uv == null) throw new ArgumentNullException();
            return _validators.Remove(uv);
        }
        /// <summary>
        /// Add user to service
        /// </summary>
        /// <param name="user">user</param>
        /// <returns></returns>
        public int Add(User user)
        {
            if (user == null) throw new ArgumentNullException();
            user.Id = this._idGenerator?.Invoke(this._lastId) ?? default(int);
            if (!this._validators.All(x => x.Validate(user) == true)) throw new ArgumentException();  
            return this._userStorage.Add(user);
        }
        /// <summary>
        /// Add user range to service
        /// </summary>
        /// <param name="users">users</param>
        /// <returns></returns>
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
        /// <summary>
        /// Remove user from service
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Remove(User user)
        {
            if (user == null) throw new ArgumentNullException();
            return this._userStorage.Remove(user);
        }
        /// <summary>
        /// Search user in service
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User Search(User user)
        {
            if (user == null) throw new ArgumentNullException();
            return this._userStorage.Search(user);
        }
        /// <summary>
        /// Search user by Predicate
        /// </summary>
        /// <param name="f">Predicate</param>
        /// <returns></returns>
        public IEnumerable<User> SearchByPredicate(Func<User, bool> f)
        {
            if (f == null) throw new ArgumentNullException();
            return _userStorage.SearchByPredicate(f);
        }
        /// <summary>
        /// Search user by ISearchCriteria
        /// </summary>
        /// <param name="sc">ISearchCriteria</param>
        /// <returns></returns>
        public IEnumerable<User> SearchByPredicate(ISearchCriteria<User> sc)
        {
            if (sc == null) throw new ArgumentNullException();
            return _userStorage.SearchByPredicate(sc.Search);
        }
        /// <summary>
        /// Create userService dump
        /// </summary>
        /// <param name="d">Dumper</param>
        public void Dump(IDumper<User> d)
        {
            if (d == null) throw new ArgumentNullException();
            d.Dump(_userStorage.GetEntities());
        }
        /// <summary>
        /// Create userService dump
        /// </summary>
        public void Dump() => Dump(_baseDump);
        /// <summary>
        /// Get Entites from dump 
        /// </summary>
        /// <param name="d">Dumper</param>
        /// <returns></returns>
        public IEnumerable<User> GetEntitiesFromDump(IDumper<User> d)
        {
            if (d == null) throw new ArgumentNullException();
            return d.GetDump();
        }

        #endregion methods
    }
}
