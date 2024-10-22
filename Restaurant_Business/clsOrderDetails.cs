using Restaurant_Data_Access;
using Restaurant_Data_Access.DTOs.OrderDetailsDTOs;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_Business
{
    public class clsOrderDetails
    {
        public enum enMode { AddNew = 1, Update = 2 }
        public enMode Mode;

        public OrderDetailsDTO ODTO
        {
            get
            {
                return new OrderDetailsDTO
                 (
                    this.OrderID,
                    this.MenuItemID,
                    this.Quantity,
                    this.Price
                 );
            }
        }
        public clsOrderDetails(OrderDetailsDTO ODTO, enMode mode = enMode.AddNew)
        {
            this.OrderID = ODTO.OrderID;
            this.MenuItemID = ODTO.MenuItemID;
            this.Quantity = ODTO.Quantity;
            this.Price = ODTO.Price;

            this.Mode = mode;
        }

        [Required(ErrorMessage = "OrderID is required.")]
        public int OrderID { get; set; }
        [Required(ErrorMessage = "MenuItemID is required.")]
        public int MenuItemID { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }

        private bool _AddNewOrderDetails()
        {
            return clsOrderDetailsData.AddNewOrderDetails(ODTO);
        }

        private async Task<bool> _AddNewOrderDetailsAsync()
        {
            return await clsOrderDetailsData.AddNewOrderDetailsAsync(ODTO);
        }

        public static async Task<bool> AddNewListOrderDetailsAsync(List<OrderDetailsDTO> orderDetails)
        {
            return await clsOrderDetailsData.AddNewListOrderDetailsAsync(orderDetails);
        }

        private bool _UpdateOrderDetails()
        {
            return clsOrderDetailsData.UpdateOrderDetails(ODTO);
        }

        private async Task<bool> _UpdateOrderDetailsAsync()
        {
            return await clsOrderDetailsData.UpdateOrderDetailsAsync(ODTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewOrderDetails())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateOrderDetails();
            }
            return false;
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewOrderDetailsAsync())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return await _UpdateOrderDetailsAsync();
            }
            return false;
        }

        public static clsOrderDetails? GetOrderDetailsByOrderID(int? orderId, int? menuItemId)
        {
            if (orderId < 1 || orderId == null || menuItemId < 1 || menuItemId == null) return null;

            OrderDetailsDTO? oDTO = clsOrderDetailsData.GetOrderDetailsByOrderID(orderId, menuItemId);

            if (oDTO != null)
            {
                return new clsOrderDetails(oDTO, enMode.Update);
            }
            else
                return null;
        }

        public static async Task<clsOrderDetails?> GetOrderDetailsByOrderIDAsync(int? orderId, int? menuItemId)
        {
            if (orderId < 1 || orderId == null || menuItemId < 1 || menuItemId == null) return null;

            OrderDetailsDTO? oDTO = await clsOrderDetailsData.GetOrderDetailsByOrderIDAsync(orderId, menuItemId);

            if (oDTO != null)
            {
                return new clsOrderDetails(oDTO, enMode.Update);
            }
            else
                return null;
        }

        public static List<OrderDetailsDTO?> GetListOrderDetailsByOrderID(int? orderId)
        {
            if (orderId < 1 || orderId == null) return null!;

            return clsOrderDetailsData.GetListOrderDetailsByOrderID(orderId);
        }

        public static async Task<List<OrderDetailsDTO?>> GetListOrderDetailsByOrderIDAsync(int? orderId)
        {
            if (orderId < 1 || orderId == null) return null!;

            return await clsOrderDetailsData.GetListOrderDetailsByOrderIDAsync(orderId);
        }

        public static bool IsOrderDetailsExists(int? orderId, int? menuItemId)
        {
            if (orderId < 1 || orderId == null || menuItemId < 1 || menuItemId == null) return false;

            return clsOrderDetailsData.IsOrderDetailsExists(orderId, menuItemId);
        }

        public static async Task<bool> IsOrderDetailsExistsAsync(int? orderId, int? menuItemId)
        {
            if (orderId < 1 || orderId == null || menuItemId < 1 || menuItemId == null) return false;

            return await clsOrderDetailsData.IsOrderDetailsExistsAsync(orderId, menuItemId);
        }

        public static bool DeleteOrderItem(int? orderId, int? menuItemId)
        {
            if (orderId < 1 || orderId == null || menuItemId < 1 || menuItemId == null) return false;

            return clsOrderDetailsData.DeleteOrderItem(orderId, menuItemId);
        }

        public static async Task<bool> DeleteOrderItemAsync(int? orderId, int? menuItemId)
        {
            if (orderId < 1 || orderId == null || menuItemId < 1 || menuItemId == null) return false;

            return await clsOrderDetailsData.DeleteOrderItemAsync(orderId, menuItemId);
        }

        public static List<OrderDetailsDTO?> GetAllOrderDetails()
        {
            return clsOrderDetailsData.GetAllOrderDetails();
        }

        public static async Task<List<OrderDetailsDTO?>> GetAllOrderDetailsAsync()
        {
            return await clsOrderDetailsData.GetAllOrderDetailsAsync();
        }

    }
}
