using DataLayer;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace ChatRepositories
{
    public class FriendRepository : IFriendRepository
    {
        private ChatContext context;
        public FriendRepository(ChatContext db)
        {
            context = db;
        }

        public FriendRepository()
        {
            context = new ChatContext();
        }

        public List<User> GetAllFriends(int id)
        {
            User currentUser = context.Users.Find(id);

            if (currentUser.FriendIds == null)
                return null;

            List<string> friendIds = currentUser.FriendIds.Split(',').ToList();
            List<User> friends = context.Users.Where(u => friendIds.Contains(u.UserId.ToString())).ToList();
            return friends;
        }


        public string ToggleFriend(int currentUserId, int friendId)
        {
            User currentUser = context.Users.Find(currentUserId);
            User friend = context.Users.Find(friendId);

            bool friendSentRequest = context.FriendRequests.Any(f => (f.Senderid == friendId && f.Recieverid == currentUserId));
            bool sentFriendRequest = context.FriendRequests.Any(f => (f.Senderid == currentUserId && f.Recieverid == friendId));

            if (friendSentRequest)
            {
                AcceptFriendRequest(currentUserId, friendId);
                return "Remove Friend";
            }
            else if (sentFriendRequest)
            {
                RemoveFriendRequest(currentUserId, friendId);
                return "Add Friend";
            }
            else
            {

                if (currentUser.FriendIds == null || currentUser.FriendIds == "")
                {
                    SendFriendRequest(currentUserId, friendId);
                    return "Cancel Request";
                }
                else
                {
                    List<string> currentFriendIds = currentUser.FriendIds.Split(',').ToList();

                    if (currentFriendIds.Contains(friendId.ToString()))
                    {
                        RemoveFriend(currentUserId, friendId);
                        return "Add Friend";
                    }
                    else
                    {
                        SendFriendRequest(currentUserId, friendId);
                        return "Cancel Request";
                    }
                }
            }
        }

        public List<int> GetFriendRequestIds(int userId)
        {
            List<FriendRequest> friendRequests = context.FriendRequests.Where(f => f.Recieverid == userId).ToList();
            List<int> friendRequestIds = new List<int>();

            foreach (FriendRequest friendRequest in friendRequests)
            {
                friendRequestIds.Add(friendRequest.Senderid);
            }
            return friendRequestIds;

        }

        public List<int> GetSentFriendRequestIds(int userId)
        {
            List<FriendRequest> sentFriendRequests = context.FriendRequests.Where(f => f.Senderid == userId).ToList();
            List<int> sentFriendRequestIds = new List<int>();

            foreach (FriendRequest sentFriendRequest in sentFriendRequests)
            {
                sentFriendRequestIds.Add(sentFriendRequest.Recieverid);
            }
            return sentFriendRequestIds;
        }

        public List<FriendRequest> GetAllFriendRequests(int userId)
        {
            List<FriendRequest> friendRequests = context.FriendRequests.Where(f => f.Recieverid == userId).ToList();

            return friendRequests;
        }

        public List<User> GetAllFriendRequestUsers(List<FriendRequest> friendRequests)
        {
            List<User> friendRequestUsers = new List<User>();

            foreach (FriendRequest friendRequest in friendRequests)
            {
                User user = context.Users.Find(friendRequest.Senderid);

                friendRequestUsers.Add(user);
            }

            return friendRequestUsers;
        }

        public void SendFriendRequest(int currentUserId, int friendId)
        {
            FriendRequest friendRequest = new FriendRequest()
            {
                Senderid = currentUserId,
                Recieverid = friendId,
                Seen = false
            };

            context.FriendRequests.Add(friendRequest);
            context.SaveChanges();
        }

        public void RemoveFriend(int currentUserId, int friendId)
        {
            User currentUser = context.Users.Find(currentUserId);
            User friend = context.Users.Find(friendId);

            List<string> currentFriendIds = currentUser.FriendIds.Split(',').ToList();

            List<string> otherFriendIds = friend.FriendIds.Split(',').ToList();

            currentFriendIds.Remove(friendId.ToString());
            otherFriendIds.Remove(currentUserId.ToString());

            currentUser.FriendIds = string.Join(",", currentFriendIds);
            friend.FriendIds = string.Join(",", otherFriendIds);

            context.SaveChanges();
        }

        public void RemoveFriendRequest(int currentUserId, int friendId)
        {
            FriendRequest friendRequest = context.FriendRequests.SingleOrDefault(f => (f.Senderid == currentUserId && f.Recieverid == friendId) || (f.Senderid == friendId && f.Recieverid == currentUserId));

            if (friendRequest != null)
            {
                context.FriendRequests.Remove(friendRequest);
                context.SaveChanges();
            }
        }

        public void AcceptFriendRequest(int currentUserId, int friendId)
        {
            User currentUser = context.Users.Find(currentUserId);
            User friend = context.Users.Find(friendId);

            string currentUserIdString = currentUser.UserId.ToString();
            string friendIdString = friend.UserId.ToString();

            if (currentUser.FriendIds == null || currentUser.FriendIds == "")
            {
                currentUser.FriendIds = friendIdString;
                if (friend.FriendIds == null || friend.FriendIds == "")
                {
                    friend.FriendIds = currentUserIdString;
                }
                else
                {
                    friend.FriendIds += "," + currentUserIdString;
                }
            }
            else if (friend.FriendIds == null || friend.FriendIds == "")
            {
                friend.FriendIds = currentUserIdString;
                if (currentUser.FriendIds == null || currentUser.FriendIds == "")
                {
                    currentUser.FriendIds = friendIdString;
                }
                else
                {
                    currentUser.FriendIds += "," + friendIdString;
                }
            }
            else
            {
                currentUser.FriendIds += "," + friendIdString;
                friend.FriendIds += "," + currentUserIdString;
            }

            context.SaveChanges();
            RemoveFriendRequest(currentUserId, friendId);
        }

        public List<User> GetMutualFriends(int currentUserId, int friendId)
        {
            //get a list of all the users where the numbers are common in both lists
            User currentUser = context.Users.Find(currentUserId);
            User other = context.Users.Find(friendId);

            string currentUserIdString = currentUser.UserId.ToString();
            string friendIdString = other.UserId.ToString();

            if (currentUser.FriendIds == null || currentUser.FriendIds == "" || other.FriendIds == null || other.FriendIds == "")
                return null;

            List<string> currentFriendIds = currentUser.FriendIds.Split(',').ToList();
            List<string> otherFriendIds = other.FriendIds.Split(',').ToList();

            List<string> matchingIds = currentFriendIds.Where(i => otherFriendIds.Contains(i)).ToList();

            if (matchingIds == null || matchingIds.Count() == 0)
                return null;

            List<User> mutualFriends = new List<User>();

            foreach (string id in matchingIds)
            {
                int idNum = int.Parse(id);
                User mutualFriend = context.Users.Find(idNum);
                mutualFriends.Add(mutualFriend);
            }

            return mutualFriends.OrderBy(u => u.FirstName).ToList();
        }

        public void MarkRequestAsSeen(FriendRequest friendRequest)
        {
            friendRequest.Seen = true;
            context.SaveChanges();
        }

        public bool CheckForNewFriendRequest(int userId)
        {
            bool request = false;

            request = context.FriendRequests.Any(f => (f.Recieverid == userId) && !f.Seen);

            return request;
        }

    }
}
