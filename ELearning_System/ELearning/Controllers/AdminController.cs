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
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using System.Linq;

namespace ELearning.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]

    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private IFileRepository _fileRepository;
        public static IWebHostEnvironment _webHostEnvironment;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private ApplicationDbContext _databaseContext;
        private IChatRepository _chatRepository;
        public IAdminRepository _adminRepository;
        private ILogger<AdminController> _logger;
        public AdminController(ILogger<AdminController> logger, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, SignInManager<ApplicationUser> _signInManager, IConfiguration configuration, ApplicationDbContext _databaseContext)
        {
            _logger = logger;
            userManager = _userManager;
            this.roleManager = _roleManager;
            _configuration = configuration;
            this.signInManager = _signInManager;
            this._databaseContext = _databaseContext;
            _webHostEnvironment = webHostEnvironment;
            _fileRepository = new FileRepository(_databaseContext, _webHostEnvironment);
            _adminRepository = new AdminRepository(_databaseContext);
            _chatRepository = new ChatRepository(_databaseContext);

        }
        [HttpGet]
        public ActionResult UsersList()
        {
            var usersList = _databaseContext.AspNetUsers.ToList();
            if (usersList.Count == null)
            {
                _logger.LogWarning("The usersList is Empty");
                return NotFound("No users found");
            }
            else
            {
                _logger.LogInformation("UsersList retrieved successfully");
                return Ok(usersList);
            }
        }
        [HttpGet]
        public ActionResult GetParticularUser(string Id)
        {
            var user = _databaseContext.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();
            if (user == null)
            {
                _logger.LogWarning("The userslist is empty");
                return NotFound("No users found");
            }
            else
            {
                return Ok(user);
            }
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteUsers(string username)
        {
            try
            {
                if (username == null)
                {
                    _logger.LogWarning("UserName should not be null");
                    return NotFound("Please Enter a UserName");
                }
                else
                {
                    var user = await userManager.FindByNameAsync(username);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var result = await userManager.DeleteAsync(user);
                        return Ok("User Deleted Successfully");
                    }
                }
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
        [HttpPatch]
        public ActionResult UpdateAdminDetails(string username, [FromBody] Details User)
        {
            if (username == null || User == null)
            {
                _logger.LogWarning("Values should not be null");
                return NotFound("Values should not be null");
            }
            else
            {
                var result = _adminRepository.UpdateUsers(username, User);
                bool output = result.Value;
                if (output)
                {
                    return Ok("User Updated Successfully");
                }
                else
                {
                    return NotFound("User not updated.Check your model");
                }
            }
        }
        [HttpGet]
        public ActionResult DownloadProjectFile(int id)
        {
            try
            {
                var file = _databaseContext.Files.Where(x => x.uploadID == id).FirstOrDefault();
                if (file == null) return NotFound("File does not exist");
                var memory = new MemoryStream();
                using (var stream = new FileStream(file.FilePath, FileMode.Open))
                {
                    stream.CopyTo(memory);
                }
                memory.Position = 0;
                return File(memory, file.FileType, file.fileName + file.FileExtension);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        public ActionResult GetMessageList(string username)
        {
            try
            {
                if (username == null)
                {
                    _logger.LogWarning("Username is required");
                    return NotFound("Message Sent Successfully");
                }
                else
                {
                    var result = _chatRepository.GetChatList(username);
                    List<Chat> chatList = result.Value;
                    return Ok(chatList);
                }
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
        [HttpPost]
        public ActionResult AdminMessage([FromBody] Chat chats)
        {
            var result = _chatRepository.AdminMessage(chats);
            bool output = result.Value;
            if (output)
            {
                return Ok("Message Sent Successfully");
            }
            else
            {
                return NotFound("Message not sent");
            }
        }
        [HttpPut]
        public ActionResult<bool> ResponseMessage(int? id, string username, string responseMessage, string status)
        {
            if (id == null || username == null || responseMessage == null || status == null)
            {
                _logger.LogWarning("Chats should not be null");
                return NotFound("Please enter proper model");
            }
            else
            {
                Chat responseChat = _databaseContext.Chats.Find(id);
                if (responseChat == null)
                {
                    _logger.LogWarning("Chats should not be null");
                    return NotFound("Please enter proper model");
                }
                else
                {
                    responseChat.ResponseMessage = responseMessage;
                    responseChat.ResponseReceivedAt = DateTime.Now;
                    responseChat.Status = status;
                    _databaseContext.SaveChanges();
                    _logger.LogWarning("Response send successfully");
                    return Ok("Response send successfully");
                }
            }
        }
       
      
    }
}
