using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Business;
using Restaurant_Data_Access.DTOs.OrderDetailsDTOs;

namespace RestaurantAPI.Controllers
{
    [Route("api/OrderDetails")]
    [ApiController]
    public class OrderDetailsAPIController : ControllerBase
    {
        [HttpPost(Name = "AddNewOrderDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderDetailsDTO?>> AddNewOrderDetails(OrderDetailsDTO OrderDetails)
        {
            if (OrderDetails.OrderID < 1 || OrderDetails.OrderID > int.MaxValue ||
                OrderDetails.MenuItemID < 1 || OrderDetails.MenuItemID > int.MaxValue || 
                OrderDetails.Quantity < 1 || OrderDetails.Quantity > int.MaxValue ||
                OrderDetails.Price < 0 || OrderDetails.Price > decimal.MaxValue)
            {
                return BadRequest("No Accept OrderDetails Info");
            }

            var newOrderDetails = new clsOrderDetails(OrderDetails);

            if (await newOrderDetails.SaveAsync())
            {
                OrderDetails.OrderDetailsID = newOrderDetails.OrderDetailsID;

                return CreatedAtRoute("GetOrderDetailsByID", new { id = newOrderDetails.OrderDetailsID }, OrderDetails);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding OrderDetails.");

        }

        [HttpGet("{id}", Name = "GetOrderDetailsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDetailsDTO?>> GetOrderDetailsByID(int? id)
        {
            if (id == null || id < 1 || id > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {id}");
            }

            var OrderDetails = await clsOrderDetails.GetOrderDetailsByIDAsync(id);

            if (OrderDetails == null)
            {
                return NotFound($"Not Found OrderDetails With ID {id}");
            }

            return Ok(OrderDetails.ODTO);
        }

        [HttpPut("ID/{id}", Name = "UpdateOrderDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDetailsDTO?>> UpdateOrderDetailsAsync(int? id, OrderDetailsDTO OrderDetails)
        {
            if (id == null || id < 1 || id > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {id}");
            }

            if (OrderDetails.OrderID < 1 || OrderDetails.OrderID > int.MaxValue ||
                OrderDetails.MenuItemID < 1 || OrderDetails.MenuItemID > int.MaxValue ||
                OrderDetails.Quantity < 1 || OrderDetails.Quantity > int.MaxValue ||
                OrderDetails.Price < 0 || OrderDetails.Price > decimal.MaxValue)
            {
                return BadRequest("No Accept OrderDetails Info");
            }

            var uOrderDetails = await clsOrderDetails.GetOrderDetailsByIDAsync(id);

            if (uOrderDetails == null)
            {
                return NotFound($"Not Found OrderDetails With ID {id}.");
            }

            OrderDetails.OrderDetailsID = id;
            uOrderDetails = new clsOrderDetails(OrderDetails, clsOrderDetails.enMode.Update);

            if (await uOrderDetails.SaveAsync())
            {
                return Ok(uOrderDetails.ODTO);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating OrderDetails.");
        }

        [HttpGet("ID/{id}", Name = "IsOrderDetailsExists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> IsOrderDetailsExistsAsync(int? id)
        {
            if (id < 1 || id == null || id > int.MaxValue)
            {
                return BadRequest($"No Accept ID {id} .");
            }

            if (await clsOrderDetails.IsOrderDetailsExistsAsync(id))
            {
                return Ok($"OrderDetails With ID {id} Is Exists.");
            }
            else
            {
                return NotFound($"Not Found OrderDetails With ID {id} .");
            }
        }

        [HttpDelete("ID/{id}", Name = "DeleteOrderDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteOrderDetailsAsync(int? id)
        {
            if (id < 1 || id == null)
            {
                return BadRequest($"No Accept ID {id} .");
            }

            if (await clsOrderDetails.IsOrderDetailsExistsAsync(id))
            {
                if (await clsOrderDetails.DeleteOrderDetailsAsync(id))
                {
                    return Ok($"Deleted OrderDetails With ID {id} Successfully.");
                }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete OrderDetails.");
            }
            else
            {
                return NotFound($"Not Found OrderDetails With ID {id} .");
            }
        }


        [HttpGet(Name = "GetAllOrderDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OrderDetailsDTO?>>> GetAllOrderDetailsAsync()
        {
            var OrderDetailsList = await clsOrderDetails.GetAllOrderDetailsAsync();
            if (OrderDetailsList == null || OrderDetailsList.Count < 1)
            {
                return NotFound($"Not Found OrderDetails.");
            }

            return Ok(OrderDetailsList);
        }
    }
}
