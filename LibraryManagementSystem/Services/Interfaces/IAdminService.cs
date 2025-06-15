using LibraryManagementSystem.DTOs.AdminDTOs;
using LibraryManagementSystem.DTOs.BookDTOs;
using LibraryManagementSystem.DTOs.LibrarianDTOs;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IAdminService
    {
        Task<string> AddAdminAsync(AddAdminDTO addAdminDto);

        Task<string> AddLibrarianAsync(AddLibrarianDTO addLibrarianDto);

        Task<string> DeleteLibrarianAsync(int userId);

        Task<string> BlockStudentAsync(UpdateStudentStatusDTO updateStudentStatusDto);

        Task<string> UnblockStudentAsync(UpdateStudentStatusDTO updateStudentStatusDto);

        Task<string> GetActiveUsersAsync();

        Task<string> GetBlockedUsersAsync();

        Task<string> GetUserCountAsync();

        Task<List<AllUsersDTO>> GetAllUsersAsync();

        Task<string> AddBookAsync(Book book);

        Task<string> GetTotalBookCountAsync();

        Task<string> GetTotalAvailableBooksAsync();

        Task<List<GetAllBooksDTO>> GetAllBooksAsync();

        Task<GetBookByIdDTO> GetBookByIdAsync(int Bookid);

        Task<string> DeleteBookAsync(int bookId);

        Task<bool> UpdateBookAsync(UpdateBookDTO dto);

    } 
}
