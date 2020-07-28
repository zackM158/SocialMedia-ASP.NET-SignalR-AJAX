using ChatRepositories;
using Entities;
using Fakebook.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Fakebook.Controllers
{
    [HandleError]
    public class SecurityController : Controller
    {
        private IUserRepository userRepository;
        User currentUser = new User();

        public SecurityController()
        {
            userRepository = new UserRepository();
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

        // GET: Security
        public ActionResult Login(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string returnUrl, LoginModel model)
        {
            if (!ModelState.IsValid)
                return View();

            bool userCredentialsAreValid = false;

            List<string> userRoles = new List<string>();

            User userDto;

            try
            {
                userDto = userRepository.GetUserByEmail(model.EmailAddress);

                if (userDto != null)
                {
                    string password = Security.Sha256(userDto.Salt, model.Password);

                    userCredentialsAreValid = (password == userDto.Password);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }

            if (!userCredentialsAreValid)
            {
                ModelState.AddModelError("", "User credentials not recognised. Please try again.");
                return View();
            }

            //Get list of claims
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, userDto.EmailAddress));
            claims.Add(new Claim(ClaimTypes.Name, userDto.FirstName));
            claims.Add(new Claim(ClaimTypes.GivenName, userDto.FirstName));
            claims.Add(new Claim(ClaimTypes.Surname, userDto.LastName));
            claims.Add(new Claim(ClaimTypes.Sid, userDto.UserId.ToString()));

            foreach (string role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            //prepare settings for the cookie
            AuthenticationProperties authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddHours(1)
            };

            //Sign out (just in case)
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;

            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            authenticationManager.SignIn(authenticationProperties, claimsIdentity);

            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = "/";

            return Redirect(returnUrl);
        }

        public ActionResult Logout()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;

            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return Redirect("/");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }

            //passwords must match
            if (registerModel.Password != registerModel.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords must match!");
                return View(registerModel);
            }

            try
            {
                string newFileExtension = null;
                bool hasImage = registerModel.PostedFile != null ? true : false;

                if (hasImage)
                {
                    newFileExtension = Path.GetExtension(registerModel.PostedFile.FileName);
                }


                User newUser = new User()
                {
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    EmailAddress = registerModel.EmailAddress,
                    Password = Security.Sha256(out string salt, registerModel.Password),
                    Salt = salt,
                    ImageUrl = "DefaultProfile.jpg"
                };

                int userId = 0;
                userId = userRepository.AddUser(newUser, newFileExtension);

                if (userId == 0)
                {
                    ModelState.AddModelError("", "Email Already In Use");
                    return View(registerModel);
                }


                if (hasImage)
                {
                    string newFileName =
                    "User_ID_" + userId;
                    string newFile = newFileName + newFileExtension;

                    string path = Path.Combine(Server.MapPath("~/Images"),
                        newFile);

                    registerModel.PostedFile.SaveAs(path);
                }

                return RedirectToAction("Login", "Security", new { email = registerModel.EmailAddress });
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "This Email Already Exists");
                return View(registerModel);
            }
            catch
            {
                return View(registerModel);
            }
        }

        public ActionResult DeleteAccount()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;

            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            userRepository.DeleteUser(currentUser.UserId);


            return RedirectToAction("Index", "Home");
        }

        public ActionResult UpdatePassword(string password)
        {
            string email = currentUser.EmailAddress;

            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;

            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            password = Security.Sha256(out string salt, password);
            string UpdatedSalt = salt;
            userRepository.UpdatePassword(currentUser.UserId, UpdatedSalt, password);


            return RedirectToAction("Login", "Security", new { email = email });
        }

        public ActionResult RefreshAccount()
        {
            string email = currentUser.EmailAddress;

            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;

            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Login", "Security", new { email = email });
        }
    }
}
