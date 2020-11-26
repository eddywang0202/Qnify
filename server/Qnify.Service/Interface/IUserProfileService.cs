namespace Qnify.Service.Interface
{
    public interface IUserProfileService
    {
        bool InsertUserProfile(string name, string age, string email, uint userId);
    }
}
