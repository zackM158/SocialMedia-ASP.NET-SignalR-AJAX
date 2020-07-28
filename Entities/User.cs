using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Entities
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please Enter A First Name"), MaxLength(20), Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter A Surname"), MaxLength(20), Display(Name = "Surname")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false), Display(Name = "Email"), MaxLength(250), DataType(DataType.EmailAddress), Index(IsUnique = true)]
        public string EmailAddress { get; set; }

        [Required(AllowEmptyStrings = false), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Salt { get; set; }

        public string ImageUrl { get; set; }
        public string FriendIds { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        //public virtual ICollection<User> Friends { get; set; }

        public DateTime DateJoined { get; set; }
    }
}
