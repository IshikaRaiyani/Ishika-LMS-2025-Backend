using LibraryManagementSystem.Models;
using LibraryManagementSystem.DTOs.UserDTOs;

namespace LibraryManagementSystem.Repositories.Interfaces

{
    public interface IStudentRepository
    {
        Task<bool> IsEmailExistsAsync(string email);
        Task AddStudentAsync(User user);
    }
}
