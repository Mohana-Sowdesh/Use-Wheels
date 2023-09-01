using System;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Repository.IRepository
{
	public interface IUserRepository
	{
        int IsUniqueUser(string username, string email);
        bool IsAbove18(DateOnly Dob);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}

