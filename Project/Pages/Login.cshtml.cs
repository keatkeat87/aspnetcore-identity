using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Project.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IdentityOptions IdentityOptions;
        public LoginModel(
            IOptionsSnapshot<IdentityOptions> identityOptions
            )
        {
            IdentityOptions = identityOptions.Value;
        }

        public void OnGet()
        {

        }


        public class LoginInputModel
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        [BindProperty]
        public LoginInputModel LoginData { get; set; }

        public async Task OnPostLoginAsync([FromServices] UserManager<IdentityUser> userManager)
        {
            var user = new IdentityUser
            {
                UserName = LoginData.username
            };
            var reuslt = await userManager.CreateAsync(user, LoginData.password);
            if (reuslt.Succeeded)
            {

            }
        }
    }
}