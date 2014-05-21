using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using FuzzyLogicWebService.Models;
using FuzzyLogicWebService.FISFiles.DBModel;
using System.Data.SqlClient;

namespace FuzzyLogicWebService.Controllers
{
    public class AccountController : Controller
    {
        private EntityFrameworkContext context = new EntityFrameworkContext();
        private ModelsRepository rep = new ModelsRepository();

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(User model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = rep.Users.Where(x=>x.Name == model.Name && x.UserPassword == model.UserPassword).First();

                if (user!=null)
                {
                    FormsAuthentication.SetAuthCookie(user.Name, true);
                    Session["userId"] = user.UserID;

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Remove("userId");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Users.Add(model);
                    int i = context.SaveChanges();
                    FormsAuthentication.SetAuthCookie(model.Name, false);
                    Session["userId"] = context.Users.Where(x => x.Name == model.Name && x.UserPassword == model.UserPassword).First().UserID;
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "The login is already occupied. Please, provide diffrent user name.");
                }
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");

            }

            return View(model);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
