using Entities;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Fakebook.Models
{
    public class UserFriendModel
    {
        public User User { get; set; }

        public int CurrentUserId { get; set; }
        public string FriendButtonText { get; set; }

        public bool Requested { get; set; }

        public string FriendButtonClass { get; set; }
    }
}