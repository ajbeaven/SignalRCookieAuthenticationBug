using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace SignalRCookieAuthenticationBug.Web.Controllers
{
	public class AccountController : Controller
	{
		public async Task<IActionResult> Login()
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, "123"),
				new Claim(ClaimTypes.Name, "MyUser"),
			};

			var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
			var claimsPrincipal = new ClaimsPrincipal(identity);

			await HttpContext.SignInAsync(
				IdentityConstants.ApplicationScheme,
				claimsPrincipal,
				new AuthenticationProperties
				{
					AllowRefresh = true,
					ExpiresUtc = DateTime.UtcNow.AddDays(14)
				});

			return RedirectToAction("Index", "Home");
		}

		public IActionResult Logout()
		{
			HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
			return RedirectToAction("Index", "Home");
		}
	}
}
