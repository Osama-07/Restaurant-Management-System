using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Business;
using Restaurant_Data_Access.DTOs.InventoryDTOs;

namespace RestaurantAPI.Controllers
{
    [Route("api/Inventory")]
    [ApiController]
    public class InventoryAPIController : ControllerBase
    {
        [HttpPost(Name = "AddNewInventory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InventoryDTO?>> AddNewInventory(InventoryDTO Inventory)
        {
            if (string.IsNullOrEmpty(Inventory.ItemName) ||
                Inventory.Quantity < 0 || Inventory.Quantity > decimal.MaxValue ||
                string.IsNullOrEmpty(Inventory.Unit))
            {
                return BadRequest("No Accept Inventory Info");
            }

            var newInventory = new clsInventory(Inventory);

            if (await newInventory.SaveAsync())
            {
                Inventory.InventoryID = newInventory.InventoryID;

                return CreatedAtRoute("GetInventoryByID", new { id = newInventory.InventoryID }, Inventory);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding Inventory.");

        }

        [HttpGet("{id}", Name = "GetInventoryByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<InventoryDTO?>> GetInventoryByID(int? id)
        {
            if (id == null || id < 1 || id > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {id}");
            }

            var Inventory = await clsInventory.GetInventoryByIDAsync(id);

            if (Inventory == null)
            {
                return NotFound($"Not Found Inventory With ID {id}");
            }

            return Ok(Inventory.IDTO);
        }

        [HttpPut("ID/{id}", Name = "UpdateInventory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<InventoryDTO?>> UpdateInventoryAsync(int? id, InventoryDTO Inventory)
        {
            if (id == null || id < 1 || id > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {id}");
            }

            if (string.IsNullOrEmpty(Inventory.ItemName) ||
                Inventory.Quantity < 0 || Inventory.Quantity > decimal.MaxValue ||
                string.IsNullOrEmpty(Inventory.Unit))
            {
                return BadRequest("No Accept Inventory Info");
            }

            var uInventory = await clsInventory.GetInventoryByIDAsync(id);

            if (uInventory == null)
            {
                return NotFound($"Not Found Inventory With ID {id}.");
            }

            Inventory.InventoryID = id;
            uInventory = new clsInventory(Inventory, clsInventory.enMode.Update);

            if (await uInventory.SaveAsync())
            {
                return Ok(uInventory.IDTO);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Inventory.");
        }

        [HttpGet("ID/{id}", Name = "IsInventoryExists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> IsInventoryExistsAsync(int? id)
        {
            if (id < 1 || id == null || id > int.MaxValue)
            {
                return BadRequest($"No Accept ID {id} .");
            }

            if (await clsInventory.IsInventoryExistsAsync(id))
            {
                return Ok($"Inventory With ID {id} Is Exists.");
            }
            else
            {
                return NotFound($"Not Found Inventory With ID {id} .");
            }
        }

        [HttpDelete("ID/{id}", Name = "DeleteInventory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteInventoryAsync(int? id)
        {
            if (id < 1 || id == null)
            {
                return BadRequest($"No Accept ID {id} .");
            }

            if (await clsInventory.IsInventoryExistsAsync(id))
            {
                if (await clsInventory.DeleteInventoryAsync(id))
                {
                    return Ok($"Deleted Inventory With ID {id} Successfully.");
                }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete Inventory.");
            }
            else
            {
                return NotFound($"Not Found Inventory With ID {id} .");
            }
        }


        [HttpGet(Name = "GetAllInventory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InventoryDTO?>>> GetAllInventoryAsync()
        {
            var InventoryList = await clsInventory.GetAllInventoryAsync();
            if (InventoryList == null || InventoryList.Count < 1)
            {
                return NotFound($"Not Found Inventory.");
            }

            return Ok(InventoryList);
        }
    }
}
