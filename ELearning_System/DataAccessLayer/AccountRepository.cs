using DataAccessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ObjectModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Repository
{
    public class AccountRepository : IAccountRepository
    {
       
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,SignInManager<ApplicationUser> signInManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;

        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            var result = await _userManager.FindByNameAsync(model.Username);

            if (result != null && await _userManager.CheckPasswordAsync(result, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(result);
                var authClaims = new List<Claim>
                {
                   new Claim(ClaimTypes.Name, model.Username),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                   
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                    );
                var Finaltoken = new JwtSecurityTokenHandler().WriteToken(token);
                return Finaltoken;


            }
            return ("UNAUTHORIZED USER");

        }

        public async Task<ApplicationUser> RegisterAsync(RegisterModel model)
        {

            var user = new ApplicationUser()
            {
                UserName = model.Username,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = model.Role,


            };
            var result = await _userManager.CreateAsync(user, model.Password);
            return user;
        }

    }
}