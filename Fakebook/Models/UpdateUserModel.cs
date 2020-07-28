using Entities;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Fakebook.Models
{
    public class UpdateUserModel
    {
        [Display(Name = "Profile Picture")]
        public HttpPostedFileBase PostedFile { get; set; }

        [Required]
        public User User { get; set; }
    }
}