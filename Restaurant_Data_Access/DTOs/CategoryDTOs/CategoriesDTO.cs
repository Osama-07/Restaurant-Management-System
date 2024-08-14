using System.ComponentModel.DataAnnotations;

namespace Restaurant_Data_Access.DTOs.CategoryDTOs
{
    public class CategoriesDTO
    {
        public CategoriesDTO(int? categoryid, string categoryname)
        {
            this.CategoryID = categoryid;
            this.CategoryName = categoryname;
        }

        [Range(0, int.MaxValue, ErrorMessage = "CategoryID must be between 0 and the maximum value of an integer.")]
        public int? CategoryID { get; set; }

        [Required(ErrorMessage = "CategoryName is required.")]
        [MaxLength(50, ErrorMessage = "CategoryName cannot exceed 50 characters.")]
        public string CategoryName { get; set; } // Length: 50
    }
}
