using DataLayer;
using Entities;
using System.Collections.Generic;
using System.Linq;

namespace ChatRepositories
{
    public class StatusRepository : IStatusRepository
    {
        private ChatContext context;
        private UserRepository userRepository;

        public StatusRepository(ChatContext db)
        {
            context = db;
            userRepository = new UserRepository();
        }

        public StatusRepository()
        {
            context = new ChatContext();
            userRepository = new UserRepository();
        }

        public List<StatusInfo> AllFriendStatuses(int userId)
        {
            User currentUser = context.Users.Find(userId);
            List<StatusInfo> statusInfos = new List<StatusInfo>();
            List<Status> statuses = new List<Status>();

            if (currentUser.FriendIds == null)
            {
                statuses = context.Statuses
                  .Where(s => s.UserId.ToString() == userId.ToString()).OrderByDescending(s => s.SentAt).ToList();
            }
            else
            {
                List<string> friendIds = currentUser.FriendIds.Split(',').ToList();

                statuses = context.Statuses
                     .Where(s => friendIds.Contains(s.UserId.ToString()) || s.UserId.ToString() == userId.ToString()).OrderByDescending(s => s.SentAt).ToList();
            }

            foreach (Status s in statuses)
            {
                List<User> likedUsers = GetLikedUsers(s.StatusId);
                User sender = userRepository.GetUserById(s.UserId);

                StatusInfo statusInfo = new StatusInfo(userId, s, likedUsers)
                {
                    ImageURL = sender.ImageUrl,
                    SenderName = sender.FirstName + " " + sender.LastName
                };

                statusInfos.Add(statusInfo);
            }

            return statusInfos;
        }

        public List<StatusInfo> UserStatuses(int currentUserId, int statusUserId)
        {
            List<StatusInfo> statusInfos = new List<StatusInfo>();

            List<Status> statuses = context.Statuses
                .Where(s => s.UserId == statusUserId).OrderByDescending(s => s.SentAt).ToList();

            User sender = userRepository.GetUserById(statusUserId);

            foreach (Status s in statuses)
            {
                List<User> likedUsers = GetLikedUsers(s.StatusId);

                StatusInfo statusInfo = new StatusInfo(currentUserId, s, likedUsers)
                {
                    ImageURL = sender.ImageUrl,
                    SenderName = sender.FirstName + " " + sender.LastName
                };

                statusInfos.Add(statusInfo);
            }

            return statusInfos;
        }

        public int ToggleLike(int userId, int statusId)
        {
            Status status = context.Statuses.Find(statusId);
            string userIdString = userId.ToString();


            if (status.LikedIds == null || status.LikedIds == "")
            {
                status.LikedIds = userIdString;
                status.Likes += 1;
                context.SaveChanges();
                return 1;
            }
            else
            {
                List<string> currentLikes = status.LikedIds.Split(',').ToList();

                if (currentLikes.Contains(userIdString))
                {
                    currentLikes.Remove(userIdString);

                    status.LikedIds = string.Join(",", currentLikes);
                    status.Likes -= 1;
                    context.SaveChanges();
                    return currentLikes.Count();
                }
                else
                {
                    status.LikedIds += "," + userIdString;
                    status.Likes += 1;
                    context.SaveChanges();
                    return currentLikes.Count() + 1;
                }
            }
        }

        public StatusInfo GetStatusInfoById(int currentUserId, int statusId)
        {
            Status status = context.Statuses.Find(statusId);
            List<User> likedUsers = GetLikedUsers(statusId);
            User sender = userRepository.GetUserById(status.UserId);

            StatusInfo statusInfo = new StatusInfo(currentUserId, status, likedUsers)
            {
                ImageURL = sender.ImageUrl,
                SenderName = sender.FirstName + " " + sender.LastName
            };

            return statusInfo;
        }

        public List<User> GetLikedUsers(int statusId)
        {
            Status status = context.Statuses.Find(statusId);

            if (status.LikedIds == null || status.LikedIds == "")
            {
                return null;
            }

            List<string> currentLikes = status.LikedIds.Split(',').ToList();

            List<User> likedUsers = context.Users.Where(u => currentLikes.Contains(u.UserId.ToString())).ToList();

            return likedUsers;
        }

        public void SaveStatus(Status status)
        {
            context.Statuses.Add(status);
            context.SaveChanges();
        }

        public void DeleteStatus(int currentUserId, int statusId)
        {
            Status status = context.Statuses.Find(statusId);

            if (status.UserId == currentUserId)
            {
                context.Statuses.Remove(status);
                context.SaveChanges();
            }

        }

        public List<StatusInfo> GetNewStatuses(int userId, HashSet<int> currentStatusIds)
        {
            User currentUser = context.Users.Find(userId);
            List<StatusInfo> statusInfos = new List<StatusInfo>();
            List<Status> statuses = new List<Status>();

            if (currentUser.FriendIds == null)
            {
                return null;
            }
            else
            {
                List<string> friendIds = currentUser.FriendIds.Split(',').ToList();

                statuses = context.Statuses
                     .Where(s => (friendIds.Contains(s.UserId.ToString()) || s.UserId.ToString() == userId.ToString()) && !currentStatusIds.Contains(s.StatusId)).OrderByDescending(s => s.SentAt).ToList();
            }

            if (statuses == null || statuses.Count == 0)
            {
                return null;
            }

            foreach (Status s in statuses)
            {
                List<User> likedUsers = GetLikedUsers(s.StatusId);
                User sender = userRepository.GetUserById(s.UserId);

                StatusInfo statusInfo = new StatusInfo(userId, s, likedUsers)
                {
                    ImageURL = sender.ImageUrl,
                    SenderName = sender.FirstName + " " + sender.LastName
                };

                statusInfos.Add(statusInfo);
                currentStatusIds.Add(s.StatusId);
            }

            return statusInfos;
        }
    }
}
