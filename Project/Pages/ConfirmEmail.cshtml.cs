using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages
{
public class ConfirmEmailModel : PageModel
{
    public async Task OnGetAsync(
        string token, string userId,
        [FromServices] UserManager<IdentityUser> userManager
    )
    {
        var user = await userManager.FindByIdAsync(userId);
        var result = await userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {

        }
    }
}
}