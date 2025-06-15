using LibraryManagementSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace LibraryManagementSystem.DTOs.StudentDTOs
{
    public class ReturnBookRequestDTO
    {

        [ForeignKey("UserId")]
        [Required(ErrorMessage = "User Id is required.")]
        public int UserId { get; set; }

        public User User { get; set; }

        [ForeignKey("BookId")]
        [Required(ErrorMessage = "Book Id is required.")]
        public int BookId { get; set; }

        public Book Book { get; set; }

        public string RequestType { get; set; }

        public DateOnly BorrowRequestDate { get; set; }

        public DateOnly ReturnDate { get; set; }

        public DateOnly DueDate { get; set; }
        public string BorrowStatus { get; set; }

        public string ReturnStatus { get; set; }
    }
}
