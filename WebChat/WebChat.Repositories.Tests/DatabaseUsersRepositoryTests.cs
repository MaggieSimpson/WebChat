using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using WebChat.Models;
using WebChat.Data.Repositories;
using System.Transactions;
using WebChat.Data;

namespace WebChat.Repositories.Tests
{
    [TestClass]
    public class DatabaseUsersRepositoryTests
    {
        public DbContext dbContext { get; set; }

        public IRepository<User> usersRepository { get; set; }

        private static TransactionScope tranScope;

        public DatabaseUsersRepositoryTests()
        {
            this.dbContext = new WebChatContext();
            this.usersRepository = new DatabaseRepository<User>(this.dbContext);
        }

        [TestInitialize]
        public void TestInit()
        {
            tranScope = new TransactionScope();
        }

        [TestCleanup]
        public void TestTearDown()
        {
            tranScope.Dispose();
        }

        [TestMethod]
        public void Add_WhenUsernameAndPassAreValid_ShouldAddUserToDatabase()
        {
            var username = "Test username";
            var password = "12345";

            var user = new User()
            {
                Username = username,
                Password = password
            };

            this.usersRepository.Add(user);
            var foundUser = this.dbContext.Set<User>().Find(user.UserId);
            Assert.IsNotNull(foundUser);
            Assert.AreEqual(username, foundUser.Username);
            Assert.AreEqual(password, foundUser.Password);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Add_WhenUserIsNull_ShouldThrowNullReferenceException()
        {
            this.usersRepository.Add(null);
        }

        [TestMethod]
        public void Get_WhenUsernameAndPassAreValid_ShouldAddUserToDatabase()
        {
            var username = "Test username";
            var password = "12345";

            var user = new User()
            {
                Username = username,
                Password = password
            };

            this.usersRepository.Add(user);
            var foundUser = this.dbContext.Set<User>().Find(user.UserId);
            Assert.IsNotNull(foundUser);
            Assert.AreEqual(username, foundUser.Username);
            Assert.AreEqual(password, foundUser.Password);
        }
    }
}
