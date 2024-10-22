namespace Restaurant_Data_Access.DTOs.UserDTOs
{
    public class UsersInfoWithoutPasswordDTO
    {
		public UsersInfoWithoutPasswordDTO(int userid, string username, string role)
		{
			this.UserID = userid;
			this.Username = username;
			this.Role = role;
		}

		public int UserID { get; set; }
		public string Username { get; set; } // Length: 50
		public string Role { get; set; } // Length: 20
	}
}
