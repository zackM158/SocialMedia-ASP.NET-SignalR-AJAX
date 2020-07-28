using Entities;
using iTextSharp.text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fakebook.Models
{
    public class UserInfoModel
    {
        [Required]
        public User User { get; set; }
        public int CurrentUserId { get; set; }
        public List<int> FriendRequestIds { get; set; }
        public List<int> SentFriendRequestIds { get; set; }

        public List<string> FriendIds { get; set; }
        public List<User> OtherUsers { get; set; }

        public List<User> FriendRequests { get; set; }
        public List<User> MutualFriends { get; set; }
    }
}