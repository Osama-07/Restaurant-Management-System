using Restaurant_Business;
using Microsoft.AspNetCore.Mvc;
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
            if (Order.TotalAmount < 0 ||
                Order.OrderDate > DateTime.Now.AddMinutes(5)) // Add 5 minutes, the request meybe late.
            {
                return BadRequest("Not Accept Order Info.");
            }

            var newOrder = new clsOrders(Order);

            if (await newOrder.SaveAsync())
            {
                Order.OrderID = newOrder.OrderID;

                return CreatedAtRoute("GetOrderByID", new { id = newOrder.OrderID }, Order);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding Order.");

        }

        [HttpGet("GetOrderByID/{id}", Name = "GetOrderByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrdersDTO?>> GetOrderByID(int id)
        {
            if (id < 1 || id > int.MaxValue)
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

        [HttpGet("GetInvoice/{orderId}", Name = "GetInvoice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<InvoiceDTO?>> GetInvoice(int orderId)
        {
            if (orderId < 1 || orderId > int.MaxValue)
            {
                return BadRequest($"Not Accept Invoice With this Order ID {orderId}");
            }

            var invoice = await clsOrders.GetInvoiceAsync(orderId);

            if (invoice == null)
            {
                return NotFound($"Not Found Invoice With this Order ID {orderId}");
            }

            return Ok(invoice);
        }
        [HttpPut("UpdateOrder/{id}", Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrdersDTO?>> UpdateOrderAsync(int id, OrdersDTO Order)
        {
            if (id < 1 || id > int.MaxValue)
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

        [HttpGet("IsOrderExists/{id}", Name = "IsOrderExists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> IsOrderExistsAsync(int id)
        {
            if (id < 1 || id > int.MaxValue)
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

        [HttpDelete("DeleteOrder/{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteOrderAsync(int id)
        {
            if (id < 1)
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

        [HttpGet("GetAllOrders", Name = "GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OrdersDTO?>>> GetAllOrdersAsync()
        {
            var OrdersList = await clsOrders.GetAllOrdersAsync();
            if (!OrdersList.Any())
            {
                return NotFound($"Not Found Orders.");
            }

            return Ok(OrdersList);
        }
    }
}
