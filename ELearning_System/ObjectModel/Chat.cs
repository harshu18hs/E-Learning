using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObjectModel
{
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string username { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string Message { get; set; }
        public DateTime ReceivedAt { get; set; }
        public string SenderRole { get; set; }
        public string ReceiverRole { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime ResponseReceivedAt { get; set; }
        public string Status { get; set; }


    }
}
