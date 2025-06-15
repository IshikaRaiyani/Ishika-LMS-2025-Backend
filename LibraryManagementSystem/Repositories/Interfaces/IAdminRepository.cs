using LibraryManagementSystem.DTOs.AdminDTOs;
using LibraryManagementSystem.DTOs.BookDTOs;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<bool> IsEmailExistsAsync(string email);
        Task AddAdminAsync(User user);

        Task AddLibrarianAsync(User user);

        Task<User> GetUserByIDAsync(int userId);
        Task<string> DeleteLibrarianAsync(User user);

        Task<string> GetUserRoleAsync(int userid);



        Task StudentStatusUpdateAsync(User user);

        Task<string> GetActiveUsersAsync();

        Task<string> GetBlockedUsersAsync();

        Task<string> GetUserCountAsync();

        Task<List<AllUsersDTO>> GetAllUsersAsync();

        Task AddBookAsync(Book book);

        Task<string> GetTotalBookCountAsync();

        Task<string> GetTotalAvailableBooksAsync();

        Task<List<GetAllBooksDTO>> GetAllBooksAsync();

        Task<Book> GetBookByIDAsync(int bookId);

        Task<string> DeleteBookAsync(Book book);

        Task<Book?> GetBookbyIdAsync(int bookId);

        Task<bool> UpdateBookAsync(Book book);

        Task<bool> UpdateUserAsync(User user);




    }
}
