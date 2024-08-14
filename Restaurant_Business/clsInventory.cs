

using Restaurant_Data_Access.DTOs.InventoryDTOs;
using Restaurant_Data_Access;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_Business
{
    public class clsInventory
    {
        public enum enMode { AddNew = 1, Update = 2 }
        public enMode Mode;

        public InventoryDTO IDTO
        {
            get
            {
                return new InventoryDTO
                 (
                    this.InventoryID,
                    this.ItemName,
                    this.Quantity,
                    this.Unit,
                    this.ReorderLevel
                 );
            }
        }
        public clsInventory(InventoryDTO IDTO, enMode mode = enMode.AddNew)
        {
            this.InventoryID = IDTO.InventoryID;
            this.ItemName = IDTO.ItemName;
            this.Quantity = IDTO.Quantity;
            this.Unit = IDTO.Unit;
            this.ReorderLevel = IDTO.ReorderLevel;

            this.Mode = mode;
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
        private bool _AddNewInventory()
        {
            this.InventoryID = clsInventoryData.AddNewInventory(IDTO);

            return (this.InventoryID > 0);
        }

        private async Task<bool> _AddNewInventoryAsync()
        {
            this.InventoryID = await clsInventoryData.AddNewInventoryAsync(IDTO);

            return (this.InventoryID > 0);
        }

        private bool _UpdateInventory()
        {
            return clsInventoryData.UpdateInventory(IDTO);
        }

        private async Task<bool> _UpdateInventoryAsync()
        {
            return await clsInventoryData.UpdateInventoryAsync(IDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewInventory())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateInventory();
            }
            return false;
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewInventoryAsync())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return await _UpdateInventoryAsync();
            }
            return false;
        }

        public static clsInventory? GetInventoryByID(int? id)
        {
            if (id < 1 || id == null) return null;

            InventoryDTO? iDTO = clsInventoryData.GetInventoryByID(id);

            if (iDTO != null)
            {
                return new clsInventory(iDTO, enMode.Update);
            }
            else
                return null;
        }

        public static async Task<clsInventory?> GetInventoryByIDAsync(int? id)
        {
            if (id < 1 || id == null) return null;

            InventoryDTO? iDTO = await clsInventoryData.GetInventoryByIDAsync(id);

            if (iDTO != null)
            {
                return new clsInventory(iDTO, enMode.Update);
            }
            else
                return null;
        }

        public static bool IsInventoryExists(int? id)
        {
            if (id < 1 || id == null) return false;

            return clsInventoryData.IsInventoryExists(id);
        }

        public static async Task<bool> IsInventoryExistsAsync(int? id)
        {
            if (id < 1 || id == null) return false;

            return await clsInventoryData.IsInventoryExistsAsync(id);
        }

        public static bool DeleteInventory(int? id)
        {
            if (id < 1 || id == null) return false;

            return clsInventoryData.DeleteInventory(id);
        }

        public static async Task<bool> DeleteInventoryAsync(int? id)
        {
            if (id < 1 || id == null) return false;

            return await clsInventoryData.DeleteInventoryAsync(id);
        }

        public static List<InventoryDTO?> GetAllInventory()
        {
            return clsInventoryData.GetAllInventory();
        }

        public static async Task<List<InventoryDTO?>> GetAllInventoryAsync()
        {
            return await clsInventoryData.GetAllInventoryAsync();
        }


    }
}
