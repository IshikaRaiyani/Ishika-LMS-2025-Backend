namespace LibraryManagementSystem.DTOs.LibrarianDTOs
{
    public class GetPendingReturnRequestDTO
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }

        public string FullName { get; set; }

        public int BookId { get; set; }

        public string Title { get; set; }

        public DateOnly BorrowDate { get; set; }

        public DateOnly DueDate { get; set; }

       public int ApplicableFine { get; set; }

    }
}
