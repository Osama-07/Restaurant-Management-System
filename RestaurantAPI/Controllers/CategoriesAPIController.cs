using Microsoft.AspNetCore.Mvc;
using Restaurant_Business;
using Restaurant_Data_Access.DTOs.CategoryDTOs;

namespace RestaurantAPI.Controllers
{
    [Route("api/Categories")]
    [ApiController]
    public class CategoriesAPIController : ControllerBase
    {
        [HttpPost(Name = "AddNewCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CategoriesDTO?>> AddNewCategory(CategoriesDTO category)
        {
            if (string.IsNullOrEmpty(category.CategoryName))
            {
                return BadRequest("No Accept Category Name");
            }

            var newCategory = new clsCategories(category);

            if (await newCategory.SaveAsync())
            {
                category.CategoryID = newCategory.CategoryID;

                return CreatedAtRoute("GetCategoryByID", new { id = newCategory.CategoryID}, category);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding Category.");

        }

        [HttpGet("{id}", Name = "GetCategoryByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoriesDTO?>> GetCategoryByID(int? id)
        {
            if (id == null || id < 1 || id > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {id}");
            }

            var category = await clsCategories.GetCategoryByIDAsync(id);

            if (category == null)
            {
                return NotFound($"Not Found Category With ID {id}");
            }

            return Ok(category.CDTO);
        }

        [HttpPut("ID/{id}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoriesDTO?>> UpdateCategoryAsync(int? id, CategoriesDTO category)
        {
            if (id == null || id < 1 || id > int.MaxValue)
            {
                return BadRequest($"Not Accept this ID {id}");
            }

            if (string.IsNullOrEmpty(category.CategoryName))
            {
                return BadRequest("No Accept Category Name");
            }

            var uCategory = await clsCategories.GetCategoryByIDAsync(id);

            if (uCategory == null)
            {
                return NotFound($"Not Found Category With ID {id}.");
            }

            category.CategoryID = id;
            uCategory = new clsCategories(category, clsCategories.enMode.Update);

            if (await uCategory.SaveAsync())
            {
                return Ok(uCategory.CDTO);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Category.");
        }

        [HttpGet("ID/{id}", Name = "IsCategoryExists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> IsCategoryExistsAsync(int? id)
        {
            if (id < 1 || id == null || id > int.MaxValue)
            {
                return BadRequest($"No Accept ID {id} .");
            }

            if (await clsCategories.IsCategoryExistsAsync(id))
            {
                return Ok($"Category With ID {id} Is Exists.");
            }
            else
            {
                return NotFound($"Not Found Category With ID {id} .");
            }
        }

        [HttpDelete("ID/{id}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteCategoryAsync(int? id)
        {
            if (id < 1 || id == null)
            {
                return BadRequest($"No Accept ID {id} .");
            }

            if (await clsCategories.IsCategoryExistsAsync(id))
            {
                if (await clsCategories.DeleteCategoryAsync(id))
                {
                    return Ok($"Deleted Category With ID {id} Successfully.");
                }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete Category.");
            }
            else
            {
                return NotFound($"Not Found Category With ID {id} .");
            }
        }

        [HttpGet(Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CategoriesDTO?>>> GetAllCategoriesAsync()
        {
            var CategoriesList = await clsCategories.GetAllCategoriesAsync();
            if (CategoriesList == null || CategoriesList.Count < 1)
            {
                return NotFound($"Not Found Categories.");
            }

            return Ok(CategoriesList);
        }
    }
}
