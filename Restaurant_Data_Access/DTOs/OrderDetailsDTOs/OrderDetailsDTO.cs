using System.ComponentModel.DataAnnotations;

namespace Restaurant_Data_Access.DTOs.OrderDetailsDTOs
{
    public class OrderDetailsDTO
    {
        public OrderDetailsDTO(int orderid, int menuitemid, int quantity, decimal price)
        {
            this.OrderID = orderid;
            this.MenuItemID = menuitemid;
            this.Quantity = quantity;
            this.Price = price;
        }

        [Required(ErrorMessage = "OrderID is required.")]
        public int OrderID { get; set; }
        [Required(ErrorMessage = "MenuItemID is required.")]
        public int MenuItemID { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }
        public decimal LineTotal
        {
            get
            {
                return Quantity * Price;
            }
        }
    }
}
