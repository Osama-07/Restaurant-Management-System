using System.ComponentModel.DataAnnotations;

namespace Restaurant_Data_Access.DTOs.InventoryDTOs
{
    public class InventoryDTO
    {
        public InventoryDTO(int? inventoryid, string itemname, decimal quantity, string unit, decimal? reorderlevel)
        {
            this.InventoryID = inventoryid;
            this.ItemName = itemname;
            this.Quantity = quantity;
            this.Unit = unit;
            this.ReorderLevel = reorderlevel;
        }

        [Range(0, int.MaxValue, ErrorMessage = "InventoryID must be between 0 and the maximum value of an integer.")]
        public int? InventoryID { get; set; }
        [Required(ErrorMessage = "ItemName is required.")]
        [MaxLength(100, ErrorMessage = "ItemName cannot exceed 100 characters.")]
        public string ItemName { get; set; } // Length: 100
        [Required(ErrorMessage = "Quantity is required.")]
        public decimal Quantity { get; set; }
        [Required(ErrorMessage = "Unit is required.")]
        [MaxLength(20, ErrorMessage = "Unit cannot exceed 20 characters.")]
        public string Unit { get; set; } // Length: 20
        public decimal? ReorderLevel { get; set; } // allow null.
    }
}
