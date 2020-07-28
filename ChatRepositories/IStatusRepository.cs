using Entities;
using System.Collections.Generic;

namespace ChatRepositories
{
    public interface IStatusRepository
    {
        List<StatusInfo> AllFriendStatuses(int userId);
        List<StatusInfo> UserStatuses(int currentUserId, int statusUserId);
        void SaveStatus(Status status);

        void DeleteStatus(int currentUserId, int statusId);

        int ToggleLike(int userId, int statusId);
        List<User> GetLikedUsers(int statusId);

        List<StatusInfo> GetNewStatuses(int userId, HashSet<int> currentStatusIds);
        StatusInfo GetStatusInfoById(int currentUserId, int statusId);
    }
}
