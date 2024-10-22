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
        public async Task<ActionResult<string>> AddNewOrderDetails(List<OrderDetailsDTO> OrderDetails)
        {
            //First Step: Check if Info is Correct.
            foreach (var orderDetails in OrderDetails)
            {
                if (orderDetails.OrderID < 1 || orderDetails.OrderID > int.MaxValue ||
                    orderDetails.MenuItemID < 1 || orderDetails.MenuItemID > int.MaxValue ||
                    orderDetails.Quantity < 1 || orderDetails.Quantity > int.MaxValue ||
                    orderDetails.Price < 0 || orderDetails.Price > decimal.MaxValue)
                {
                    return BadRequest("No Accept OrderDetails Info");
                }
            }

            bool IsAdded = await clsOrderDetails.AddNewListOrderDetailsAsync(OrderDetails);

            if (IsAdded)
            {
                return Ok("Added Successfully.");
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding OrderDetails.");

        }

        [HttpGet("{orderId}&{menuItemId}", Name = "GetOrderDetailsByOrderIDAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<OrderDetailsDTO?>>> GetOrderDetailsByOrderIDAsync(int? orderId, int? menuItemId)
        {
            if (orderId == null || orderId < 1 || orderId > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {orderId}");
            }

            var OrderDetails = await clsOrderDetails.GetOrderDetailsByOrderIDAsync(orderId, menuItemId);

            if (OrderDetails == null)
            {
                return NotFound($"Not Found OrderDetails With ID {orderId}");
            }

            return Ok(OrderDetails.ODTO);
        }

        [HttpGet("list/{orderId}", Name = "GetListOrderDetailsByOrderID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<OrderDetailsDTO?>>> GetListOrderDetailsByOrderIDAsync(int? orderId)
        {
            if (orderId == null || orderId < 1 || orderId > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {orderId}");
            }

            var OrderDetails = await clsOrderDetails.GetListOrderDetailsByOrderIDAsync(orderId);

            if (OrderDetails == null)
            {
                return NotFound($"Not Found OrderDetails With ID {orderId}");
            }

            return Ok(OrderDetails);
        }

        [HttpPut("update/{orderId}&{menuItemId}", Name = "UpdateOrderDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDetailsDTO?>> UpdateOrderDetailsAsync(int? orderId, int? menuItemId, OrderDetailsDTO OrderDetails)
        {
            if (orderId == null || orderId < 1 || orderId > int.MaxValue ||
                menuItemId == null || menuItemId < 1 || menuItemId > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {orderId}");
            }

            if (OrderDetails.OrderID < 1 || OrderDetails.OrderID > int.MaxValue ||
                OrderDetails.MenuItemID < 1 || OrderDetails.MenuItemID > int.MaxValue ||
                OrderDetails.Quantity < 1 || OrderDetails.Quantity > int.MaxValue ||
                OrderDetails.Price < 0 || OrderDetails.Price > decimal.MaxValue)
            {
                return BadRequest("No Accept OrderDetails Info");
            }

            var uOrderDetails = await clsOrderDetails.GetOrderDetailsByOrderIDAsync(orderId, menuItemId);

            if (uOrderDetails == null)
            {
                return NotFound($"Not Found OrderDetails With ID {orderId}.");
            }

            uOrderDetails = new clsOrderDetails(OrderDetails, clsOrderDetails.enMode.Update);

            if (await uOrderDetails.SaveAsync())
            {
                return Ok(uOrderDetails.ODTO);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating OrderDetails.");
        }

        [HttpGet("isExists/{orderId}&{menuItemId}", Name = "IsOrderDetailsExists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> IsOrderDetailsExistsAsync(int? orderId, int? menuItemId)
        {
            if (orderId < 1 || orderId == null || orderId > int.MaxValue ||
                menuItemId < 1 || menuItemId == null || menuItemId > int.MaxValue)
            {
                return BadRequest($"No Accept ID {orderId} .");
            }

            if (await clsOrderDetails.IsOrderDetailsExistsAsync(orderId, menuItemId))
            {
                return Ok($"OrderDetails With OrderID {orderId} Is Exists.");
            }
            else
            {
                return NotFound($"Not Found OrderDetails With OrderID {orderId} .");
            }
        }

        [HttpDelete("delete/{orderId}&{menuItemId}", Name = "DeleteOrderDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteOrderDetailsAsync(int? orderId, int? menuItemId)
        {
            if (orderId < 1 || orderId == null ||
                menuItemId < 1 || menuItemId == null)
            {
                return BadRequest($"No Accept ID {orderId} .");
            }

            if (await clsOrderDetails.IsOrderDetailsExistsAsync(orderId, menuItemId))
            {
                if (await clsOrderDetails.DeleteOrderItemAsync(orderId, menuItemId))
                {
                    return Ok($"Deleted OrderDetails With ID {orderId} Successfully.");
                }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete OrderDetails.");
            }
            else
            {
                return NotFound($"Not Found OrderDetails With ID {orderId} .");
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
