using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Restaurant_Data_Access;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAPIController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> IsConnectionTrue()
        {
            using (SqlConnection conn = new SqlConnection(clsDataSettings._connectionString))
            {
                await conn.OpenAsync();

            }

            return Ok("Connection Successfully.");
        }
    }
}
