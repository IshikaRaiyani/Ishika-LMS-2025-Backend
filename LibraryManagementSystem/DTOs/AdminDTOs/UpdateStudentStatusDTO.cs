using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs.AdminDTOs
{
    public class UpdateStudentStatusDTO
    {
        [Key]
        [Required(ErrorMessage = "User Id is required.")]
        public int UserId { get; set; }

        
    }
}
