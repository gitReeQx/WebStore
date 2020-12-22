using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager)
        {
            userManager = UserManager;
            signInManager = SignInManager;
        }

        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid)
                return View(Model);

            var user = new User
            {
                UserName = Model.UserName
            };

            var registration_result = await userManager.CreateAsync(user, Model.Password);
            if (registration_result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach(var error in registration_result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(Model);
        }
    }
}
