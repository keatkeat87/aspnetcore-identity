using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Identity
{
    public class MyPasswordValidator : IPasswordValidator<IdentityUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user, string password)
        {
            if (user.UserName.Equals(password, StringComparison.OrdinalIgnoreCase))
            {
                var result = IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordSameAsUsername",
                    Description = "password can't same as username."
                });
                return Task.FromResult(result);
            } 
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
