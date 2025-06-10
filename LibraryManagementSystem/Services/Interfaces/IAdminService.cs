using LibraryManagementSystem.DTOs.AdminDTOs;
using LibraryManagementSystem.DTOs.LibrarianDTOs;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IAdminService
    {
        Task<string> AddAdminAsync(AddAdminDTO addAdminDto);

        Task<string> AddLibrarianAsync(AddLibrarianDTO addLibrarianDto);

        Task<string> DeleteLibrarianAsync(int userId);

        Task<string> BlockStudentAsync(UpdateStudentStatusDTO updateStudentStatusDto);

        Task<string> UnblockStudentAsync(UpdateStudentStatusDTO updateStudentStatusDto);

    }
}
