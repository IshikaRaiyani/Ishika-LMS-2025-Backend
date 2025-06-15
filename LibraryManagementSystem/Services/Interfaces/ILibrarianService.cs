

using LibraryManagementSystem.DTOs.LibrarianDTOs;
using LibraryManagementSystem.DTOs.StudentDTOs;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface ILibrarianService
    {
        Task<string> ApproveBorrowRequestAsync(int TransactionId);

        Task<string> RejectBorrowRequestAsync(int TransactionId);

        Task<List<GetPendingBorrowRequestsDTO>> GetPendingBorrowRequestsAsync();

        Task<string> ApproveReturnRequestAsync(int TransactionId);
    }
}
