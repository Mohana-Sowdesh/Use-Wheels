using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileSystemGlobbing.Internal;

namespace Use_Wheels.Models
{
	public class CustomUserValidator : UserValidator<IdentityUser>
	{
        public override async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            var errors = new List<IdentityError>();

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            string passwrdPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$";

            if (!Regex.IsMatch(user.Email, pattern))
            {
                errors.Add(new IdentityError
                {
                    Description = "Please enter a valid email!!"
                });
            }

            if (!Regex.IsMatch(user.PasswordHash, passwrdPattern))
            {
                errors.Add(new IdentityError
                {
                    Description = "Password must be atleast of 8 characters and must contain atleast 1 lowercase alphabet, 1 uppercase alphabet, 1 number & 1 special character"
                });
            }

            if (errors.Count > 0)
            {
                return IdentityResult.Failed(errors.ToArray());
            }

            return IdentityResult.Success;
        }
    }
}

