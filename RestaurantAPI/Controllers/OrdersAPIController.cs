using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Business;
using Restaurant_Data_Access.DTOs.OrderDTOs;

namespace RestaurantAPI.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrdersAPIController : ControllerBase
    {
        [HttpPost("AddNew_Order", Name = "AddNewOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrdersDTO?>> AddNewOrder(OrdersDTO Order)
        {
            // Not There Validation for Properties Currently.

            var newOrder = new clsOrders(Order);

            if (await newOrder.SaveAsync())
            {
                Order.OrderID = newOrder.OrderID;

                return CreatedAtRoute("GetOrderByID", new { id = newOrder.OrderID }, Order);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding Order.");

        }

        [HttpGet("Get_Order_ByID/{id}", Name = "GetOrderByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrdersDTO?>> GetOrderByID(int? id)
        {
            if (id == null || id < 1 || id > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {id}");
            }

            var Order = await clsOrders.GetOrderByIDAsync(id);

            if (Order == null)
            {
                return NotFound($"Not Found Order With ID {id}");
            }

            return Ok(Order.ODTO);
        }

        [HttpPut("Update_Order/{id}", Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrdersDTO?>> UpdateOrderAsync(int? id, OrdersDTO Order)
        {
            if (id == null || id < 1 || id > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {id}");
            }

            // Not There Validation for Properties Currently.

            var uOrder = await clsOrders.GetOrderByIDAsync(id);

            if (uOrder == null)
            {
                return NotFound($"Not Found Order With ID {id}.");
            }

            Order.OrderID = id;
            uOrder = new clsOrders(Order, clsOrders.enMode.Update);

            if (await uOrder.SaveAsync())
            {
                return Ok(uOrder.ODTO);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Order.");
        }

        [HttpGet("Is_Order_Exists/{id}", Name = "IsOrderExists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> IsOrderExistsAsync(int? id)
        {
            if (id < 1 || id == null || id > int.MaxValue)
            {
                return BadRequest($"No Accept ID {id} .");
            }

            if (await clsOrders.IsOrderExistsAsync(id))
            {
                return Ok($"Order With ID {id} Is Exists.");
            }
            else
            {
                return NotFound($"Not Found Order With ID {id} .");
            }
        }

        [HttpDelete("Delete_Order/{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteOrderAsync(int? id)
        {
            if (id < 1 || id == null)
            {
                return BadRequest($"No Accept ID {id} .");
            }

            if (await clsOrders.IsOrderExistsAsync(id))
            {
                if (await clsOrders.DeleteOrderAsync(id))
                {
                    return Ok($"Deleted Order With ID {id} Successfully.");
                }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete Order.");
            }
            else
            {
                return NotFound($"Not Found Order With ID {id} .");
            }
        }

        [HttpGet("Get_All_Orders",Name = "GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OrdersDTO?>>> GetAllOrdersAsync()
        {
            var OrdersList = await clsOrders.GetAllOrdersAsync();
            if (OrdersList == null || OrdersList.Count < 1)
            {
                return NotFound($"Not Found Orders.");
            }

            return Ok(OrdersList);
        }
    }
}
