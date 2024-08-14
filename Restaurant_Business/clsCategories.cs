

using Restaurant_Data_Access.DTOs.CategoryDTOs;
using Restaurant_Data_Access;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_Business
{
    public class clsCategories
    {
        public enum enMode { AddNew = 1, Update = 2 }
        public enMode Mode;

        public CategoriesDTO CDTO
        {
            get
            {
                return new CategoriesDTO
                 (
                    this.CategoryID,
                    this.CategoryName
                 );
            }
        }
        public clsCategories(CategoriesDTO CDTO, enMode mode = enMode.AddNew)
        {
            this.CategoryID = CDTO.CategoryID;
            this.CategoryName = CDTO.CategoryName;

            this.Mode = mode;
        }

        [Range(0, int.MaxValue, ErrorMessage = "CategoryID must be between 0 and the maximum value of an integer.")]
        public int? CategoryID { get; set; }
        [Required(ErrorMessage = "CategoryName is required.")]
        [MaxLength(50, ErrorMessage = "CategoryName cannot exceed 50 characters.")]
        public string CategoryName { get; set; } // Length: 50

        private bool _AddNewCategory()
        {
            this.CategoryID = clsCategoriesData.AddNewCategory(CDTO);

            return (this.CategoryID > 0);
        }

        private async Task<bool> _AddNewCategoryAsync()
        {
            this.CategoryID = await clsCategoriesData.AddNewCategoryAsync(CDTO);

            return (this.CategoryID > 0);
        }

        private bool _UpdateCategory()
        {
            return clsCategoriesData.UpdateCategory(CDTO);
        }

        private async Task<bool> _UpdateCategoryAsync()
        {
            return await clsCategoriesData.UpdateCategoryAsync(CDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCategory())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateCategory();
            }
            return false;
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewCategoryAsync())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return await _UpdateCategoryAsync();
            }
            return false;
        }

        public static clsCategories? GetCategoryByID(int? id)
        {
            if (id < 1 || id == null) return null;

            CategoriesDTO? cDTO = clsCategoriesData.GetCategoryByID(id);

            if (cDTO != null)
            {
                return new clsCategories(cDTO, enMode.Update);
            }
            else
                return null;
        }

        public static async Task<clsCategories?> GetCategoryByIDAsync(int? id)
        {
            if (id < 1 || id == null) return null;

            CategoriesDTO? cDTO = await clsCategoriesData.GetCategoryByIDAsync(id);

            if (cDTO != null)
            {
                return new clsCategories(cDTO, enMode.Update);
            }
            else
                return null;
        }

        public static bool IsCategoryExists(int? id)
        {
            if (id < 1 || id == null) return false;

            return clsCategoriesData.IsCategoryExists(id);
        }

        public static async Task<bool> IsCategoryExistsAsync(int? id)
        {
            if (id < 1 || id == null) return false;

            return await clsCategoriesData.IsCategoryExistsAsync(id);
        }

        public static bool DeleteCategoryByID(int? id)
        {
            if (id < 1 || id == null) return false;

            return clsCategoriesData.DeleteCategory(id);
        }

        public static async Task<bool> DeleteCategoryAsync(int? id)
        {
            if (id < 1 || id == null) return false;

            return await clsCategoriesData.DeleteCategoryAsync(id);
        }

        public static List<CategoriesDTO?> GetAllCategories()
        {
            return clsCategoriesData.GetAllCategories();
        }
        
        public static async Task<List<CategoriesDTO?>> GetAllCategoriesAsync()
        {
            return await clsCategoriesData.GetAllCategoriesAsync();
        }

    }
}
