using LibraryManagementSystem.DTOs.UserDTOs;


namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IStudentService
    {
        Task<string> RegistrationStudentAsync(RegisterStudentDTO regsiterStudentDto);
    }
}
