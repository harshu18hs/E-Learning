using DataAccessLayer;
using Microsoft.AspNetCore.Hosting;
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
    public class ChatRepository : IChatRepository
    {
        public ApplicationDbContext _databaseContext;
        public ChatRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;

        }
        public ActionResult<bool> AdminMessage(Chat chats)
        {
            if (chats == null)
            {
                throw new ArgumentNullException(nameof(chats));
            }
            else
            {
                Chat newChat = new Chat()
                {
                    username = chats.username,
                    ResponseMessage = chats.ResponseMessage,
                    FromUserName = chats.FromUserName,
                    ToUserName = chats.ToUserName,
                    Message = chats.Message,
                    ReceivedAt = DateTime.Now,
                    ResponseReceivedAt = DateTime.Now,
                    SenderRole = chats.SenderRole,
                    ReceiverRole = chats.ReceiverRole,
                    Status = chats.Status
                };
                _databaseContext.Chats.Add(newChat);
                _databaseContext.SaveChanges();
                return true;
            }
        }

        public ActionResult<bool> FacultyMessage(Chat chats)
        {
            if (chats == null)
            {
                throw new ArgumentException(nameof(chats));
            }
            else
            {
                Chat newChat = new Chat()
                {
                    username= chats.username,
                    ResponseReceivedAt = DateTime.Now,
                    ResponseMessage = chats.ResponseMessage,
                    FromUserName = chats.FromUserName,
                    ToUserName = chats.ToUserName,
                    Message = chats.Message,
                    ReceivedAt = DateTime.Now,
                    SenderRole = chats.SenderRole,
                    ReceiverRole = chats.ReceiverRole,
                    Status = chats.Status
                };
                _databaseContext.Chats.Add(newChat);
                _databaseContext.SaveChanges();
                return true;
            }
        }
        public ActionResult<List<Chat>> GetChatList(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
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
                return GetChatList;
            }
        }

        public ActionResult<bool> ResponseMessage(int? id,string username,string responseMessage, string status)
        {
            if (id==null ||username == null ||responseMessage == null || status == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                Chat responseChat = _databaseContext.Chats.Find(username);
                if (responseChat == null)
                {
                    return false;
                }
                else
                {
                    responseChat.ResponseMessage = responseMessage;
                    responseChat.ResponseReceivedAt = DateTime.Now;
                    responseChat.Status = status;
                    _databaseContext.SaveChanges();
                    return true;
                }
            }
        }

        public ActionResult<bool> StudentMessage(Chat chats)
        {
            if (chats == null)
            {
                throw new ArgumentException(nameof(chats));
            }
            else
            {
                Chat newChat = new Chat()
                {
                    username = chats.username,
                    FromUserName = chats.FromUserName,
                    ToUserName = chats.ToUserName,
                    Message = chats.Message,
                    ReceivedAt = DateTime.Now,
                    ResponseReceivedAt = DateTime.Now,
                    SenderRole = chats.SenderRole,
                    ResponseMessage = chats.ResponseMessage,
                    ReceiverRole = chats.ReceiverRole,
                    Status = chats.Status
                };
                _databaseContext.Chats.Add(newChat);
                _databaseContext.SaveChanges();
                return true;
            }
        }
    }
}





