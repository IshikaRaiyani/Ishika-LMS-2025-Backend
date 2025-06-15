using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DTOs.StudentDTOs
{
    public class BorrowApproveDTO
    {
        public int TransactionId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }


    }
}
