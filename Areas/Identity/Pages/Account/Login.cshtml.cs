#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NewAppBookShop.Areas.Identity.Data;

namespace NewAppBookShop.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<APpUser> _signInManager;
        private readonly UserManager<APpUser> _userManager; // Thêm UserManager
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<APpUser> signInManager, 
                          UserManager<APpUser> userManager, // Inject UserManager
                          ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager; // Gán giá trị
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    // Tìm người dùng dựa trên Email
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user != null)
                    {
                        // Kiểm tra vai trò của người dùng
                        if (await _userManager.IsInRoleAsync(user, "Admin"))
                        {
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }

                        // Nếu không phải Admin, chuyển hướng về returnUrl
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                        else
                        {
                             return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                    }
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // Nếu có lỗi, hiển thị lại form
            return Page();
        }
    }
}
