using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Status
    {
        public int StatusId { get; set; }

        [Required]
        public int UserId { get; set; }

        //[Required]
        //public string SenderName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }

        [Required, Display(Name = "Sent At")]
        public DateTime SentAt { get; set; }

        //[Required]
        //public string ImageURL { get; set; }

        public int Likes { get; set; }
        public string LikedIds { get; set; }

    }
}
