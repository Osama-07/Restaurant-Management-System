

using Restaurant_Data_Access.DTOs.EmployeeDTOs;
using Restaurant_Data_Access;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_Business
{
    public class clsEmployees
    {
        public enum enMode { AddNew = 1, Update = 2 }
        public enMode Mode;

        public EmployeesDTO EDTO
        {
            get
            {
                return new EmployeesDTO
                 (
                    this.EmployeeID,
                    this.Name,
                    this.Position,
                    this.HireDate,
                    this.Salary
                 );
            }
        }
        public clsEmployees(EmployeesDTO EDTO, enMode mode = enMode.AddNew)
        {
            this.EmployeeID = EDTO.EmployeeID;
            this.Name = EDTO.Name;
            this.Position = EDTO.Position;
            this.HireDate = EDTO.HireDate;
            this.Salary = EDTO.Salary;

            this.Mode = mode;
        }

        [Range(0, int.MaxValue, ErrorMessage = "EmployeeID must be between 0 and the maximum value of an integer.")]
        public int? EmployeeID { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } // Length: 100
        [Required(ErrorMessage = "Position is required.")]
        [MaxLength(50, ErrorMessage = "Position cannot exceed 50 characters.")]
        public string Position { get; set; } // Length: 50
        [Required(ErrorMessage = "HireDate is required.")]
        public DateTime HireDate { get; set; }
        public decimal? Salary { get; set; } // allow null.

        private bool _AddNewEmployee()
        {
            this.EmployeeID = clsEmployeesData.AddNewEmployee(EDTO);

            return (this.EmployeeID > 0);
        }

        private async Task<bool> _AddNewEmployeeAsync()
        {
            this.EmployeeID = await clsEmployeesData.AddNewEmployeeAsync(EDTO);

            return (this.EmployeeID > 0);
        }

        private bool _UpdateEmployee()
        {
            return clsEmployeesData.UpdateEmployee(EDTO);
        }

        private async Task<bool> _UpdateEmployeeAsync()
        {
            return await clsEmployeesData.UpdateEmployeeAsync(EDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewEmployee())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateEmployee();
            }
            return false;
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewEmployeeAsync())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return await _UpdateEmployeeAsync();
            }
            return false;
        }

        public static clsEmployees? GetEmployeeByID(int? id)
        {
            if (id < 1 || id == null) return null;

            EmployeesDTO? eDTO = clsEmployeesData.GetEmployeeByID(id);

            if (eDTO != null)
            {
                return new clsEmployees(eDTO, enMode.Update);
            }
            else
                return null;
        }

        public static async Task<clsEmployees?> GetEmployeeByIDAsync(int? id)
        {
            if (id < 1 || id == null) return null;

            EmployeesDTO? eDTO = await clsEmployeesData.GetEmployeeByIDAsync(id);

            if (eDTO != null)
            {
                return new clsEmployees(eDTO, enMode.Update);
            }
            else
                return null;
        }

        public static bool IsEmployeeExists(int? id)
        {
            if (id < 1 || id == null) return false;

            return clsEmployeesData.IsEmployeeExists(id);
        }

        public static async Task<bool> IsEmployeeExistsAsync(int? id)
        {
            if (id < 1 || id == null) return false;

            return await clsEmployeesData.IsEmployeeExistsAsync(id);
        }

        public static bool DeleteEmployee(int? id)
        {
            if (id < 1 || id == null) return false;

            return clsEmployeesData.DeleteEmployee(id);
        }

        public static async Task<bool> DeleteEmployeeAsync(int? id)
        {
            if (id < 1 || id == null) return false;

            return await clsEmployeesData.DeleteEmployeeAsync(id);
        }

        public static List<EmployeesDTO?> GetAllEmployees()
        {
            return clsEmployeesData.GetAllEmployees();
        }
        
        public static async Task<List<EmployeesDTO?>> GetAllEmployeesAsync()
        {
            return await clsEmployeesData.GetAllEmployeesAsync();
        }

    }
}
