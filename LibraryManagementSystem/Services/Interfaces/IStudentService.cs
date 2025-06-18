using LibraryManagementSystem.DTOs.BookDTOs;
using LibraryManagementSystem.DTOs.StudentDTOs;
using LibraryManagementSystem.DTOs.UserDTOs;
using LibraryManagementSystem.Models;


namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IStudentService
    {
        Task<string> RegistrationStudentAsync(RegisterStudentDTO regsiterStudentDto);

        Task<string> BorrowBookRequestAsync(BorrowBookRequestDTO borrowBookRequestDTO);

        Task<List<BookingHistoryDTO>> GetBookingHistoryAsync(int UserId);

        Task<List<PendingRequestDTO>> GetPendingRequestAsync(int UserId);

        Task<List<CurrentBookingDTO>> CurrentBookingAsync(int UserId);

        Task<string> ReturnBookRequestAsync(int TransactionId);

        Task<string> AddtoWishlistAsync(int userId, int bookId);

        Task<List<Studentwishlist>> GetUserWishlistsAsync(int userId);

        Task<bool> RemoveFromWishlistAsync(int wishlistId);

        Task<List<GetAllBooksDTO>> GetBestSellingBooksAsync();

        Task<List<GetAllBooksDTO>> GetNewArrivalsAsync();

        Task<List<Book>> BookRecommendationsAsync(int userid);

        
    }
    
}
