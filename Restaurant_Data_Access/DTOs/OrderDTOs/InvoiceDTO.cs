namespace Restaurant_Data_Access.DTOs.OrderDTOs
{
    public class InvoiceDTO
    {
        public InvoiceDTO(int orderId, DateTime orderDate, string createdByUser, decimal totalAmount, decimal? appliedTaxRate = 0) 
        {
            this.OrderID = orderId;
            this.OrderDate = orderDate;
            this.CreatedByUser = createdByUser;
            this.TotalAmount = totalAmount;
            this.AppliedTaxRate = appliedTaxRate;
            this.OrderDetails = new List<OrderDetail>();
        }

        public int OrderID { get; set; }                  // رقم الطلب
        public DateTime OrderDate { get; set; }           // تاريخ الطلب
        public string CreatedByUser { get; set; } = null!; // اسم العميل
        public decimal TotalAmount { get; set; }          // إجمالي الفاتورة
        public decimal? AppliedTaxRate { get; set; }          // الضريبه
        public IEnumerable<OrderDetail> OrderDetails { get; set; } // قائمة تفاصيل الطلب

        public class OrderDetail
        {
            public OrderDetail(string itemName, string categoryName, int quantity, decimal price)
            {
                ItemName = itemName;
                CategoryName = categoryName;
                Quantity = quantity;
                Price = price;
            }

            public string ItemName { get; set; } = null!;       // اسم الصنف
            public string CategoryName { get; set; } = null!;   // اسم الفئة
            public int Quantity { get; set; }                   // الكمية
            public decimal Price { get; set; }                  // السعر لكل صنف
            public decimal LineTotal => Quantity * Price;       // إجمالي السعر (سعر الوحدة * الكمية)
        }

    }
}
