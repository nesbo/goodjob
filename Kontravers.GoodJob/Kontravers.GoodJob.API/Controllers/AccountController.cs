using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kontravers.GoodJob.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AccountController : Controller
{
    [HttpGet("SignIn")]
    public ActionResult SignIn([FromQuery]string returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
            return Ok(User.Claims.Select(c => new { type = c.Type, value = c.Value }));
        return Redirect(returnUrl);
    }
    
    [AllowAnonymous]
    [HttpGet("SignOut")]
    public async Task<ActionResult> SignOut([FromQuery] string returnUrl = null)
    {
        await HttpContext.SignOutAsync("Cookies");
        var redirectUrl = Url.Action("SignOutComplete", new { returnUrl });
        return SignOut(new AuthenticationProperties { RedirectUri = redirectUrl }, "oidc");
    }

    [AllowAnonymous]
    [HttpGet("SignOutComplete")]
    public ActionResult SignOutComplete(string returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
            return Redirect("~/");
        return Redirect(returnUrl);
    }
}