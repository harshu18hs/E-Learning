using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ObjectModel;
using System.Threading.Tasks;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;

namespace ELearning.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IAccountRepository accountRepository, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _accountRepository = accountRepository;
            _roleManager = roleManager;
            _userManager = userManager;

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {

            var result = await _accountRepository.RegisterAsync(model);
            if (result != null)
            {
                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Role));
                }
                if (await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _userManager.AddToRoleAsync(result, model.Role);
                }
                return Ok(result);
            }

            return Unauthorized();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _accountRepository.LoginAsync(model);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }
            return Ok(result);

        }
    }
}