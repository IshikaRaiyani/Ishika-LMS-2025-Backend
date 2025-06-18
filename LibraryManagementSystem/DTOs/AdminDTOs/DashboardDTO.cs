namespace LibraryManagementSystem.DTOs.AdminDTOs
{
    public class DashboardDTO
    {
        public int TotalTransactions { get; set; }
        public int BooksIssuedThisWeek { get; set; }
        public int BooksIssuedThisMonth { get; set; }
        public string MostIssuedBookTitle { get; set; }
        public int TotalOverdueTransactions { get; set; }
        public int TotalFineAmount { get; set; }
        public int ActiveUsersThisWeek { get; set; }
        public int ActiveUsersThisMonth { get; set; }
    }
    

    
}
