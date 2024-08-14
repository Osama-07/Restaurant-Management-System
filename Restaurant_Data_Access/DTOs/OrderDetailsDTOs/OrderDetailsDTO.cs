using System.ComponentModel.DataAnnotations;


namespace Restaurant_Data_Access.DTOs.OrderDetailsDTOs
{
    public class OrderDetailsDTO
    {
        public OrderDetailsDTO(int? orderdetailsid, int orderid, int menuitemid, int quantity, decimal price)
        {
            this.OrderDetailsID = orderdetailsid;
            this.OrderID = orderid;
            this.MenuItemID = menuitemid;
            this.Quantity = quantity;
            this.Price = price;
        }

        [Range(0, int.MaxValue, ErrorMessage = "OrderDetailsID must be between 0 and the maximum value of an integer.")]
        public int? OrderDetailsID { get; set; }
        [Required(ErrorMessage = "OrderID is required.")]
        public int OrderID { get; set; } // allow null.
        [Required(ErrorMessage = "MenuItemID is required.")]
        public int MenuItemID { get; set; } // allow null.
        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }
    }
}
