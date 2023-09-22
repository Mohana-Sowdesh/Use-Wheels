using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Use_Wheels.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration,
            UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            secretKey = configuration.GetValue<string>(Constants.Configurations.JWT_SECRET_CONFIGURATION_KEY);
            _roleManager = roleManager;
        }

        // Method log in a user
        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            User user = _db.Users.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);


            if (user == null || isValid == false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            if(user.isBlacked == true)
                throw new BadHttpRequestException(Constants.LoginConstants.USER_IS_BLACKED, Constants.ResponseConstants.BAD_REQUEST);

            user.Last_Login = DateTime.Now;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
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

        // Method to register a new user
        public async Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            User user = _mapper.Map<User>(registerationRequestDTO);
            RegisterUtility registerUtility = new RegisterUtility(_db);
            registerUtility.ValidateNewRegisterationRequest(registerationRequestDTO);

            var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("seller").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("customer"));
                    await _roleManager.CreateAsync(new IdentityRole("seller"));
                }
                await _userManager.AddToRoleAsync(user, registerationRequestDTO.Role);
                var userToReturn = _db.Users.FirstOrDefault(u => u.UserName == registerationRequestDTO.Username);
                return _mapper.Map<UserDTO>(userToReturn);
            }
            else
            {
                Console.WriteLine(result.Errors.ToList());
            }
            return new UserDTO();
        }

        // Method to unblack a seller
        public async Task<int> UnblackSeller(string username)
        {
            User user = _db.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
                return -1;

            user.isBlacked = false;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return 1;
        }
    }
}

