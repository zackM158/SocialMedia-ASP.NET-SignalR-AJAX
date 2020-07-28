using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Fakebook.Models
{
    public class RegisterModel
    {
        [Display(Name = "Profile Picture")]
        public HttpPostedFileBase PostedFile { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter A First Name"), Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter A Last Name"), Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter An Email"), Display(Name = "Email"), DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter A Password"), DataType(DataType.Password), MinLength(6, ErrorMessage = "Minimum Length 6"), RegularExpression("(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,20})$", ErrorMessage = "Must Be At Least 6 Characters,\nOne Uppercase Letter,\n One Lowercase Letter,\n One Numeric Character")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Confirm Your Password"), Display(Name = "Confirm Password"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}