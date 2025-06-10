using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _context;

        public AdminRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIDAsync(int userid)
        {
            return await _context.Users.FindAsync(userid);


        }

        public async Task<string> GetUserRoleAsync(int userid)
        {
            return await _context.Users.Where(u => u.UserId == userid).Select(u => u.RoleName).FirstOrDefaultAsync();
            
           


        }

        public async Task AddAdminAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddLibrarianAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> DeleteLibrarianAsync(User user)
        {
            try
            {
               

               var users =   _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return "Librarian Deleted Successfully";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return "Error in Deleting Librarian";
            }
        }

        public async Task StudentStatusUpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
