using Restaurant_Data_Access;
using Restaurant_Data_Access.DTOs.UserDTOs;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_Business
{
    public class clsUsers
    {
        public enum enMode { AddNew = 1, Update = 2 }
        public enMode Mode;

        public UsersDTO UDTO
        {
            get
            {
                return new UsersDTO
                 (
                    this.UserID,
                    this.Username,
                    this.Password,
                    this.Role
                 );
            }
        }
        public clsUsers(UsersDTO UDTO, enMode mode = enMode.AddNew)
        {
            this.UserID = UDTO.UserID;
            this.Username = UDTO.Username;
            this.Password = UDTO.Password;
            this.Role = UDTO.Role;

            this.Mode = mode;
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

        private bool _AddNewUser()
        {
            this.UserID = clsUsersData.AddNewUser(UDTO);

            return (this.UserID > 0);
        }

        private async Task<bool> _AddNewUserAsync()
        {
            this.UserID = await clsUsersData.AddNewUserAsync(UDTO);

            return (this.UserID > 0);
        }

        private bool _UpdateUser()
        {
            return clsUsersData.UpdateUser(UDTO);
        }

        private async Task<bool> _UpdateUserAsync()
        {
            return await clsUsersData.UpdateUserAsync(UDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateUser();
            }
            return false;
        }
        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewUserAsync())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return await _UpdateUserAsync();
            }
            return false;
        }

        public static clsUsers? GetUserByID(int? id)
        {
            if (id < 1 || id == null) return null;

            UsersDTO? uDTO = clsUsersData.GetUserByID(id);

            if (uDTO != null)
            {
                return new clsUsers(uDTO, enMode.Update);
            }
            else
                return null;
        }
        public static async Task<clsUsers?> GetUserByIDAsync(int? id)
        {
            if (id < 1 || id == null) return null;

            UsersDTO? uDTO = await clsUsersData.GetUserByIDAsync(id);

            if (uDTO != null)
            {
                return new clsUsers(uDTO, enMode.Update);
            }
            else
                return null;
        }

        public static UserPsswordDTO? GetUserPassword(int? id)
        {
            if (id < 1 || id == null) return null;

            var upDTO = clsUsersData.GetUserPassword(id);

            if (upDTO != null)
            {
                return upDTO;
            }
            else
                return null;
        }
        public static async Task<UserPsswordDTO?> GetUserPasswordAsync(int? id)
        {
            if (id < 1 || id == null) return null;

            var upDTO = await clsUsersData.GetUserPasswordAsync(id);

            if (upDTO != null)
            {
                return upDTO;
            }
            else
                return null;
        }

        public static bool SetUserPassword(UserPsswordDTO newUpDTO)
        {
            if (newUpDTO == null) return false;

            return clsUsersData.SetUserPassword(newUpDTO);
        }
        public static async Task<bool> SetUserPasswordAsync(UserPsswordDTO newUpDTO)
        {
            if (newUpDTO == null) return false;

            return await clsUsersData.SetUserPasswordAsync(newUpDTO);
        }

        public static bool IsUserExists(int? id)
        {
            if (id < 1 || id == null) return false;

            return clsUsersData.IsUserExists(id);
        }
        public static async Task<bool> IsUserExistsAsync(int? id)
        {
            if (id < 1 || id == null) return false;

            return await clsUsersData.IsUserExistsAsync(id);
        }

		public static UsersInfoWithoutPasswordDTO? VerifyLoginCredentials(LoginDTO login)
		{
			if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password)) return null;

			return clsUsersData.VerifyLoginCredentials(login);
		}
		public static async Task<UsersInfoWithoutPasswordDTO?> VerifyLoginCredentialsAsync(LoginDTO login)
		{
			if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password)) return null;

			return await clsUsersData.VerifyLoginCredentialsAsync(login);
		}

		public static bool DeleteUser(int? id)
        {
            if (id < 1 || id == null) return false;

            return clsUsersData.DeleteUser(id);
        }
        public static async Task<bool> DeleteUserAsync(int? id)
        {
            if (id < 1 || id == null) return false;

            return await clsUsersData.DeleteUserAsync(id);
        }

        public static List<UsersDTO?> GetAllUsers()
        {
            return clsUsersData.GetAllUsers();
        }
        public static async Task<List<UsersDTO?>> GetAllUsersAsync()
        {
            return await clsUsersData.GetAllUsersAsync();
        }

    }
}
