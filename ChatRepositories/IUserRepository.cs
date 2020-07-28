using Entities;

namespace ChatRepositories
{
    public interface IUserRepository
    {
        int AddUser(User user, string fileType);
        string UpdateUser(User user);
        string DeleteUser(int id);
        User GetUserById(int id);
        User GetUserByEmail(string email);
        string UpdatePassword(int id, string salt, string password);
    }
}
