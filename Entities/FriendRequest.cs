using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Entities
{
    public class FriendRequest
    {
        public int FriendRequestId { get; set; }

        [Required]
        public int Senderid { get; set; }

        [Required]
        public int Recieverid { get; set; }

        [Required]
        public bool Seen { get; set; }

    }
}
