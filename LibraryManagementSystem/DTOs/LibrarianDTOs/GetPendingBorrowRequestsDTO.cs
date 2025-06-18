using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DTOs.LibrarianDTOs
{
    public class GetPendingBorrowRequestsDTO
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        

        public string FullName { get; set; }

        public int NoofBooks { get; set; }

        public int BookId { get; set; }

       
        public string Title { get; set; }

        public int AvailableCopies { get; set; }

        public DateOnly BorrowRequestDate { get; set; }


        public string BorrowStatus { get; set; }



        


    }
}
