using Microsoft.AspNetCore.Mvc;
using ObjectModel;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IAccountRepository
    {
        Task<string> LoginAsync(LoginModel model);
        Task<ApplicationUser> RegisterAsync(RegisterModel model);
    }
}
