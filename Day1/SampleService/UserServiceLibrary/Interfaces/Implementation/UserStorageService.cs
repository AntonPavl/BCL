using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions;
using UserServiceLibrary.Interfaces.Generic;
using UserServiceLibrary.Interfaces.Implementation;
using NLog;
using System.Diagnostics;

namespace UserServiceLibrary.Interfaces.Implementations
{
    [Serializable]
    public class UserStorageService : IService<User>, IDumpeable<User>
    {

        #region fields
        public ILogger Logger { set; private get; }

        private static BooleanSwitch boolSwitch = new BooleanSwitch("LogEnabled",
    "Switch in config file");

        private readonly IRepository<User> _userStorage;

        private readonly Func<int, int> _idGenerator = (a) => a + 1;

        private readonly IUserValidator _baseValidator = new UserValidator();

        private readonly List<IUserValidator> _validators = new List<IUserValidator>();

        private readonly XMLDump _baseDump = new XMLDump();

        private int _lastId;

        #endregion endfields

        #region ctors
        /// <summary>
        /// Create userService with id generation from user repository and custom logger
        /// </summary>
        /// <param name="us">user repository</param>
        /// <param name="idGenerator">Function with id generation logic</param>
        public UserStorageService(IRepository<User> us, Func<int, int> idGenerator,ILogger logger)
        {
            if (us == null || idGenerator == null || logger == null) throw new ArgumentNullException();
            _validators.Add(_baseValidator);
            this.Logger = logger;
            if (boolSwitch.Enabled)
            {
                Logger.Info("new userService start");
            }
            this._idGenerator = idGenerator;
            this._userStorage = us;
        }
        /// <summary>
        /// Create userService with id generation from user repository 
        /// </summary>
        /// <param name="us">user repository</param>
        /// <param name="idGenerator">Function with id generation logic</param>
        public UserStorageService(IRepository<User> us,Func<int,int> idGenerator) :this(us, idGenerator,LogManager.GetCurrentClassLogger())
        {
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
            if (boolSwitch.Enabled)
            {
                Logger.Info ("Start Add Validator");
            }
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
            if (boolSwitch.Enabled)
            {
                Logger.Info ("Start Remove Validator");
            }
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
            if (boolSwitch.Enabled)
            {
                Logger.Info ("Start Add");
            }
            if (user == null) throw new ArgumentNullException();
            if (_userStorage.Contains(user)) throw new UserExistsException();
            user.Id = this._idGenerator?.Invoke(this._lastId) ?? default(int);
            if (!this._validators.All(x => x.Validate(user) == true)) throw new UserIsNotValid();  
            return this._userStorage.Add(user);
        }
        /// <summary>
        /// Add user range to service
        /// </summary>
        /// <param name="users">users</param>
        /// <returns></returns>
        public IEnumerable<int> AddRange(IEnumerable<User> users)
        {
            if (boolSwitch.Enabled)
            {
                Logger.Info ("Start AddRange");
            }
            if (users == null) throw new ArgumentNullException();
            if (users.Distinct().Count() < users.Count()) throw new ArgumentException();
            foreach (var item in users)
            {
                if (!_validators.All(x => x.Validate(item) == true)) throw new UserIsNotValid();
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
            if (boolSwitch.Enabled)
            {
                Logger.Info ("Start Remove");
            }
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
            if (boolSwitch.Enabled)
            {
                Logger.Info ("Start Search");
            }
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
            if (boolSwitch.Enabled)
            {
                Logger.Info ("Start SearchByPredicate");
            }
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
            if (boolSwitch.Enabled)
            {
                Logger.Info ("Start SearchByPreicate(interface)");
            }
            if (sc == null) throw new ArgumentNullException();
            return _userStorage.SearchByPredicate(sc.Search);
        }
        /// <summary>
        /// Create userService dump
        /// </summary>
        /// <param name="d">Dumper</param>
        public void Dump(IDumper<User> d)
        {
            if (boolSwitch.Enabled)
            {
                Logger.Info ("Start Dump");
            }
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
            if (boolSwitch.Enabled)
            {
                Logger.Info ("Start GetEntitiesFromDump");
            }
            if (d == null) throw new ArgumentNullException();
            return d.GetDump();
        }

        public IEnumerable<User> GetEntities()
        {
            if (boolSwitch.Enabled)
            {
                Logger.Info ("Start GetEntities");
            }
            return _userStorage.GetEntities();
        }

        public User GetEntityById(int i)
        {
            if (boolSwitch.Enabled)
            {
                Logger.Info ("Start GetEntitiyById");
            }
            try
            { 
                return _userStorage.GetEntityById(i);
            }
            catch(ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public bool Contains(User user)
        {
            if (boolSwitch.Enabled)
            {
                Logger.Info ("Start Contains");
            }
            if (user == null) throw new ArgumentNullException();
            return _userStorage.Contains(user);
        }

        #endregion methods
    }
}
