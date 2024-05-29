using EvAldarado.DAL;
using EvAldarado.Extension;
using EvAldarado.Extensions;
using EvAldarado.Models;
using EvAldarado.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EvAldarado.Controllers
{
    public class AccountController : Controller
    {

       
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly IEmailService _emailService;


        public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Users user = new Users()
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { user.Id, token });
                await _emailService.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your account by <a href='https://localhost:7294{confirmationLink}'>clicking here</a>.");

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }


        public IActionResult LoginI()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginI(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("LoginI", model); // Return the same view with the model containing errors
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.IsRemember, false);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Password or email incorrect");
                    return View("LoginI", model); // Return the same view with the model containing errors
                }
            }

            return RedirectToAction("Index", "Home"); // Redirect to home on successful login
        }


        public async Task<IActionResult> ConfirmEmail(string Id, string token)
        {
            if (Id == null || token == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(Id);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    return RedirectToAction("LoginI", "Account");
                }
            }
            return View();
        }






        // Add this action method in your AccountController

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // This will clear the user's session
            return RedirectToAction("LoginI", "Account"); // Redirect the user to the login page after logging out
        }







		// Forgot password form
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user != null && await _userManager.IsEmailConfirmedAsync(user))
				{
					// Generate the reset password token
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);

					// Generate the password reset link
					var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

					// Send the email
					await _emailService.SendEmailAsync(model.Email, "Reset Password",
						$"Please reset your password by clicking <a href='{passwordResetLink}'>here</a>.");

					return RedirectToAction("ForgotPasswordConfirmation");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		// Password reset form
		public IActionResult ResetPassword(string email, string token)
		{
			if (email == null || token == null)
			{
				ModelState.AddModelError("", "Invalid password reset token");
			}
			return View(new ResetPasswordViewModel { Email = email, Token = token });
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user != null)
				{
					var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
					if (result.Succeeded)
					{
						return RedirectToAction("ResetPasswordConfirmation");
					}
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}
			return View(model);
		}

		// Confirmation view for forgot password
		public IActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		// Confirmation view for reset password
		public IActionResult ResetPasswordConfirmation()
		{
			return View();
		}

	}
}
