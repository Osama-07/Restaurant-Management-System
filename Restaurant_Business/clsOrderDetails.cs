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
                    this.OrderDetailsID,
                    this.OrderID,
                    this.MenuItemID,
                    this.Quantity,
                    this.Price
                 );
            }
        }
        public clsOrderDetails(OrderDetailsDTO ODTO, enMode mode = enMode.AddNew)
        {
            this.OrderDetailsID = ODTO.OrderDetailsID;
            this.OrderID = ODTO.OrderID;
            this.MenuItemID = ODTO.MenuItemID;
            this.Quantity = ODTO.Quantity;
            this.Price = ODTO.Price;

            this.Mode = mode;
        }

        [Range(0, int.MaxValue, ErrorMessage = "OrderDetailsID must be between 0 and the maximum value of an integer.")]
        public int? OrderDetailsID { get; set; }
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
            this.OrderDetailsID = clsOrderDetailsData.AddNewOrderDetails(ODTO);

            return (this.OrderDetailsID > 0);
        }

        private async Task<bool> _AddNewOrderDetailsAsync()
        {
            this.OrderDetailsID = await clsOrderDetailsData.AddNewOrderDetailsAsync(ODTO);

            return (this.OrderDetailsID > 0);
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

        public static clsOrderDetails? GetOrderDetailsByID(int? id)
        {
            if (id < 1 || id == null) return null;

            OrderDetailsDTO? oDTO = clsOrderDetailsData.GetOrderDetailsByID(id);

            if (oDTO != null)
            {
                return new clsOrderDetails(oDTO, enMode.Update);
            }
            else
                return null;
        }

        public static async Task<clsOrderDetails?> GetOrderDetailsByIDAsync(int? id)
        {
            if (id < 1 || id == null) return null;

            OrderDetailsDTO? oDTO = await clsOrderDetailsData.GetOrderDetailsByIDAsync(id);

            if (oDTO != null)
            {
                return new clsOrderDetails(oDTO, enMode.Update);
            }
            else
                return null;
        }

        public static bool IsOrderDetailsExists(int? id)
        {
            if (id < 1 || id == null) return false;

            return clsOrderDetailsData.IsOrderDetailsExists(id);
        }

        public static async Task<bool> IsOrderDetailsExistsAsync(int? id)
        {
            if (id < 1 || id == null) return false;

            return await clsOrderDetailsData.IsOrderDetailsExistsAsync(id);
        }

        public static bool DeleteOrderDetailsByID(int? id)
        {
            if (id < 1 || id == null) return false;

            return clsOrderDetailsData.DeleteOrderDetails(id);
        }

        public static async Task<bool> DeleteOrderDetailsAsync(int? id)
        {
            if (id < 1 || id == null) return false;

            return await clsOrderDetailsData.DeleteOrderDetailsAsync(id);
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
