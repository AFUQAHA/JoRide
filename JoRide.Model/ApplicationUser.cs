using Microsoft.AspNetCore.Identity;

namespace JoRide.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AgencyName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? HireDate { get; set; }
        public string? AgencyId { get; set; }
        public bool IsAgency { get; set; }
    }
}
