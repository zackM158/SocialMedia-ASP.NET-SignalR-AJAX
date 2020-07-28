using Entities;
using System.Collections.Generic;

namespace ChatRepositories
{
    public interface IFriendRepository
    {
        List<User> GetAllFriends(int id);
        string ToggleFriend(int currentUserId, int friendId);
        List<int> GetFriendRequestIds(int userId);
        List<int> GetSentFriendRequestIds(int userId);
        List<FriendRequest> GetAllFriendRequests(int userId);
        List<User> GetAllFriendRequestUsers(List<FriendRequest> friendRequests);
        void SendFriendRequest(int currentUserId, int friendId);
        void RemoveFriend(int currentUserId, int friendId);
        void RemoveFriendRequest(int currentUserId, int friendId);
        void AcceptFriendRequest(int currentUserId, int friendId);

        List<User> GetMutualFriends(int currentUserId, int friendId);
        void MarkRequestAsSeen(FriendRequest friendRequest);
        bool CheckForNewFriendRequest(int userId);

    }
}
