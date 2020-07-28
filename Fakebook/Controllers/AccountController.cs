using ChatRepositories;
using Entities;
using Fakebook.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace Fakebook.Controllers
{
    [Authorize, HandleError]
    public class AccountController : Controller
    {
        private UserRepository userRepository;
        private StatusRepository statusRepository;
        private FriendRepository friendRepository;
        User currentUser = new User();

        public AccountController()
        {
            userRepository = new UserRepository();
            statusRepository = new StatusRepository();
            friendRepository = new FriendRepository();
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

        // GET: User
        public ActionResult Index(int id = 0)
        {
            User user;

            if (id == 0)
            {
                user = currentUser;
            }
            else
            {
                user = userRepository.GetUserById(id);
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

            List<int> friendRequestIds = friendRepository.GetFriendRequestIds(currentUser.UserId);
            List<int> sentFriendRequestIds = friendRepository.GetSentFriendRequestIds(currentUser.UserId);


            string friendButtonText;
            string friendButtonClass = "UserButtonProfile";
            bool requested = false;
            if (friendIds != null && friendIds.Contains(user.UserId.ToString()))
            {
                friendButtonText = "Remove Friend";
            }
            else if (sentFriendRequestIds != null && sentFriendRequestIds.Contains(user.UserId))
            {
                friendButtonText = "Cancel Request";
                friendButtonClass = "RemoveButtonProfile";
            }
            else if (friendRequestIds != null && friendRequestIds.Contains(user.UserId))
            {
                friendButtonText = "Accept";
                requested = true;
            }
            else
            {
                friendButtonText = "Add Friend";
                friendButtonClass = "AcceptButtonProfile";
            }

            UserPageModel userPage = new UserPageModel()
            {
                User = user,
                Statuses = statusRepository.UserStatuses(currentUser.UserId, user.UserId),
                Friends = friendRepository.GetAllFriends(user.UserId),
                CurrentUserId = currentUser.UserId,
                FriendIds = friendIds,
                FriendButtonText = friendButtonText,
                Requested = requested,
                FriendButtonClass = friendButtonClass,
            };

            ViewBag.StatusClass = "ProfileStatus";
            return View(userPage);
        }

        public ActionResult Edit()
        {
            User user = userRepository.GetUserById(currentUser.UserId);

            UpdateUserModel updateUserModel = new UpdateUserModel()
            {
                User = user
            };

            return View(updateUserModel);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(UpdateUserModel updateUserModel)
        {
            try
            {
                User oldUser = userRepository.GetUserById(currentUser.UserId);

                bool changedInfo = oldUser.FirstName != updateUserModel.User.FirstName || oldUser.LastName != updateUserModel.User.LastName || oldUser.EmailAddress != updateUserModel.User.EmailAddress;

                if (changedInfo)
                {
                    updateUserModel.User.Password = oldUser.Password;
                    updateUserModel.User.Salt = oldUser.Salt;

                    string updateMessage = userRepository.UpdateUser(updateUserModel.User);

                    if (updateMessage == "Email Taken")
                    {
                        ModelState.AddModelError("", "Email Already In Use");
                        return View();
                    }

                }

                if (updateUserModel.PostedFile != null)
                {
                    string newFileExtension = Path.GetExtension(updateUserModel.PostedFile.FileName);
                    string newFileName = "User_ID_" + updateUserModel.User.UserId;
                    string newFile = newFileName + newFileExtension;
                    string path = Path.Combine(Server.MapPath("~/Images"),
                        newFile);
                    updateUserModel.PostedFile.SaveAs(path);
                    userRepository.ChangeImageURL(updateUserModel.User, newFileExtension);
                }

                if (changedInfo)
                    return RedirectToAction("RefreshAccount", "Security");
                else
                    return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete()
        {
            User userToDelete = userRepository.GetUserById(currentUser.UserId);
            return View(userToDelete);
        }

        // POST: Manufacturer/Delete/5
        [HttpPost]
        public ActionResult Delete(User user)
        {
            try
            {
                return RedirectToAction("DeleteAccount", "Security");
            }
            catch
            {
                return View(user);
            }
        }

        public ActionResult UpdatePassword()
        {
            UpdatePasswordModel updatePasswordModel = new UpdatePasswordModel();
            return View(updatePasswordModel);
        }

        // POST: Manufacturer/Delete/5
        [HttpPost]
        public ActionResult UpdatePassword(UpdatePasswordModel passwordModel)
        {
            try
            {
                if (passwordModel.Password != passwordModel.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Passwords must match!");
                    return View(passwordModel);
                }

                return RedirectToAction("UpdatePassword", "Security", new { password = passwordModel.Password });
            }
            catch
            {
                return View(passwordModel);
            }
        }
    }
}