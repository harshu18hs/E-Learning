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
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Faculty + "," + UserRoles.Student)]
    public class StudentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private IFileRepository _fileRepository;
        public static IWebHostEnvironment _webHostEnvironment;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private ApplicationDbContext _databaseContext;
        private IChatRepository _chatRepository;
        private IArticleRepository _articleRepository;
        public IAdminRepository _adminRepository;

        private ILogger<StudentController> _logger;
        public StudentController(ILogger<StudentController> logger, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, SignInManager<ApplicationUser> _signInManager, IConfiguration configuration, ApplicationDbContext _databaseContext)
        {
            _logger = logger;
            userManager = _userManager;
            this.roleManager = _roleManager;
            _configuration = configuration;
            this.signInManager = _signInManager;
            this._databaseContext = _databaseContext;
            _webHostEnvironment = webHostEnvironment;
            _fileRepository = new FileRepository(_databaseContext, _webHostEnvironment);
            _articleRepository = new ArticleRepository(_databaseContext, _webHostEnvironment);
            _adminRepository = new AdminRepository(_databaseContext);
            _chatRepository = new ChatRepository(_databaseContext);

        }
        
        [HttpPost]
        public IActionResult UploadProjectFile(List<IFormFile> fileName)
        {
            ActionResult<bool> username = _fileRepository.FileUpload(fileName);
            if (username != null)
            {
                return Ok("File uploaded successfully");
            }
            else
            {
                return BadRequest("File not uploaded");
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
        [HttpDelete]
        public ActionResult DeleteProjectFile(string filename)
        {
            try
            {
                if (filename == null)
                {
                    _logger.LogWarning("FileName should not be null");
                    return NotFound("Please Enter a file Name");
                }
                else
                {
                    var deleteStatus = _fileRepository.DeleteFile(filename);
                    bool success = deleteStatus.Value;
                    if (success)
                    {
                        _logger.LogInformation("File deleted successfully");
                        return Ok("File Deleted successfully");
                    }
                    else
                    {
                        _logger.LogWarning("File deletion failed");
                        return NotFound("Not Deleted");
                    }
                }
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
        
        [HttpGet]
        public ActionResult DownloadResourceFile(int id)
        {
            try
            {
                var file = _databaseContext.Resources.Where(x => x.uploadID == id).FirstOrDefault();
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
            if (username == null)
            {
                _logger.LogWarning("UserName is required");
                return NotFound("Enter valid userName");
            }
            else
            {
                List<Chat> GetMessageList = _databaseContext.Chats.ToList();
                List<Chat> GetChatList = new List<Chat>();
                foreach (Chat chat in GetMessageList)
                {
                    if (chat.ToUserName == username)
                    {
                        GetChatList.Add(chat);
                    }
                }
                return Ok(GetChatList);
            }
        }
        [HttpPost]
        public ActionResult StudentMessage([FromBody] Chat chats)
        {
            try
            {
                if (chats == null)
                {
                    _logger.LogWarning("Chats should not be null");
                    return NotFound("Please enter proper model");
                }
                else
                {
                    var messageStatus = _chatRepository.StudentMessage(chats);
                    bool success = messageStatus.Value;
                    if (success)
                    {
                        _logger.LogInformation("Message sent successfully");
                        return Ok("Message Send ");
                    }
                    else
                    {
                        _logger.LogError("Please try again to send message");
                        return NotFound("Message not send");
                    }
                }
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
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


