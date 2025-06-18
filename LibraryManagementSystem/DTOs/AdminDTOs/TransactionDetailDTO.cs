namespace LibraryManagementSystem.DTOs.AdminDTOs
{
    public class TransactionDetailDTO
    {
        public int TransactionId { get; set; }

        public string UserName { get; set; }

        public string BookTitle { get; set; }

        public string RequestType { get; set; }

        public DateOnly? BorrowDate { get; set; }

        public DateOnly? ReturnDate { get; set; }

        public int Fine { get; set; }

        public string BorrowStatus { get; set; }

        public string ReturnStatus { get; set; }


    }
}
