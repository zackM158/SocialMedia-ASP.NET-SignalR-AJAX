using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using ZacksLibrary;

namespace ChatRepositories
{
    public class UserRepository : IUserRepository
    {
        private ChatContext context;
        public UserRepository(ChatContext db)
        {
            context = db;
        }

        public UserRepository()
        {
            context = new ChatContext();
        }

        public int AddUser(User user, string fileType)
        {
            try
            {
                user.FirstName = ExtraMethods.TitleString(user.FirstName);
                user.LastName = ExtraMethods.TitleString(user.LastName);
                user.DateJoined = DateTime.Now;
                context.Users.Add(user);
                context.SaveChanges();

                if (fileType != null)
                {
                    user.ImageUrl = "User_ID_" + user.UserId + fileType;
                    context.SaveChanges();
                }
                return user.UserId;
            }
            catch
            {
                return 0;
            }
        }

        public void ChangeImageURL(User user, string fileType)
        {
            User userToUpdate = context.Users.Find(user.UserId);
            userToUpdate.ImageUrl = "User_ID_" + user.UserId + fileType;
            context.SaveChanges();
        }

        public void RemoveAllFriends(int currentUserId)
        {
            User currentUser = context.Users.Find(currentUserId);

            if (currentUser.FriendIds != null && currentUser.FriendIds != "")
            {
                List<string> currentFriendIds = currentUser.FriendIds.Split(',').ToList();
                currentUser.FriendIds = "";
                List<User> friends = context.Users.Where(u => currentFriendIds.Contains(u.UserId.ToString())).ToList();

                foreach (User friend in friends)
                {
                    List<string> otherFriendIds = friend.FriendIds.Split(',').ToList();
                    otherFriendIds.Remove(currentUserId.ToString());
                    friend.FriendIds = string.Join(",", otherFriendIds);
                }

            }
        }

        public string DeleteUser(int id)
        {
            User user = context.Users.Find(id);

            if (user != null)
            {
                var friendRequests = context.FriendRequests.Where(f => f.Senderid == id || f.Recieverid == id);
                foreach (var f in friendRequests)
                {
                    context.FriendRequests.Remove(f);
                }

                var messages = context.Messages.Where(m => m.SenderId == id || m.RecieverId == id);
                foreach (var m in messages)
                {
                    context.Messages.Remove(m);
                }

                var statuses = context.Statuses.Where(s => s.UserId == id);
                foreach (var s in statuses)
                {
                    context.Statuses.Remove(s);
                }

                RemoveAllFriends(id);


                context.Users.Remove(user);
                context.SaveChanges();
                return "User With ID " + id + " Successfully Removed";
            }
            else
            {
                return "No User With ID Of " + id;
            }
        }

        public User GetUserByEmail(string email)
        {
            User user = context.Users.SingleOrDefault(u => u.EmailAddress == email);
            return user; // UserToUserDto(user);
        }

        public User GetUserById(int id)
        {
            User user = context.Users.SingleOrDefault(u => u.UserId == id);

            if (user == null)
            {
                user = new User()
                {
                    UserId = -1,
                    FirstName = "Fakebook",
                    LastName = "User",
                    ImageUrl = "DefaultProfile.jpg"
                };
            }

            return user; //UserToUserDto(user);
        }

        public string UpdateUser(User user)
        {
            User userToUpdate = context.Users.Find(user.UserId);

            user.FirstName = ExtraMethods.TitleString(user.FirstName);
            user.LastName = ExtraMethods.TitleString(user.LastName);

            if (userToUpdate != null)
            {
                if (user.EmailAddress != userToUpdate.EmailAddress && GetUserByEmail(user.EmailAddress) != null)
                {
                    return "Email Taken";
                }

                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                userToUpdate.EmailAddress = user.EmailAddress;

                context.SaveChanges();
                return "Updated User With ID " + user.UserId;
            }
            else
            {
                return "There Is No User With ID Of " + user.UserId;
            }
        }

        public string UpdatePassword(int id, string salt, string password)
        {
            User userToUpdate = context.Users.Find(id);

            if (userToUpdate != null)
            {
                userToUpdate.Salt = salt;
                userToUpdate.Password = password;
                context.SaveChanges();
                return "Updated Password";
            }
            else
            {
                return "There Is No User With ID Of " + id;
            }
        }

    }
}
