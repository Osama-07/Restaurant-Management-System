using System.ComponentModel.DataAnnotations;

namespace Restaurant_Data_Access.DTOs.OrderDTOs
{
    public class OrdersDTO
    {
        public OrdersDTO(int? orderid, int? userid, DateTime orderdate, decimal totalamount, decimal? appliedTaxRate = 0)
        {
            this.OrderID = orderid;
            this.UserID = userid;
            this.OrderDate = orderdate;
            this.TotalAmount = totalamount;
            this.AppliedTaxRate = appliedTaxRate;
        }

        [Range(0, int.MaxValue, ErrorMessage = "OrderID must be between 0 and the maximum value of an integer.")]
        public int? OrderID { get; set; }
        public int? UserID { get; set; } // allow null.
        [Required(ErrorMessage = "OrderDate is required.")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "TotalAmount is required.")]
        public decimal TotalAmount { get; set; }
        public decimal? AppliedTaxRate { get; set; }
    }
}
