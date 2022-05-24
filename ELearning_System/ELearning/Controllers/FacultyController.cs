using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ELearning.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Faculty + "," + UserRoles.Admin)]
    public class FacultyController : ControllerBase
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

        private ILogger<FacultyController> _logger;
        public FacultyController(ILogger<FacultyController> logger, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, SignInManager<ApplicationUser> _signInManager, IConfiguration configuration, ApplicationDbContext _databaseContext)
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
       
        public ActionResult<bool> UploadProjectFile(List<IFormFile> fileName)
        {
            try
            {
                if (fileName == null)
                {
                    _logger.LogError("Check your model");
                    return NotFound("Please enter valid field types");
                }
                else
                {
                    var filestatus = _fileRepository.FileUpload(fileName);
                    bool success = filestatus.Value;
                    if (success)
                    {
                        _logger.LogInformation("File uploaded successfully");
                        return Ok("File uploaded successfully");
                    }
                    else
                    {
                        _logger.LogError("File not uploaded successfully");
                        return NotFound("File not uploaded successfully");

                    }
                }
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
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

        [HttpPost]
        
        public ActionResult<bool> UploadResourceFile(List<IFormFile> fileName)
        {
            try
            {
                if (fileName == null)
                {
                    _logger.LogError("Check your model");
                    return NotFound("Please enter valid field types");
                }
                else
                {
                    var filestatus = _articleRepository.ArticleUpload(fileName);
                    bool success = filestatus.Value;
                    if (success)
                    {
                        _logger.LogInformation("Article uploaded successfully");
                        return Ok("Article uploaded successfully");
                    }
                    else
                    {
                        _logger.LogError("Article not uploaded successfully");
                        return NotFound("Article not uploaded successfully");

                    }
                }
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }

        }
         [HttpDelete]
        
        public ActionResult DeleteResourceFile(string filename)
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
                    var deleteStatus = _articleRepository.DeleteArticle(filename);
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
        public ActionResult FacultyMessage([FromBody] Chat chats)
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
                    var messageStatus = _chatRepository.FacultyMessage(chats);
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
