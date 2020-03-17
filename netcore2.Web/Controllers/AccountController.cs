using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using netcore2.Web.Models.IdentityModels;

namespace netcore2.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
           private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.PassWord, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "用户名或密码不正确");
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            var user = new ApplicationUser()
            {
                UserName = registerViewModel.UserName
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.PassWord);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", "注册失败");
            return View(registerViewModel);
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}