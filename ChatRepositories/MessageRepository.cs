using DataLayer;
using Entities;
using System.Collections.Generic;
using System.Linq;

namespace ChatRepositories
{
    public class MessageRepository : IMessageRepository
    {
        private ChatContext context;

        private UserRepository userRepository;
        public MessageRepository(ChatContext db)
        {
            context = db;
            userRepository = new UserRepository();
        }

        public MessageRepository()
        {
            context = new ChatContext();
            userRepository = new UserRepository();
        }

        public List<Message> MessagesWithFriend(int userId, int friendId)
        {
            List<Message> messages = context.Messages
                .Where(m => (m.RecieverId == userId && m.SenderId == friendId) || (m.RecieverId == friendId && m.SenderId == userId)).OrderBy(m => m.SentAt).ToList();

            return messages;
        }

        public List<Message> MessagesFromFriendOnly(int userId, int friendId)
        {
            List<Message> messages = context.Messages
                .Where(m => (m.RecieverId == userId && m.SenderId == friendId)).OrderBy(m => m.SentAt).ToList();

            return messages;
        }

        public void SaveMessage(Message message)
        {
            context.Messages.Add(message);
            context.SaveChanges();
        }

        public List<Message> GetUnseenMessages(List<Message> messages)
        {
            List<Message> unseenMessages = messages.Where(m => m.Seen != true).ToList();

            return unseenMessages;
        }

        public void MarkMessagesAsSeen(List<Message> unseenMessages)
        {
            foreach (Message message in unseenMessages)
            {
                message.Seen = true;
            }

            context.SaveChanges();
        }

        public int CheckForNewMessages(int userId, int friendId)
        {
            if (friendId != 0)
            {
                List<Message> messages = MessagesFromFriendOnly(userId, friendId);
                List<Message> unseenMessages = GetUnseenMessages(messages);

                if (unseenMessages != null && unseenMessages.Count > 0)
                    MarkMessagesAsSeen(unseenMessages);
            }

            int amountOfPeople = context.Messages.Where(m => (m.RecieverId == userId) && !m.Seen).GroupBy(m => m.SenderId).Count();

            return amountOfPeople;
        }

        public List<MessageInfo> GetLatestMessages(int currentUserId)
        {
            //List<Message> messages = context.Messages.Where(m => m.RecieverId == currentUserId).GroupBy(m => m.SenderId).Select(m => m.OrderByDescending(x => x.SentAt).FirstOrDefault()).ToList();

            //Find all messages that involve user, group them by friend and find the latest one to show.
            List<Message> messages = context.Messages.Where(m => (m.RecieverId == currentUserId || m.SenderId == currentUserId))
                .GroupBy(m => m.SenderId == currentUserId ? m.RecieverId : m.SenderId)
                .Select(m => m.OrderByDescending(x => x.SentAt).FirstOrDefault()).ToList();

            List<MessageInfo> messageInfos = new List<MessageInfo>();

            foreach (Message message in messages)
            {
                User friend;

                if (message.SenderId == currentUserId)
                {
                    friend = userRepository.GetUserById(message.RecieverId);
                }
                else
                {
                    friend = userRepository.GetUserById(message.SenderId);
                }

                MessageInfo messageInfo = new MessageInfo(friend, message);

                messageInfos.Add(messageInfo);
            }

            return messageInfos.OrderByDescending(m => m.Message.SentAt).ToList();
        }
    }
}
