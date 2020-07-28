using ChatRepositories;
using Entities;
using Fakebook.Hubs;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace Fakebook.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private IUserRepository userRepository;
        private ISocialRepository socialRepository;
        private IMessageRepository messageRepository;

        User currentUser = new User();

        public ChatController()
        {
            userRepository = new UserRepository();
            socialRepository = new SocialRepository();
            messageRepository = new MessageRepository();
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (User.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)User.Identity;
                Claim email = identity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email);
                currentUser = userRepository.GetUserByEmail(email.Value);
            }
        }



        // GET: Chat
        public ActionResult Index(int friendId)
        {
            Session["currentFriendId"] = friendId;

            List<Message> messages = messageRepository.MessagesWithFriend(currentUser.UserId, friendId);

            User friend = userRepository.GetUserById(friendId);

            ViewBag.CurrentUserID = currentUser.UserId;
            ViewBag.FriendId = friendId;
            ViewBag.FriendName = friend.FirstName + " " + friend.LastName;
            ViewBag.FriendImage = friend.ImageUrl;

            List<Message> unseenMessages = messageRepository.GetUnseenMessages(messages);
            if (unseenMessages != null && unseenMessages.Count > 0)
                messageRepository.MarkMessagesAsSeen(unseenMessages);

            return View(messages);
        }

        public ActionResult Messages()
        {

            List<MessageInfo> messageInfos = messageRepository.GetLatestMessages(currentUser.UserId);
            ViewBag.CurrentUserId = currentUser.UserId;

            return View(messageInfos);
        }

        public ActionResult CheckForNewMessages()
        {
            var obj = Session["currentFriendId"];
            int currentFriendId;

            if (obj == null)
            {
                Session["currentFriendId"] = 0;
                currentFriendId = 0;
            }
            else
            {
                currentFriendId = (int)obj;
            }


            int amountOfMessages = messageRepository.CheckForNewMessages(currentUser.UserId, currentFriendId);

            if (amountOfMessages <= 0)
                return null;

            return Content(amountOfMessages.ToString());
        }

        public ActionResult ResetFriendId()
        {
            Session["currentFriendId"] = 0;
            return Content("Reset Friend ID");
        }

        public ActionResult Test()
        {
            int test = (int)Session["currentFriendId"];
            return Content(test.ToString());
        }

    }
}