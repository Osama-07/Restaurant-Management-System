
using Restaurant_Data_Access.DTOs.MenuItemsDTOs;
using Restaurant_Data_Access;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_Business
{
    public class clsMenuItems
    {
        public enum enMode { AddNew = 1, Update = 2 }
        public enMode Mode;

        public MenuItemsDTO MDTO
        {
            get
            {
                return new MenuItemsDTO
                 (
                    this.MenuItemID,
                    this.Name,
                    this.Price,
                    this.Description,
                    this.CategoryID
                 );
            }
        }

        public clsMenuItems(MenuItemsDTO MDTO, enMode mode = enMode.AddNew)
        {
            this.MenuItemID = MDTO.MenuItemID;
            this.Name = MDTO.Name;
            this.Price = MDTO.Price;
            this.Description = MDTO.Description;
            this.CategoryID = MDTO.CategoryID;

            this.Mode = mode;
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

        private bool _AddNewMenuItem()
        {
            this.MenuItemID = clsMenuItemsData.AddNewMenuItem(MDTO);

            return (this.MenuItemID > 0);
        }

        private async Task<bool> _AddNewMenuItemAsync()
        {
            this.MenuItemID = await clsMenuItemsData.AddNewMenuItemAsync(MDTO);

            return (this.MenuItemID > 0);
        }

        private bool _UpdateMenuItem()
        {
            return clsMenuItemsData.UpdateMenuItem(MDTO);
        }

        private async Task<bool> _UpdateMenuItemAsync()
        {
            return await clsMenuItemsData.UpdateMenuItemAsync(MDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewMenuItem())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateMenuItem();
            }
            return false;
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewMenuItemAsync())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return await _UpdateMenuItemAsync();
            }
            return false;
        }

        public static clsMenuItems? GetMenuItemByID(int? id)
        {
            if (id < 1 || id == null) return null;

            MenuItemsDTO? mDTO = clsMenuItemsData.GetMenuItemByID(id);

            if (mDTO != null)
            {
                return new clsMenuItems(mDTO, enMode.Update);
            }
            else
                return null;
        }

        public static async Task<clsMenuItems?> GetMenuItemByIDAsync(int? id)
        {
            if (id < 1 || id == null) return null;

            MenuItemsDTO? mDTO = await clsMenuItemsData.GetMenuItemByIDAsync(id);

            if (mDTO != null)
            {
                return new clsMenuItems(mDTO, enMode.Update);
            }
            else
                return null;
        }

        public static bool IsMenuItemExists(int? id)
        {
            if (id < 1 || id == null) return false;

            return clsMenuItemsData.IsMenuItemExists(id);
        }

        public static async Task<bool> IsMenuItemExistsAsync(int? id)
        {
            if (id < 1 || id == null) return false;

            return await clsMenuItemsData.IsMenuItemExistsAsync(id);
        }

        public static bool DeleteMenuItem(int? id)
        {
            if (id < 1 || id == null) return false;

            return clsMenuItemsData.DeleteMenuItem(id);
        }

        public static async Task<bool> DeleteMenuItemAsync(int? id)
        {
            if (id < 1 || id == null) return false;

            return await clsMenuItemsData.DeleteMenuItemAsync(id);
        }

        public static List<MenuItemsDTO?> GetAllMenuItems()
        {
            return clsMenuItemsData.GetAllMenuItems();
        }
        
        public static async Task<List<MenuItemsDTO?>> GetAllMenuItemsAsync()
        {
            return await clsMenuItemsData.GetAllMenuItemsAsync();
        }

    }
}
