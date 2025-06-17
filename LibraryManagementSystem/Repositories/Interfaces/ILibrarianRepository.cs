using LibraryManagementSystem.DTOs.LibrarianDTOs;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories.Interfaces
{
    public interface ILibrarianRepository 
    {
        Task<BookManagement> GetTransactionById(int TransactionId);

        Task<int> GetUserFineAsync(int UserId);

        Task<bool> ApproveBorrowRequestAsync(BookManagement bookManagement);

        Task<bool> RejectBorrowRequestAsync(BookManagement bookManagement);

        Task<List<GetPendingBorrowRequestsDTO>> GetPendingBorrowRequestsAsync();

        Task<bool> ApproveReturnRequestAsync(BookManagement bookManagement);

        Task<List<BookManagement>> GetPendingReturnRequestsAsync();
    }
}
