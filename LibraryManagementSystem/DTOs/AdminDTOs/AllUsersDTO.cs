using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs.AdminDTOs
{
    public class AllUsersDTO
    {
        [Key]
        public int UserId { get; set; }
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

       public string Status { get; set; }

        public int NoofBooks { get; set; }
    }
}
