using Qnify.DAL;
using Qnify.Model;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;

namespace Qnify.Service
{
    public class UserService : IUserService
    {
        private static Database _database;

        public UserService()
        {
            _database = new Database(Config.AppSettings);
        }

        public uint InsertUser(string username)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new UserRepository(unitOfWork).InsertUser(username);
            }
        }

        public UsersResponse GetMemberUsers()
        {
            List<User> userList;
            var result = new UsersResponse();
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                userList = new UserRepository(unitOfWork).GetMemberUsers();
            }
            result.Users = userList;
            return result;
        }

        public UsersResponse GetMemberUsers(uint testId)
        {
            List<User> userList;
            var result = new UsersResponse();
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                userList = new UserRepository(unitOfWork).GetMemberUsers(testId);
            }
            result.Users = userList;
            return result;
        }

        public User GetMemberUser(string userName)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new UserRepository(unitOfWork).GetMemberUser(userName);
            }
        }

        public User GetMemberUser(uint userId)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new UserRepository(unitOfWork).GetMemberUser(userId);
            }
        }

        public UsersResponse GetUsers()
        {
            List<User> userList;
            var result = new UsersResponse();
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                userList = new UserRepository(unitOfWork).GetUsers();
            }
            result.Users = userList;
            return result;
        }
    }
}
