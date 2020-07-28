using System.ComponentModel.DataAnnotations;

namespace Fakebook.Models
{
    public class LoginModel
    {
        [Required(AllowEmptyStrings = false), DataType(DataType.EmailAddress), Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Required(AllowEmptyStrings = false), DataType((DataType.Password))]
        public string Password { get; set; }
    }
}