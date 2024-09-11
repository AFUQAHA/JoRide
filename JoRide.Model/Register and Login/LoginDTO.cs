using System.ComponentModel.DataAnnotations;

namespace JORide.Model.DTO.Register_and_Login
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="The Email Can't be Blank")]
        [EmailAddress(ErrorMessage = "The Email Should Be In The Format : Test@Domain.com")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Password Can't be Blank")]
        public string Password { get; set; } = string.Empty;
    }
}
