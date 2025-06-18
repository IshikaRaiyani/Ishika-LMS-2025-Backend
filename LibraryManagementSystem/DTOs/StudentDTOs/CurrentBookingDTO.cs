namespace LibraryManagementSystem.DTOs.StudentDTOs
{
    public class CurrentBookingDTO
    {
        public int TransactionId { get; set; }
        public string Title { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }

        public DateOnly BorrowDate { get; set; }

        public DateOnly DueDate { get; set; }

    }
}
