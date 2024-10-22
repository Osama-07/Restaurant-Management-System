namespace Restaurant_Data_Access.DTOs.UserDTOs
{
    public class LoginDTO
    {
        public LoginDTO(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
