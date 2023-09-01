using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Use_Wheels.Data;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository.IRepository;
using Serilog;

namespace Use_Wheels.Repository
{
	public class UserRepository : IUserRepository
	{
        private readonly ApplicationDbContext _db;
        private readonly UserManager<UserDTO> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;
        private readonly IMapper _mapper;
        private readonly ILogger<IUserRepository> _logger;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration,
            UserManager<UserDTO> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager, ILogger<UserRepository> logger)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _roleManager = roleManager;
            _logger = logger;
        }

        public int IsUniqueUser(string username, string email)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == username);
            var userEmail = _db.Users.FirstOrDefault(x => x.Email == email);

            if (user != null)
            {
                _logger.LogError("Requested username already exists");
                return -1;
            }
            else if (userEmail != null)
            {
                _logger.LogError("Requested email already exists");
                return 0;
            }
            return 1;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.Users
                .FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);


            if (user == null || isValid == false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            //if user was found generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDTO>(user),

            };
            return loginResponseDTO;
        }

        public async Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            UserDTO user = new()
            {
                UserName = registerationRequestDTO.Username,
                Role = "customer",
                Email = registerationRequestDTO.Email,
                First_Name = registerationRequestDTO.First_Name,
                Last_Name = registerationRequestDTO.Last_Name,
                Dob = registerationRequestDTO.Dob,
                Phone_Number = registerationRequestDTO.Phone_Number,
                Gender = registerationRequestDTO.Gender
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    await _userManager.AddToRoleAsync(user, "customer");
                    var userToReturn = _db.Users
                        .FirstOrDefault(u => u.UserName == registerationRequestDTO.Username);
                    return _mapper.Map<UserDTO>(userToReturn);

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return new UserDTO();
        }

        public bool IsAbove18(DateOnly Dob)
        {
            int birthYear = Dob.Year;
            int currentYear = DateTime.Today.Year;

            if(currentYear - birthYear < 18)
            {
                _logger.LogError("User age is not above or equal to 18");
                return false;
            }
            return true;
        }
    }
}

