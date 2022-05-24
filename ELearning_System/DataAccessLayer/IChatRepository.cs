using Microsoft.AspNetCore.Mvc;
using ObjectModel;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface IChatRepository
    {
        public ActionResult<bool> AdminMessage(Chat chats);
        public ActionResult<bool> FacultyMessage(Chat chats);
        public ActionResult<List<Chat>> GetChatList(string username);
        public ActionResult<bool> ResponseMessage(int? id,string username,string responseMessage, string status);
        public ActionResult<bool> StudentMessage(Chat chats);
    }
}
