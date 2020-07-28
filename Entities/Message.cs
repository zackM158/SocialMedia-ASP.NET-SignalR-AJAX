using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Message
    {
        public int MessageId { get; set; }

        [Required]
        public int SenderId { get; set; }
        [Required]
        public int RecieverId { get; set; }

        [Required]
        public string SenderName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }

        [Required, Display(Name = "Sent At")]
        public DateTime SentAt { get; set; }

        [Required]
        public bool Seen { get; set; }
    }
}
