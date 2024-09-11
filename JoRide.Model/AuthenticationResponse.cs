namespace JoRide.Model
{
    public class AuthenticationResponse
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Token { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
