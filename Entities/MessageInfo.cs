using System.Collections.Generic;
using System.Linq;

namespace Entities
{
    public class MessageInfo
    {
        public User User { get; set; }
        public Message Message { get; set; }

        public MessageInfo(User user, Message message)
        {
            User = user;
            Message = message;
        }
    }
}