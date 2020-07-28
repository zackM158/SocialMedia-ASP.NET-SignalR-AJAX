using DataLayer;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace ChatRepositories
{
    public class SocialRepository : ISocialRepository
    {
        private ChatContext context;
        public SocialRepository(ChatContext db)
        {
            context = db;
        }

        public SocialRepository()
        {
            context = new ChatContext();
        }


        public List<User> GetAllUsers(int id)
        {
            List<User> users = context.Users.Where(u => u.UserId != id).OrderBy(u => u.FirstName).ToList();

            return users;
        }

        public List<User> GetAllUsersSorted(int id)
        {
            List<User> otherUsers = context.Users.Where(u => u.UserId != id).ToList();
            User currentUser = context.Users.Find(id);
            List<User> sortedUsers = new List<User>();

            if (currentUser.FriendIds != null && currentUser.FriendIds != "")
            {
                List<string> currentFriendIds = currentUser.FriendIds.Split(',').ToList();
                Dictionary<User, int> UsersWithMutualFriends = new Dictionary<User, int>();

                foreach (User user in otherUsers)
                {
                    int mutualFriends = 0;

                    if (user.FriendIds != null && user.FriendIds != "")
                    {
                        List<string> otherUserFriendIds = user.FriendIds.Split(',').ToList();
                        IEnumerable<string> mutualIds = currentFriendIds.Where(i => otherUserFriendIds.Contains(i));

                        if (mutualIds != null)
                            mutualFriends = mutualIds.Count();
                    }

                    UsersWithMutualFriends.Add(user, mutualFriends);
                }


                sortedUsers = UsersWithMutualFriends.OrderByDescending(u => u.Value).ThenBy(u => u.Key.FirstName).Select(u => u.Key).ToList(); //
            }


            return sortedUsers;
        }

        public List<User> PeopleYouMayKnow(int id)
        {
            List<User> otherUsers = context.Users.Where(u => u.UserId != id).ToList();
            User currentUser = context.Users.Find(id);
            List<User> sortedUsers = new List<User>();

            if (currentUser.FriendIds != null && currentUser.FriendIds != "")
            {
                List<string> currentFriendIds = currentUser.FriendIds.Split(',').ToList();
                Dictionary<User, int> UsersWithMutualFriends = new Dictionary<User, int>();

                foreach (User user in otherUsers.Where(u => !currentFriendIds.Contains(u.UserId.ToString())))
                {
                    int mutualFriends = 0;

                    if (user.FriendIds != null && user.FriendIds != "")
                    {
                        List<string> otherUserFriendIds = user.FriendIds.Split(',').ToList();
                        List<string> mutualIds = currentFriendIds.Where(i => otherUserFriendIds.Contains(i)).ToList();

                        if (mutualIds != null && mutualIds.Count() != 0)
                        {
                            mutualFriends = mutualIds.Count();
                            UsersWithMutualFriends.Add(user, mutualFriends);
                        }

                    }

                }

                sortedUsers = UsersWithMutualFriends.OrderByDescending(u => u.Value).ThenBy(u => u.Key.FirstName).Select(u => u.Key).ToList(); //
            }


            return sortedUsers;
        }

        public List<User> SearchUsers(int currentUserId, string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            User currentUser = context.Users.Find(currentUserId);

            string[] searchTermSplit = searchTerm.Split();

            List<User> users = new List<User>();

            if (searchTermSplit.Count() == 1)
                users = context.Users.Where(u => u.FirstName.ToLower().StartsWith(searchTerm) || u.LastName.ToLower().StartsWith(searchTerm)).ToList();
            else
            {
                string firstName = searchTermSplit[0];
                string lastName = searchTermSplit[1];
                users = context.Users.Where(u => u.FirstName.StartsWith(firstName) && u.LastName.StartsWith(lastName)).ToList();
            }

            return users;

        }
    }
}
