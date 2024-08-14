using Restaurant_Data_Access;
using Restaurant_Data_Access.DTOs.OrderDTOs;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_Business
{
    public class clsOrders
    {
        public enum enMode { AddNew = 1, Update = 2 }
        public enMode Mode;

        public OrdersDTO ODTO
        {
            get
            {
                return new OrdersDTO
                 (
                    this.OrderID,
                    this.UserID,
                    this.OrderDate,
                    this.TotalAmount
                 );
            }
        }
        public clsOrders(OrdersDTO ODTO, enMode mode = enMode.AddNew)
        {
            this.OrderID = ODTO.OrderID;
            this.UserID = ODTO.UserID;
            this.OrderDate = ODTO.OrderDate;
            this.TotalAmount = ODTO.TotalAmount;

            this.Mode = mode;
        }

        [Range(0, int.MaxValue, ErrorMessage = "OrderID must be between 0 and the maximum value of an integer.")]
        public int? OrderID { get; set; }
        public int? UserID { get; set; } // allow null. [Required(ErrorMessage = "OrderDate is required.")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "TotalAmount is required.")]
        public decimal TotalAmount { get; set; }

        private bool _AddNewOrder()
        {
            this.OrderID = clsOrdersData.AddNewOrder(ODTO);

            return (this.OrderID > 0);
        }

        private async Task<bool> _AddNewOrderAsync()
        {
            this.OrderID = await clsOrdersData.AddNewOrderAsync(ODTO);

            return (this.OrderID > 0);
        }

        private bool _UpdateOrder()
        {
            return clsOrdersData.UpdateOrder(ODTO);
        }

        private async Task<bool> _UpdateOrderAsync()
        {
            return await clsOrdersData.UpdateOrderAsync(ODTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewOrder())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateOrder();
            }
            return false;
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewOrderAsync())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return await _UpdateOrderAsync();
            }
            return false;
        }

        public static clsOrders? GetOrderByID(int? id)
        {
            if (id < 1 || id == null) return null;

            OrdersDTO? oDTO = clsOrdersData.GetOrderByID(id);

            if (oDTO != null)
            {
                return new clsOrders(oDTO, enMode.Update);
            }
            else
                return null;
        }

        public static async Task<clsOrders?> GetOrderByIDAsync(int? id)
        {
            if (id < 1 || id == null) return null;

            OrdersDTO? oDTO = await clsOrdersData.GetOrderByIDAsync(id);

            if (oDTO != null)
            {
                return new clsOrders(oDTO, enMode.Update);
            }
            else
                return null;
        }

        public static bool IsOrderExists(int? id)
        {
            if (id < 1 || id == null) return false;

            return clsOrdersData.IsOrderExists(id);
        }

        public static async Task<bool> IsOrderExistsAsync(int? id)
        {
            if (id < 1 || id == null) return false;

            return await clsOrdersData.IsOrderExistsAsync(id);
        }

        public static bool DeleteOrder(int? id)
        {
            if (id < 1 || id == null) return false;

            return clsOrdersData.DeleteOrder(id);
        }

        public static async Task<bool> DeleteOrderAsync(int? id)
        {
            if (id < 1 || id == null) return false;

            return await clsOrdersData.DeleteOrderAsync(id);
        }

        public static List<OrdersDTO?> GetAllOrders()
        {
            return clsOrdersData.GetAllOrders();
        }
        
        public static async Task<List<OrdersDTO?>> GetAllOrdersAsync()
        {
            return await clsOrdersData.GetAllOrdersAsync();
        }

    }
}
