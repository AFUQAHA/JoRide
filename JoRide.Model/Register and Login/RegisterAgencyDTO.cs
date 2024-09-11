using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace JORide.Model.DTO.Register
{
    public class RegisterAgencyDTO : BaseRegister
    {
        [Required(ErrorMessage = "The Agency Name Can't Be Blank")]
        [DisplayName("Agency Name")]
        public string AgencyName { get; set; }
    }
}
