using AutoMapper;
using IdentityByExamples.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityByExamples.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AccountController(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }
            var user = _mapper.Map<User>(userModel);
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View(userModel);
            }
            await _userManager.AddToRoleAsync(user, "Visitor");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel userModel, string returnUrl = null)
        {
            //check if the ModelState is valid, if not return to the view
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }
            //find user in the database with the same email. 
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            // if that user exists and password match, create ClaimsIdentity object and add a couple of claims
            if (user != null &&
                await _userManager.CheckPasswordAsync(user, userModel.Password))
            {
                var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                // user id as NameIdentifier claim
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                // username as name claim. 
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                //sign the user in with SignInAsync method. This will create the Identity.Application cookie in our browser
                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
                    new ClaimsPrincipal(identity));
                //return RedirectToAction(nameof(HomeController.Index), "Home");
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Invalid UserName or Password");
                return View();
            }
        }
        //Method that redirect the user after login to appropriate location
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");

        }


    }
}
