using Qnify.Model;

namespace Qnify.Service.Interface
{
    public interface IUserService
    {
        UsersResponse GetUsers();
        UsersResponse GetMemberUsers();
        UsersResponse GetMemberUsers(uint testId);
        User GetMemberUser(string userName);
        User GetMemberUser(uint userId);
        uint InsertUser(string username);
    }
}
