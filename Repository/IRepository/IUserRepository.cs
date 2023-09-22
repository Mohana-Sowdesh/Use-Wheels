using System;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Repository.IRepository
{
	public interface IUserRepository
	{
        // Method log in a user
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        // Method to register a new user
        Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);

        // Method to unblack a seller
        Task<int> UnblackSeller(string username);
    }
}

