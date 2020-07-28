using Entities;
using System.Collections.Generic;

namespace ChatRepositories
{
    public interface IMessageRepository
    {
        List<Message> MessagesWithFriend(int userId, int friendId);
        void SaveMessage(Message message);
        List<Message> GetUnseenMessages(List<Message> messages);
        void MarkMessagesAsSeen(List<Message> unseenMessages);
        int CheckForNewMessages(int userId, int friendId);
        List<Message> MessagesFromFriendOnly(int userId, int friendId);
        List<MessageInfo> GetLatestMessages(int currentUserId);
    }
}
