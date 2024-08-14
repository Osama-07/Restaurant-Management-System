using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Business;
using Restaurant_Data_Access.DTOs.MenuItemsDTOs;

namespace RestaurantAPI.Controllers
{
    [Route("api/MenuItems")]
    [ApiController]
    public class MenuItemsAPIController : ControllerBase
    {
        [HttpPost(Name = "AddNewMenuItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MenuItemsDTO?>> AddNewMenuItem(MenuItemsDTO MenuItem)
        {
            if (string.IsNullOrEmpty(MenuItem.Name) ||
                MenuItem.Price < 0 || MenuItem.Price > decimal.MaxValue || 
                MenuItem.CategoryID < 1 || MenuItem.CategoryID > int.MaxValue)
            {
                return BadRequest("No Accept MenuItem Name");
            }

            var newMenuItem = new clsMenuItems(MenuItem);

            if (await newMenuItem.SaveAsync())
            {
                MenuItem.MenuItemID = newMenuItem.MenuItemID;

                return CreatedAtRoute("GetMenuItemByID", new { id = newMenuItem.MenuItemID }, MenuItem);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding MenuItem.");

        }

        [HttpGet("{id}", Name = "GetMenuItemByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MenuItemsDTO?>> GetMenuItemByID(int? id)
        {
            if (id == null || id < 1 || id > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {id}");
            }

            var MenuItem = await clsMenuItems.GetMenuItemByIDAsync(id);

            if (MenuItem == null)
            {
                return NotFound($"Not Found MenuItem With ID {id}");
            }

            return Ok(MenuItem.MDTO);
        }

        [HttpPut("ID/{id}", Name = "UpdateMenuItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MenuItemsDTO?>> UpdateMenuItemAsync(int? id, MenuItemsDTO MenuItem)
        {
            if (id == null || id < 1 || id > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {id}");
            }

            if (string.IsNullOrEmpty(MenuItem.Name) ||
                MenuItem.Price < 0 || MenuItem.Price > decimal.MaxValue ||
                MenuItem.CategoryID < 1 || MenuItem.CategoryID > int.MaxValue)
            {
                return BadRequest("No Accept MenuItem Name");
            }

            var uMenuItem = await clsMenuItems.GetMenuItemByIDAsync(id);

            if (uMenuItem == null)
            {
                return NotFound($"Not Found MenuItem With ID {id}.");
            }

            MenuItem.MenuItemID = id;
            uMenuItem = new clsMenuItems(MenuItem, clsMenuItems.enMode.Update);

            if (await uMenuItem.SaveAsync())
            {
                return Ok(uMenuItem.MDTO);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating MenuItem.");
        }

        [HttpGet("ID/{id}", Name = "IsMenuItemExists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> IsMenuItemExistsAsync(int? id)
        {
            if (id < 1 || id == null || id > int.MaxValue)
            {
                return BadRequest($"No Accept ID {id} .");
            }

            if (await clsMenuItems.IsMenuItemExistsAsync(id))
            {
                return Ok($"MenuItem With ID {id} Is Exists.");
            }
            else
            {
                return NotFound($"Not Found MenuItem With ID {id} .");
            }
        }

        [HttpDelete("ID/{id}", Name = "DeleteMenuItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteMenuItemAsync(int? id)
        {
            if (id < 1 || id == null)
            {
                return BadRequest($"No Accept ID {id} .");
            }

            if (await clsMenuItems.IsMenuItemExistsAsync(id))
            {
                if (await clsMenuItems.DeleteMenuItemAsync(id))
                {
                    return Ok($"Deleted MenuItem With ID {id} Successfully.");
                }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete MenuItem.");
            }
            else
            {
                return NotFound($"Not Found MenuItem With ID {id} .");
            }
        }

        [HttpGet(Name = "GetAllMenuItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MenuItemsDTO?>>> GetAllMenuItemsAsync()
        {
            var MenuItemsList = await clsMenuItems.GetAllMenuItemsAsync();
            if (MenuItemsList == null || MenuItemsList.Count < 1)
            {
                return NotFound($"Not Found MenuItems.");
            }

            return Ok(MenuItemsList);
        }
    }
}
