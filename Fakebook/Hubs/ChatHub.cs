using ChatRepositories;
using Entities;
using Microsoft.AspNet.SignalR;
using System;

namespace Fakebook.Hubs
{
    public class ChatHub : Hub
    {
        private IMessageRepository messageRepository;
        private IUserRepository userRepository;
        public int CurrentId { get; set; }

        public ChatHub()
        {
            messageRepository = new MessageRepository();
            userRepository = new UserRepository();

        }

        public void Send(string senderIdString, string recieverIdString, string text)
        {

            int senderId = int.Parse(senderIdString);
            int recieverId = int.Parse(recieverIdString);

            string groupName = GetGroupName(senderId, recieverId);
            string senderName = userRepository.GetUserById(senderId).FirstName;
            DateTime sentAt = DateTime.Now;

            Message message = new Message()
            {
                SenderId = senderId,
                RecieverId = recieverId,
                Text = text,
                SentAt = sentAt,
                SenderName = senderName,
                Seen = false
            };

            messageRepository.SaveMessage(message);

            Clients.Group(groupName).addNewMessageToPage(senderIdString, text, sentAt.ToShortTimeString());
        }

        public void AddToGroup(int id1, int id2)
        {
            string groupName = GetGroupName(id1, id2);
            Groups.Add(Context.ConnectionId, groupName);
        }

        public string GetGroupName(int id1, int id2)
        {
            string groupName = "";

            groupName = id1 <= id2 ? string.Format($"{id1.ToString()} And {id2.ToString()}") : string.Format($"{id2.ToString()} And {id1.ToString()}");

            return groupName;
        }

    }
}