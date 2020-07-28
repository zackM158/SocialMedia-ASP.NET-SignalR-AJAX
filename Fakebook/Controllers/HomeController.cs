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
using System;
using System.Web.UI.WebControls;

namespace Fakebook.Controllers
{
    [Authorize, HandleError]
    public class HomeController : Controller
    {
        private IUserRepository userRepository;
        private IStatusRepository statusRepository;
        User currentUser;

        public HomeController()
        {
            userRepository = new UserRepository();
            statusRepository = new StatusRepository();
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

        public ActionResult Index()
        {
            var statuses = statusRepository.AllFriendStatuses(currentUser.UserId);
            StatusModel statusModel = new StatusModel()
            {
                User = currentUser,
                AllStatuses = statuses
            };

            HashSet<int> currentStatusIds = statuses.Select(s => s.Status.StatusId).ToHashSet();
            Session["currentStatusIds"] = currentStatusIds;

            ViewBag.StatusClass = "HomeStatus";

            return View(statusModel);
        }

        [HttpPost]
        public ActionResult Index(StatusModel statusModel)
        {
            if (statusModel.NewStatus.Text != null && statusModel.NewStatus.Text.Trim() != "")
            {
                statusModel.NewStatus.SentAt = DateTime.Now;
                statusModel.NewStatus.UserId = currentUser.UserId;

                statusRepository.SaveStatus(statusModel.NewStatus);
            }

            return RedirectToAction("Index");
        }

        public ActionResult GetNewStatuses()
        {
            var obj = Session["currentStatusIds"];
            HashSet<int> currentStatusIds;

            if (obj == null)
            {
                currentStatusIds = new HashSet<int>();
                Session["currentStatusIds"] = currentStatusIds;
            }
            else
            {
                currentStatusIds = (HashSet<int>)obj;
            }

            List<StatusInfo> statusInfos = statusRepository.GetNewStatuses(currentUser.UserId, currentStatusIds);

            if (statusInfos == null || statusInfos.Count == 0)
            {
                return null;
            }

            return PartialView("_NewStatuses", statusInfos);
        }
    }
}