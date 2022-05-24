using DataAccessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ObjectModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AdminRepository : IAdminRepository
    {
        public ApplicationDbContext _databaseContext;
        public AdminRepository(ApplicationDbContext dbContext)
        {
            _databaseContext = dbContext;
        }
        public ActionResult<bool> UpdateUsers(String userName, Details User)
        {
            if (userName == null || User == null)
            {
                throw new ArgumentException();
            }
            else
            {
                var result = _databaseContext.AspNetUsers.Where(x => x.UserName == userName).FirstOrDefault();
                if (User == null)
                {
                    return false;
                }
                else
                {
                    result.UserName = User.UserName;
                    result.Email = User.Email;
                    result.PhoneNumber = User.PhoneNumber;
                    _databaseContext.AspNetUsers.Update(result);
                    _databaseContext.SaveChanges();
                    return true;
                }
            }
        }
    }
}
