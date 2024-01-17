using GreenSpecial.Areas.ViewModels.Account;
using GreenSpecial.Models;
using GreenSpecial.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GreenSpecial.Areas.GreenAdmin.Controllers
{
	[Area("GreenAdmin")]
	public class AccountController : Controller
	{
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
		{
            _userManager = userManager;
            _signInManager = signInManager;
			_roleManager = roleManager;
		}
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterVM registerVM)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			AppUser user = new AppUser
			{
				UserName = registerVM.Username,
				Email = registerVM.Email
			};
			IdentityResult identityResult = await _userManager.CreateAsync(user, registerVM.Password);
			if (!identityResult.Succeeded)
			{
				foreach (var item in identityResult.Errors)
				{
					ModelState.AddModelError(String.Empty, item.Description);
					return View();
				}
			}
			await _signInManager.SignInAsync(user, false);
			return RedirectToAction("Index", "Home", new { area = "" });
		}
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			AppUser existed = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
			if (existed is null)
			{
				existed = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
				if (existed is null)
				{
					ModelState.AddModelError(String.Empty, "Username Email or Password is incorrect");
					return View();
				}
			}
			var passwordCheck =await _signInManager.PasswordSignInAsync(existed, loginVM.Password, loginVM.IsRemembered, false);
			if (!passwordCheck.Succeeded)
			{
                ModelState.AddModelError(String.Empty, "Username Email or Password is incorrect");
                return View();
            }
			if (passwordCheck.IsLockedOut)
			{
                ModelState.AddModelError(String.Empty, "Bloklandiniz hahah");
                return View();
            }
			return RedirectToAction("Index", "Home");
		}
		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
		//public async Task<IActionResult> CreateRoles()
		//{
		//	foreach (var item in Enum.GetValues(typeof(UserRoles)))
		//	{
		//		_roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
		//	}
		//	return RedirectToAction("Index", "Home");
		//}

	}
}
