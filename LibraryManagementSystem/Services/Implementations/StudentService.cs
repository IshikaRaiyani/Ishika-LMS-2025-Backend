using System.Security.Cryptography;
using System.Text;
using LibraryManagementSystem.DTOs.UserDTOs;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Services.Interfaces;

namespace LibraryManagementSystem.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<string> RegistrationStudentAsync(RegisterStudentDTO registerStudentDTO)
        {
            try
            {
                if (registerStudentDTO == null)
                {
                    throw new Exception("Please enter the valid User Admin Details.");
                }

                if (await _studentRepository.IsEmailExistsAsync(registerStudentDTO.Email))
                {
                    return "This Email Address already exists. Please enter a valid Email Address.";
                }

                var sha256 = SHA256.Create();
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(registerStudentDTO.Password));

                var UserAdmin = new User
                {
                    FullName = registerStudentDTO.FullName,
                    Email = registerStudentDTO.Email,
                    Password = Convert.ToBase64String(hashBytes),
                   
                };

                await _studentRepository.AddStudentAsync(UserAdmin);
                return "Student Registered Successfully";
            }

            catch (Exception ex)
            {
                return ex.InnerException?.Message ?? ex.Message;
            }
        }
    }

}
