using LibraryManagementSystem.Models;
using LibraryManagementSystem.DTOs.UserDTOs;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.DTOs.StudentDTOs;
using LibraryManagementSystem.DTOs.BookDTOs;

namespace LibraryManagementSystem.Repositories.Interfaces

{
    public interface IStudentRepository
    {
        Task<bool> IsEmailExistsAsync(string email);
        Task AddStudentAsync(User user);

        Task<bool> isBookExistingAsync(int userid, int bookid, string borrowStatus, string borrowStatus1, string returnStatus, string returnStatus1);

         Task<int> GetUserBooksAsync(int UserId);

        Task BorrowBookRequestAsync(BookManagement bookManagement);

        Task<User> GetUserByIDAsync(int userid);

        Task<List<BookingHistoryDTO>> GetBookingHistoryAsync(int UserId);

        Task<List<PendingRequestDTO>> GetPendingRequestAsync(int UserId);

        Task<List<CurrentBookingDTO>> CurrentBookingAsync(int UserId);

        Task<bool> ReturnBookRequestAsync(BookManagement bookManagement);

        Task AddtoWishlistAsync(Studentwishlist studentwishlist);

        Task<bool> IsBookInWishlistAsync(int userId, int bookId);

        Task<List<Studentwishlist>> GetUserWishlistsAsync(int userId);

        Task<bool> RemoveFromWishlistAsync(int wishlistId);

        Task<List<GetAllBooksDTO>> GetBestSellingBooksAsync();

        Task<List<GetAllBooksDTO>> GetNewArrivalsAsync();

        //Task<List<Book>> GetUserBookHistoryAsync(int userid);

        //Task<List<Book>> GetMostIssuedBooksAsync(int count);

        //Task<List<Book>> GetBooksByAuthorOrGenreAsync(List<string> genre, List<string> author, List<int> readBookIds);
        Task<List<Book>> GetRecommendedBooksAsync(int userid);

        Task<List<Studentwishlist>> FindWishlistAsync(int bookId);

        
    }
}
