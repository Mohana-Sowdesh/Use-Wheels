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

        public bool IsUniqueUser(string username)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
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
                Expires = DateTime.UtcNow.AddDays(1),
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
                if (!IsAbove18(user.Dob))
                    return null;
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
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("User must be above 18 years of age")),
                    ReasonPhrase = "Registration unsuccessful"
                };
                throw new HttpResponseException(resp);
            }
            return true;
        }
    }
}

