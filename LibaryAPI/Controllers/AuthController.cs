using LibaryAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibaryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user=new IdentityUser {UserName=model.Login, Email = model.Email };
            var result= await _userManager.CreateAsync(user,model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");

                return Ok("Пользователь успешно зарегистрирован");
            }
            return BadRequest(result.Errors);
        }
        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login (LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, false, false);
            if (result.Succeeded)
            {
                return Ok("Успешно вошел");
            }
            return Unauthorized("Неверный данные");

        }
     
    }
}
