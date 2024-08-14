using System.ComponentModel.DataAnnotations;


namespace Restaurant_Data_Access.DTOs.MenuItemsDTOs
{
    public class MenuItemsDTO
    {
        public MenuItemsDTO(int? menuitemid, string name, decimal price, string? description, int categoryid)
        {
            this.MenuItemID = menuitemid;
            this.Name = name;
            this.Price = price;
            this.Description = description;
            this.CategoryID = categoryid;
        }

        [Range(0, int.MaxValue, ErrorMessage = "MenuItemID must be between 0 and the maximum value of an integer.")]
        public int? MenuItemID { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } // Length: 100
        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }
        [MaxLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string? Description { get; set; } // allow null. // Length: 255
        [Required(ErrorMessage = "CategoryID is required.")]
        public int CategoryID { get; set; }
    }
}