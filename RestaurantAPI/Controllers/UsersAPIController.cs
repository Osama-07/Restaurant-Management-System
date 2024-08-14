using Microsoft.AspNetCore.Mvc;
using Restaurant_Business;
using Restaurant_Data_Access.DTOs.UserDTOs;

namespace RestaurantAPI.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersAPIController : ControllerBase
    {
        [HttpPost(Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UsersDTO?>> AddNewUser(UsersDTO User)
        {
            if (string.IsNullOrEmpty(User.Username) || User.Username.Length > 50 ||  
                string.IsNullOrEmpty(User.Password) || User.Password.Length > 64 ||
                string.IsNullOrEmpty(User.Role) || User.Role.Length > 20)
            {
                return BadRequest("No Accept User Info");
            }

            var newUser = new clsUsers(User);

            if (await newUser.SaveAsync())
            {
                User.UserID = newUser.UserID;

                return CreatedAtRoute("GetUserByID", new { id = newUser.UserID }, User);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding User.");

        }

        [HttpGet("{id}", Name = "GetUserByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UsersDTO?>> GetUserByID(int? id)
        {
            if (id == null || id < 1 || id > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {id}");
            }

            var User = await clsUsers.GetUserByIDAsync(id);

            if (User == null)
            {
                return NotFound($"Not Found User With ID {id}");
            }

            return Ok(User.UDTO);
        }

        [HttpPut("ID/{id}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UsersDTO?>> UpdateUserAsync(int? id, UsersDTO User)
        {
            if (id == null || id < 1 || id > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {id}");
            }

            if (string.IsNullOrEmpty(User.Username) || User.Username.Length > 50 ||
                string.IsNullOrEmpty(User.Password) || User.Password.Length > 64 ||
                string.IsNullOrEmpty(User.Role) || User.Role.Length > 20)
            {
                return BadRequest("No Accept User Info");
            }

            var uUser = await clsUsers.GetUserByIDAsync(id);

            if (uUser == null)
            {
                return NotFound($"Not Found User With ID {id}.");
            }

            User.UserID = id;
            uUser = new clsUsers(User, clsUsers.enMode.Update);

            if (await uUser.SaveAsync())
            {
                return Ok(uUser.UDTO);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating User.");
        }

        [HttpGet("ID/{id}", Name = "IsUserExists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> IsUserExistsAsync(int? id)
        {
            if (id < 1 || id == null || id > int.MaxValue)
            {
                return BadRequest($"No Accept ID {id} .");
            }

            if (await clsUsers.IsUserExistsAsync(id))
            {
                return Ok($"User With ID {id} Is Exists.");
            }
            else
            {
                return NotFound($"Not Found User With ID {id} .");
            }
        }

        [HttpDelete("ID/{id}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteUserAsync(int? id)
        {
            if (id < 1 || id == null)
            {
                return BadRequest($"No Accept ID {id} .");
            }

            if (await clsUsers.IsUserExistsAsync(id))
            {
                if (await clsUsers.DeleteUserAsync(id))
                {
                    return Ok($"Deleted User With ID {id} Successfully.");
                }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete User.");
            }
            else
            {
                return NotFound($"Not Found User With ID {id} .");
            }
        }


        [HttpGet(Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UsersDTO?>>> GetAllUsersAsync()
        {
            var usersList = await clsUsers.GetAllUsersAsync();
            if (usersList == null || usersList.Count < 1)
            {
                return NotFound($"Not Found Users.");
            }

            return Ok(usersList);
        }
    }
}
