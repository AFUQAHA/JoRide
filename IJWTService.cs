using JoRide.Model;

namespace JoRide.IServices
{
    public interface IJWTService
    {
        public Task<AuthenticationResponse> CreateJwtToken(ApplicationUser user);
    }
}
