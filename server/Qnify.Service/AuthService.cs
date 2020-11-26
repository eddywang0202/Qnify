using Qnify.DAL;
using Qnify.Model;
using Qnify.Service.Interface;
using Qnify.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qnify.Service
{
    public class AuthService : IAuthService
    {

        private static Database _database;

        public AuthService()
        {
            _database = new Database(Config.AppSettings);
        }

        public User ValidateCredential(string username, string password)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new UserRepository(unitOfWork).GetUser(username, password);
            }
        }
    }
 }
