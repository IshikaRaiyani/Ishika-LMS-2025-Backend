using LibraryManagementSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs.StudentDTOs
{
    public class BorrowBookRequestDTO
    {

        [ForeignKey("UserId")]
        [Required(ErrorMessage = "User Id is required.")]
        public int UserId { get; set; }

        //public User User { get; set; }

        [ForeignKey("BookId")]
        [Required(ErrorMessage = "Book Id is required.")]
        public int BookId { get; set; }

        //public Book Book { get; set; }

        public string RequestType { get; set; }

        public DateOnly BorrowRequestDate { get; set; }
        
        public string BorrowStatus { get; set; }

        public string ReturnStatus { get; set; }
    }
}
