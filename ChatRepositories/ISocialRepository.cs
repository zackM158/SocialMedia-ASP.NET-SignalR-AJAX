using Entities;
using System.Collections.Generic;

namespace ChatRepositories
{
    public interface ISocialRepository
    {
        List<User> GetAllUsers(int id);
        List<User> SearchUsers(int currentUserId, string searchTerm);
        List<User> GetAllUsersSorted(int id);
        List<User> PeopleYouMayKnow(int id);

    }
}
