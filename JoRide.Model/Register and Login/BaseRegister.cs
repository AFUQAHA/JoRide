using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JORide.Model.DTO.Register
{
    public class BaseRegister
    {
        [Required(ErrorMessage = "The Email Can't Be Blank")]
        [EmailAddress(ErrorMessage ="The Email Should Be In The Format : Test@Domain.com")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Username Can't Be Blank")]
        [DisplayName("User Name")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Phone Number Can't Be Blank")]
        [DisplayName("Phone Number")]
        [RegularExpression("^[0-9]*$") , Length(10,10,ErrorMessage = "The Phone Number Can't Be Less Than or Greater Than 10 Digit")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Password Number Can't Be Blank")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Confirm Password Can't Be Blank")]
        [DisplayName("Confirm Password")]
        [Compare("Password" , ErrorMessage = "The Confirm Password must Match with Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
