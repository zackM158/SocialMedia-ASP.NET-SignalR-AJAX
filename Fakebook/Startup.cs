using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Security.Claims;
using System.Web.Helpers;

[assembly: OwinStartupAttribute(typeof(Fakebook.Startup))]
namespace Fakebook
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Email;

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Security/Login"),
                CookieSecure = CookieSecureOption.SameAsRequest
            });
        }
    }
}
