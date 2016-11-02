using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserServiceLibrary.Interfaces.Implementations;
using UserServiceLibrary.Exceptions;
using System.Collections.Generic;
using System.Linq;
using Moq;
using UserServiceLibrary;

namespace UserStorageServiceTests
{
    [TestClass]
    public class UserStorageServiceTests
    {
        public static User testuserFirstNameNull = new User() { FirstName = null, LastName = "asda", DateOfBirth = DateTime.Today };
        public static User testuserSecondNameNull = new User() { FirstName = "asda", LastName = null, DateOfBirth = DateTime.Today };
        public static User testuserValid = new User() { FirstName = "asda", LastName = "asda", DateOfBirth = DateTime.Today };
        //wtf?
        #region MOQ
        //
        //[TestMethod]
        //public void Training1()
        //{
        //    var mockUser = new Mock<User>();
        //    mockUser.SetupProperty(x => x.FirstName, "Mock")
        //            .SetupProperty(x => x.LastName, "Mock")
        //            .SetupProperty(x => x.DateOfBirth, DateTime.Today);
        //    var mockRepository = new Mock<IUserRepository>();
        //    mockRepository.Setup(x => x.Add(It.IsAny<User>())).Returns(new User());
        //    var t = mockRepository.Object.Add(new User());
        //    Assert.IsNotNull(t);
        //}

        #endregion

        #region ADD

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_NullUser_ExceptionThrown()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);
            uss.Add(null);
        }

        [TestMethod]
        [ExpectedException(typeof(UserFieldsNullException))]
        public void Add_FirstNameNullUser_ExceptionThrown()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);

            uss.Add(testuserFirstNameNull);
        }

        [TestMethod]
        [ExpectedException(typeof(UserFieldsNullException))]
        public void Add_LastNameNullUser_ExceptionThrown()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);

            uss.Add(testuserSecondNameNull);
        }

        [TestMethod]
        public void Add_ValidUser_Adding()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);

            var t = uss.Add(testuserValid);
            Assert.AreEqual(t, testuserValid);
        }

        [TestMethod]
        [ExpectedException(typeof(UserExistsException))]
        public void Add_ExistsUser_ExceptionThrown()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);
            uss.Add(testuserValid);
            uss.Add(testuserValid);
        }

        #endregion ADD

        #region AddRange

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddRange_NullCollection_ExceptionThrown()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);
            uss.AddRange(null);
        }

        [TestMethod]
        [ExpectedException(typeof(UserFieldsNullException))]
        public void AddRange_WrongUsers_ExceptionThrown()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);
            List<User> list = new List<User>();
            list.Add(testuserFirstNameNull);
            list.Add(testuserSecondNameNull);
            uss.AddRange(list);
        }

        [TestMethod]
        public void AddRange_ValidUsers_Adding()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);
            var valideUser = new User() { FirstName = "bbb", LastName = "bbbb", DateOfBirth = DateTime.Today };
            List<User> list = new List<User>();

            list.Add(testuserValid);
            list.Add(valideUser);

            Assert.AreEqual(Enumerable.SequenceEqual(list, uss.AddRange(list)),true);
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddRange_BadCollection_ExceptionThrown()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);
            List<User> list = new List<User>();
            list.Add(testuserValid);
            list.Add(testuserValid);
            uss.AddRange(list);
        }

        #endregion

        #region Search

        [TestMethod]
        public void Search_NullUser_ExceptionThrown()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);
            Assert.AreEqual(uss.Search(null),null);
        }

        [TestMethod]
        public void Search_ExistsUser_User()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);
            uss.Add(testuserValid);
            Assert.AreEqual(testuserValid,uss.Search(testuserValid));
        }

        [TestMethod]
        public void Search_NotExists_Null()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);
            uss.Add(testuserValid);
            Assert.AreEqual(null, uss.Search(testuserSecondNameNull));
        }

        #endregion Search

        #region SearchByPredicate

        [TestMethod]
        public void SearchByPredicate_ExistsUser_User()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);
            uss.Add(testuserValid);

            var list = uss.SearchByPredicate((x) => {
                if (x == testuserValid) return x;
                return null;
            });

            Assert.AreEqual(list.ElementAt(0), testuserValid);
        }

        [TestMethod]
        public void SearchByPredicate_NotExistsUser_EmptyCollection()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);
            uss.Add(testuserValid);

            var list = uss.SearchByPredicate((x) => {
                if (x == testuserFirstNameNull) return x;
                return null;
            });

            Assert.AreEqual(list.Count(), 0);
        }

        [TestMethod]
        public void SearchByPredicate_NullColection_EmptyCollection()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);
            var list = uss.SearchByPredicate((x) => {
                if (x == testuserFirstNameNull) return x;
                return null;
            });
            Assert.AreEqual(0, list.Count());
        }
        #endregion

        #region Delete

        [TestMethod]
        public void Delete_NullUser_false()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);

            Assert.AreEqual(uss.Remove(null),false);
        }

        [TestMethod]
        public void Delete_WrongUser_false()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);

            Assert.AreEqual(uss.Remove(testuserFirstNameNull),false);
        }

        [TestMethod]
        public void Delete_ValidUser_true()
        {
            UserRepository ur = new UserRepository();
            UserStorageService uss = new UserStorageService(ur);

            uss.Add(testuserValid);
            Assert.AreEqual(uss.Remove(testuserValid), true);
        }

        #endregion Delete
    }
}
