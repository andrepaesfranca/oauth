namespace oauth_service.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsEnabled { get; set; }
    }
}