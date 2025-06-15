namespace LibraryManagementSystem.DTOs.StudentDTOs
{
    public class PendingRequestDTO
    {
        public string Title { get; set;}

        public string Author { get; set; }

        public DateOnly BorrowRequestDate { get; set; }

        public DateOnly BorrowDate { get; set; }

        public DateOnly DueDate { get; set; }

        public string BorrowStatus { get; set; }

        public string ReturnStatus { get; set; }

    }
}
