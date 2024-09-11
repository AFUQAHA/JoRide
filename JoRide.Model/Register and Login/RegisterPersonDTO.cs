using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JORide.Model.DTO.Register
{
    public class RegisterPersonDTO : BaseRegister
    {
        [Required(ErrorMessage = "The First Name Can't Be Blank")]
        [DisplayName("First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Last Name Can't Be Blank")]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = string.Empty;
    }
}
