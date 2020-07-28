using System.Collections.Generic;
using System.Linq;

namespace Entities
{
    public class StatusInfo
    {
        public int UserId { get; set; }
        public Status Status { get; set; }
        public List<User> LikedUsers { get; set; }

        public bool Liked { get; set; }

        public string SenderName { get; set; }
        public string ImageURL { get; set; }

        public StatusInfo(int UserId, Status Status, List<User> LikedUsers)
        {
            this.UserId = UserId;
            this.Status = Status;
            this.LikedUsers = LikedUsers;

            if (LikedUsers == null)
                Liked = false;
            else
                Liked = LikedUsers.Any(u => u.UserId == UserId);
        }
    }
}