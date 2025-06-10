using LibraryManagementSystem.Models;

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




    }
}
