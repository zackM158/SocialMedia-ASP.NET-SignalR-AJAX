using Entities;
using iTextSharp.text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fakebook.Models
{
    public class LikedUsersModel
    {
        [Required]
        public User User { get; set; }
        public List<int> FriendRequestIds { get; set; }
        public List<int> SentFriendRequestIds { get; set; }

        public List<string> FriendIds { get; set; }

        public List<User> FriendRequests { get; set; }
        public List<User> MutualFriends { get; set; }

        public StatusInfo StatusInfo { get; set; }
    }
}