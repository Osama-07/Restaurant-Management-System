using System.ComponentModel.DataAnnotations;

namespace Restaurant_Data_Access.DTOs.UserDTOs
{
    public class UsersDTO
    {
        public UsersDTO(int? userid, string username, string password, string role)
        {
            this.UserID = userid;
            this.Username = username;
            this.Password = password;
            this.Role = role;
        }

        [Range(0, int.MaxValue, ErrorMessage = "UserID must be between 0 and the maximum value of an integer.")]
        public int? UserID { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string Username { get; set; } // Length: 50
        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(64, ErrorMessage = "Password cannot exceed 64 characters.")]
        public string Password { get; set; } // Length: 64
        [Required(ErrorMessage = "Role is required.")]
        [MaxLength(20, ErrorMessage = "Role cannot exceed 20 characters.")]
        public string Role { get; set; } // Length: 20
    }
}
