using LibraryManagementSystem.DTOs.StudentDTOs;
using LibraryManagementSystem.DTOs.UserDTOs;


namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IStudentService
    {
        Task<string> RegistrationStudentAsync(RegisterStudentDTO regsiterStudentDto);

        Task<string> BorrowBookRequestAsync(BorrowBookRequestDTO borrowBookRequestDTO);

        Task<List<BookingHistoryDTO>> GetBookingHistory(int UserId);

        Task<List<PendingRequestDTO>> GetPendingRequest(int UserId);

        Task<List<CurrentBookingDTO>> CurrentBookingAsync(int UserId);

        Task<string> ReturnBookRequestAsync(int TransactionId);
    }
}
