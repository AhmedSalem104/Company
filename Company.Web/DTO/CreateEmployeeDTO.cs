using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.Web.DTO
{
    public class CreateEmployeeDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Range(22, 60, ErrorMessage = "Age must be between 22 and 60")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [RegularExpression(@"^[a-zA-Z0-9\s,.\-\/]{5,}$", ErrorMessage = "Invalid address format. Minimum 5 characters, letters, numbers, spaces, and ',.-/' are allowed.")]
        public string? Address { get; set; }

        public string? Phone { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive value")]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

        [DisplayName("Hiring Date")]
        public DateTime HiringDate { get; set; }

        [DisplayName("Created At")]
        public DateTime CreateAt { get; set; }
        public int? DepartmentId { get; set; }

    }
}
