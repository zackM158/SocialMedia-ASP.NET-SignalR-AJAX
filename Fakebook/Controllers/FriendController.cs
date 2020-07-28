using ChatRepositories;
using Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using System.Web;
using Fakebook.Models;

using System.Web.Services;
using System.Web.Script.Services;

namespace Fakebook.Controllers
{
    [Authorize, HandleError]
    public class FriendController : Controller
    {
        private IUserRepository userRepository;
        private IFriendRepository friendRepository;

        User currentUser = new User();

        public FriendController()
        {
            userRepository = new UserRepository();
            friendRepository = new FriendRepository();
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            // Now you can access the HttpContext and User
            if (User.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)User.Identity;
                Claim email = identity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email);
                currentUser = userRepository.GetUserByEmail(email.Value);
            }
        }

        public ActionResult AllFriends(int id = 0)
        {
            if (id == 0)
                id = currentUser.UserId;

            User user = userRepository.GetUserById(id);

            List<User> friends = new List<User>();
            List<FriendRequest> friendRequests = new List<FriendRequest>();
            List<User> friendRequestUsers = null;
            List<User> mutualFriends = null;

            if (id == currentUser.UserId)
            {
                friendRequests = friendRepository.GetAllFriendRequests(id);
                friendRequestUsers = friendRepository.GetAllFriendRequestUsers(friendRequests);
                MarkRequestsAsSeen(friendRequests);
            }
            else
            {
                mutualFriends = friendRepository.GetMutualFriends(currentUser.UserId, id);
            }

            List<User> currentFriends = friendRepository.GetAllFriends(id);

            if (currentFriends != null)
                friends = currentFriends.OrderBy(u => u.FirstName).ToList();

            List<string> friendIds;

            if (currentUser.FriendIds == null || currentUser.FriendIds == "")
            {
                friendIds = null;
            }
            else
            {
                friendIds = currentUser.FriendIds.Split(',').ToList();
            }

            UserInfoModel userInfo = new UserInfoModel()
            {
                User = user,
                CurrentUserId = currentUser.UserId,
                FriendIds = friendIds,
                OtherUsers = friends,
                FriendRequestIds = friendRepository.GetFriendRequestIds(currentUser.UserId),
                SentFriendRequestIds = friendRepository.GetSentFriendRequestIds(currentUser.UserId),
                FriendRequests = friendRequestUsers,
                MutualFriends = mutualFriends
            };

            return View(userInfo);
        }

        public ActionResult RejectRequest(int id)
        {
            friendRepository.RemoveFriendRequest(currentUser.UserId, id);
            return Content("Add Friend");
        }

        public ActionResult ToggleFriend(int id)
        {
            string buttonMessage = friendRepository.ToggleFriend(currentUser.UserId, id);
            return Content(buttonMessage);
        }

        private void MarkRequestsAsSeen(List<FriendRequest> friendRequests)
        {
            foreach (FriendRequest friendRequest in friendRequests.Where(f => f.Seen == false))
            {
                friendRepository.MarkRequestAsSeen(friendRequest);
            }
        }

        public ActionResult CheckForFrendRequests()
        {
            bool newRequest = false;

            newRequest = friendRepository.CheckForNewFriendRequest(currentUser.UserId);

            if (!newRequest)
                return null;

            return Content("New Request");
        }

        public ActionResult ToggleFriendFromProfile(int friendId)
        {
            friendRepository.ToggleFriend(currentUser.UserId, friendId);
            return RedirectToAction("Index", "Account", new { id = friendId });
        }
    }
}