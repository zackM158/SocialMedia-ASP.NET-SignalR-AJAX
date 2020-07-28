using System.ComponentModel.DataAnnotations;

namespace Fakebook.Models
{
    public class UpdatePasswordModel
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter A Password"), DataType(DataType.Password), MinLength(6, ErrorMessage = "Minimum Length 6"), RegularExpression("(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,20})$", ErrorMessage = "Must Be At Least 6 Characters,\nOne Uppercase Letter,\n One Lowercase Letter,\n One Numeric Character"), Display(Name = "New Password")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Confirm Your Password"), Display(Name = "Confirm Password"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}