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
    public class SocialController : Controller
    {
        private IUserRepository userRepository;
        private ISocialRepository socialRepository;
        private IStatusRepository statusRepository;
        private IFriendRepository friendRepository;
        User currentUser = new User();

        public SocialController()
        {
            userRepository = new UserRepository();
            socialRepository = new SocialRepository();
            statusRepository = new StatusRepository();
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

        public ActionResult AllUsers(string searchTerm)
        {
            List<User> allUsers;

            if (searchTerm == null || searchTerm == "")
            {
                allUsers = socialRepository.GetAllUsers(currentUser.UserId);
                ViewBag.Title = "All Users";
            }
            else if (searchTerm == "mutual")
            {
                allUsers = socialRepository.PeopleYouMayKnow(currentUser.UserId);
                ViewBag.Title = "People You May Know";
                ViewBag.IsPeople = true;
            }
            else
            {
                ViewBag.Title = "Search Users";
                ViewBag.SearchTerm = searchTerm;
                allUsers = socialRepository.SearchUsers(currentUser.UserId, searchTerm);
            }

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
                User = currentUser,
                CurrentUserId = currentUser.UserId,
                FriendIds = friendIds,
                OtherUsers = allUsers,
                FriendRequestIds = friendRepository.GetFriendRequestIds(currentUser.UserId),
                SentFriendRequestIds = friendRepository.GetSentFriendRequestIds(currentUser.UserId)
            };

            return View(userInfo);
        }

        public ActionResult ToggleLike(int statusId)
        {
            int likes = statusRepository.ToggleLike(currentUser.UserId, statusId);
            return Content(likes.ToString());
        }

        public ActionResult LikedUsers(int statusId)
        {
            StatusInfo statusInfo = statusRepository.GetStatusInfoById(currentUser.UserId, statusId);

            List<string> friendIds;

            if (currentUser.FriendIds == null || currentUser.FriendIds == "")
            {
                friendIds = null;
            }
            else
            {
                friendIds = currentUser.FriendIds.Split(',').ToList();
            }

            LikedUsersModel likedUsersModel = new LikedUsersModel()
            {
                User = currentUser,
                StatusInfo = statusInfo,
                FriendIds = friendIds,
                FriendRequestIds = friendRepository.GetFriendRequestIds(currentUser.UserId),
                SentFriendRequestIds = friendRepository.GetSentFriendRequestIds(currentUser.UserId)
            };

            return View(likedUsersModel);
        }

        public ActionResult DeleteStatus(int statusId)
        {
            statusRepository.DeleteStatus(currentUser.UserId, statusId);
            return RedirectToAction("Index", "Home");
        }


    }
}