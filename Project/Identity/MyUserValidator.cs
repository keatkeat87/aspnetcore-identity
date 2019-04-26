using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Identity
{
    public class MyUserValidator : IUserValidator<IdentityUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            if (!user.UserName.EndsWith("stooges.com.my"))
            {
                var result = IdentityResult.Failed(new IdentityError
                {
                    Code = "UsernameNotEndsWithStoogesDomain",
                    Description = "username must ends with stooges.com.my"
                });
                return Task.FromResult(result);
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
