using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserServiceLibrary.Interfaces.Implementations;
using UserServiceLibrary.Exceptions;
using System.Collections.Generic;
using System.Linq;
using UserServiceLibrary;

namespace UserServiceLibraryTests
{
    [TestClass]
    public class UserRepositoryTests
    {
        public static User testuserFirstNameNull = new User() { FirstName = null, LastName = "asda", DateOfBirth = DateTime.Today, VisaRecords = new List<Visa>() };
        public static User testuserSecondNameNull = new User() { FirstName = "asda", LastName = null, DateOfBirth = DateTime.Today, VisaRecords = new List<Visa>() };
        public static User testuserValid = new User() { FirstName = "asda", LastName = "asda", DateOfBirth = DateTime.Today, VisaRecords = new List<Visa>() };

        [TestMethod]
        public void Add_ValidUser_true()
        {
            var ur = new UserRepository();
            ur.Add(testuserValid);
            Assert.AreEqual(ur.GetEntityById(0), testuserValid);
        }

        [TestMethod]
        public void Remove_ValidUser_true()
        {
            var ur = new UserRepository();
            ur.Add(testuserValid);
            ur.Remove(testuserValid);
            Assert.AreEqual(ur.Count, 0);
        }

        [TestMethod]
        public void Search_ValidUser_true()
        {
            var ur = new UserRepository();
            ur.Add(testuserValid);
            Assert.AreEqual(ur.Search(testuserValid), testuserValid);
        }
        [TestMethod]
        public void Contains_ValidUser_true()
        {
            var ur = new UserRepository();
            ur.Add(testuserValid);
            Assert.AreEqual(ur.Contains(testuserValid), true);

        }
        [TestMethod]
        public void Count_num()
        {
            var ur = new UserRepository();
            ur.Add(testuserValid);
            Assert.AreEqual(ur.Count, 1);
        }

        [TestMethod]
        public void GetEntites_Entities()
        {
            var ur = new UserRepository();
            List<User> l = new List<User>();
            l.Add(testuserValid);
            l.Add(testuserValid);
            ur.Add(testuserValid);
            ur.Add(testuserValid);
            Assert.AreEqual(Enumerable.SequenceEqual(ur.GetEntities(),l), true);
        }
    }
}
