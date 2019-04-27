using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
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


        public async Task<IActionResult> OnPostLoginAsync(
            [FromServices] UserManager<IdentityUser> userManager,
            [FromServices] SignInManager<IdentityUser> signInManager
        )
        {
            var result = await signInManager.PasswordSignInAsync(LoginData.username, LoginData.password, lockoutOnFailure: true, isPersistent: true);
            if (result.IsLockedOut)
            {
                var user = await userManager.FindByNameAsync(LoginData.username);
                DateTimeOffset? datetime = await userManager.GetLockoutEndDateAsync(user);
            }
            if (result.IsNotAllowed)
            {
                // 报错了, 因为 email 或 phone 还没有 confirm 
            }
            if (result.Succeeded)
            {
                return LocalRedirect("/about");
            }
            return Page();
        }

        public async Task OnPostResetPassword(
           [FromServices] UserManager<IdentityUser> userManager
        )
        {
            var user = await userManager.FindByNameAsync("hengkeat87@gmail.com");
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            await userManager.ResetPasswordAsync(user, token, newPassword: "123456");
        }


        public class RegisterInputModel
        {
            public string username { get; set; }
            public string password { get; set; }
            public string phone { get; set; }
        }

        [BindProperty]
        public RegisterInputModel RegisterData { get; set; }

        public async Task<IActionResult> OnPostRegisterAsync(
            [FromServices] UserManager<IdentityUser> userManager
        )
        {
            var user = new IdentityUser
            {
                UserName = RegisterData.username,
                Email = RegisterData.username, // 加入这个
                PhoneNumber = RegisterData.phone
            };
            var reuslt = await userManager.CreateAsync(user, RegisterData.password);
            if (reuslt.Succeeded)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Page(
                    "ConfirmEmail",
                    pageHandler: null,
                    values: new { userId = user.Id, token },
                    protocol: Request.Scheme
                );
                var subject = "Confirm your email";
                var message = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
                var sendTo = RegisterData.username;

                // EmailService.send(sendTo, subject, message);
                // return RedirectToPage("ConfirmEmail", new { userId = user.Id, token });
                await userManager.ConfirmEmailAsync(user, token);

                token = await userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
                subject = "Confirm your account";
                sendTo = RegisterData.phone;
                message = $"please confirm your account by enter numbers : {token}";

                // SMSService.send(sendTo, subject, message);
                //for (var i = Convert.ToInt32(phoneToken) - 3000; i < 999999; i++)
                //{
                //    var phoneResult = await userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, i.ToString());
                //    if (phoneResult.Succeeded)
                //    {
                //         //暴力破解成功
                //    }
                //}
                await userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, token);
            }
            return Page();
        }
    }
}