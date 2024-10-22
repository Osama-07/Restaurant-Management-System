using System.ComponentModel.DataAnnotations;

namespace Restaurant_Data_Access.DTOs.UserDTOs
{
    public class UserPsswordDTO
    {
        public UserPsswordDTO(int userID, string password) 
        {
            this.UserID = userID;
            this.Password = password;
        }

        [Range(0, int.MaxValue, ErrorMessage = "UserID must be between 0 and the maximum value of an integer.")]
        public int UserID { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(64, ErrorMessage = "Password cannot exceed 64 characters.")]
        public string Password { get; set; } = null!; // Length: 64
    }
}
