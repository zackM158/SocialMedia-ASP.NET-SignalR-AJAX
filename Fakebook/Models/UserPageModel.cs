using Entities;
using iTextSharp.text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fakebook.Models
{
    public class UserPageModel
    {
        public User User { get; set; }
        public List<StatusInfo> Statuses { get; set; }

        public List<User> Friends { get; set; }

        public int CurrentUserId { get; set; }

        public List<string> FriendIds { get; set; }

        public string FriendButtonText { get; set; }
        public bool Requested { get; set; }
        public string FriendButtonClass { get; set; }
    }
}