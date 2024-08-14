using System.ComponentModel.DataAnnotations;

namespace Restaurant_Data_Access.DTOs.EmployeeDTOs
{
    public class EmployeesDTO
    {
        public EmployeesDTO(int? employeeid, string name, string position, DateTime hiredate, decimal? salary)
        {
            this.EmployeeID = employeeid;
            this.Name = name;
            this.Position = position;
            this.HireDate = hiredate;
            this.Salary = salary;
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
    }
}
