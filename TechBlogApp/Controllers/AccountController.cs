using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TechBlogApp.Domain.Models;
using TechBlogApp.Domain.Static;
using TechBlogApp.Domain.ViewModels;
using TechBlogApp.Helpers;
using TechBlogApp.Persistence;

namespace TechBlogApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly JwtService _jwtService;
        private readonly SignInManager<AppUser> signInManager;
        private IPasswordHasher<AppUser> passwordHasher;

        public AccountController
            (
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IPasswordHasher<AppUser> passwordHasher, 
            JwtService jwtService
            )
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordHasher = passwordHasher;
            _jwtService = jwtService;

        }

        // GET /account/login
        //[AllowAnonymous]
        //public IActionResult Login(string returnUrl)
        //{
        //    Login login = new Login
        //    {
        //        ReturnUrl = returnUrl
        //    };

        //    return View(login);
        //}

        //Login
        public IActionResult Login() => View(new LoginVM());

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {

            if (!ModelState.IsValid) return View(loginVM);

            var user = await userManager.FindByEmailAsync(loginVM.EmailAddress);


            if (user != null)
            {
                var passwordCheck = await userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {

                    var result = await signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        var token = _jwtService.Generate(user.Id);
                        Response.Cookies.Append("jwt", token, new CookieOptions
                        {
                            HttpOnly = true
                        });
                        TempData["Success"] = "Successfully Authenticated!";
                        return RedirectToAction("Index", "Article");
                    }
                }

                TempData["Error"] = "Wrong credentials. Please, try again!";
                return View(loginVM);

            }

            TempData["Error"] = "Wrong credentials. Please, try again!";
            return View(loginVM);
        }

        //Register
        public IActionResult Register() => View(new RegisterVM());

        [HttpPost]

        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);
            var user = await userManager.FindByEmailAsync(registerVM.EmailAddress);

            if (user != null)
            {
                TempData["Error"] = "This email is already in use!";
                return View(registerVM);
            }
            var newUser = new AppUser()
            {
                Fullname = registerVM.FirstName + " " + registerVM.LastName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress,
                EmailConfirmed = true

            };
            //var token = _jwtService.Generate(newUser.Id);

            var newUserResponese = await userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponese.Succeeded)
                await userManager.AddToRoleAsync(newUser, UserRoles.User);

            return View("RegisterCompleted");

        }

        //Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {

            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Article");
        }

    }
}
