using LibraryManagementSystem.Models;
using LibraryManagementSystem.DTOs.UserDTOs;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.DTOs.StudentDTOs;

namespace LibraryManagementSystem.Repositories.Interfaces

{
    public interface IStudentRepository
    {
        Task<bool> IsEmailExistsAsync(string email);
        Task AddStudentAsync(User user);

        Task<bool> isBookExistingAsync(int userid, int bookid, string borrowStatus, string borrowStatus1, string returnStatus, string returnStatus1);

         Task<int> GetUserBooks(int UserId);

        Task BorrowBookRequestAsync(BookManagement bookManagement);

        Task<User> GetUserByIDAsync(int userid);

        Task<List<BookingHistoryDTO>> GetBookingHistory(int UserId);

        Task<List<PendingRequestDTO>> GetPendingRequest(int UserId);

        Task<List<CurrentBookingDTO>> CurrentBookingAsync(int UserId);

        Task<bool> ReturnBookRequestAsync(BookManagement bookManagement);


    }
}
