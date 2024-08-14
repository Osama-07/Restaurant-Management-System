using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Business;
using Restaurant_Data_Access.DTOs.EmployeeDTOs;

namespace RestaurantAPI.Controllers
{
    [Route("api/Employees")]
    [ApiController]
    public class EmployeesAPIController : ControllerBase
    {
        [HttpPost(Name = "AddNewEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeesDTO?>> AddNewEmployee(EmployeesDTO employee)
        {
            if (string.IsNullOrEmpty(employee.Name) || 
                string.IsNullOrEmpty(employee.Position))
            {
                return BadRequest("No Accept Employee Info");
            }

            var newEmployee = new clsEmployees(employee);

            if (await newEmployee.SaveAsync())
            {
                employee.EmployeeID = newEmployee.EmployeeID;

                return CreatedAtRoute("GetEmployeeByID", new { id = newEmployee.EmployeeID }, employee);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding Employee.");

        }

        [HttpGet("{id}", Name = "GetEmployeeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeesDTO?>> GetEmployeeByID(int? id)
        {
            if (id == null || id < 1 || id > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {id}");
            }

            var Employee = await clsEmployees.GetEmployeeByIDAsync(id);

            if (Employee == null)
            {
                return NotFound($"Not Found Employee With ID {id}");
            }

            return Ok(Employee.EDTO);
        }

        [HttpPut("ID/{id}", Name = "UpdateEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeesDTO?>> UpdateEmployeeAsync(int? id, EmployeesDTO employee)
        {
            if (id == null || id < 1 || id > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {id}");
            }

            if (string.IsNullOrEmpty(employee.Name) ||
                string.IsNullOrEmpty(employee.Position))
            {
                return BadRequest("No Accept Employee Info");
            }

            var uEmployee = await clsEmployees.GetEmployeeByIDAsync(id);

            if (uEmployee == null)
            {
                return NotFound($"Not Found Employee With ID {id}.");
            }

            employee.EmployeeID = id;
            uEmployee = new clsEmployees(employee, clsEmployees.enMode.Update);

            if (await uEmployee.SaveAsync())
            {
                return Ok(uEmployee.EDTO);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Employee.");
        }

        [HttpGet("ID/{id}", Name = "IsEmployeeExists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> IsEmployeeExistsByIDAsync(int? id)
        {
            if (id < 1 || id == null || id > int.MaxValue)
            {
                return BadRequest($"No Accept ID {id} .");
            }

            if (await clsEmployees.IsEmployeeExistsAsync(id))
            {
                return Ok($"Employee With ID {id} Is Exists.");
            }
            else
            {
                return NotFound($"Not Found Employee With ID {id} .");
            }
        }

        [HttpDelete("ID/{id}", Name = "DeleteEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteEmployeeAsync(int? id)
        {
            if (id < 1 || id == null)
            {
                return BadRequest($"No Accept ID {id} .");
            }

            if (await clsEmployees.IsEmployeeExistsAsync(id))
            {
                if (await clsEmployees.DeleteEmployeeAsync(id))
                {
                    return Ok($"Deleted Employee With ID {id} Successfully.");
                }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete Employee.");
            }
            else
            {
                return NotFound($"Not Found Employee With ID {id} .");
            }
        }

        [HttpGet(Name = "GetAllEmployees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EmployeesDTO?>>> GetAllEmployeesAsync()
        {
            var EmployeesList = await clsEmployees.GetAllEmployeesAsync();
            if (EmployeesList == null || EmployeesList.Count < 1)
            {
                return NotFound($"Not Found Employees.");
            }

            return Ok(EmployeesList);
        }
    }
}
