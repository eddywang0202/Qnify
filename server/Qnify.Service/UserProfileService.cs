using Qnify.DAL;
using Qnify.Model;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;

namespace Qnify.Service
{
    public class UserProfileService : IUserProfileService
    {
        private static Database _database;

        public UserProfileService()
        {
            _database = new Database(Config.AppSettings);
        }

        public bool InsertUserProfile(string name, string age, string email, uint userId)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new UserProfileRepository(unitOfWork).InsertUserProfile(name, age, email, userId);
            }
        }
    }
}
