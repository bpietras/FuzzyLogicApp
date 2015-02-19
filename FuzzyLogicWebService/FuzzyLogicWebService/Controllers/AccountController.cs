using System;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Web.Security;
using FuzzyLogicModel;
using FuzzyLogicWebService.Models;

namespace FuzzyLogicWebService.Controllers
{
    public class AccountController : HigherController
    {
        private ModelsRepository context = new ModelsRepository();

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(User userData, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                try
                {
                    user = context.GetuserByLogin(userData.Name);
                }catch(Exception){
                    ViewBag.ShouldRegister = "Podany użytkownik nie istnieje. ";
                    return View("Register");
                }

                if (user!=null && isUserAuthenticated(user, userData.UserPassword))
                {
                    FormsAuthentication.SetAuthCookie(user.Name, true);
                    AddOrReplaceUserCookie(user.UserID);

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
                    ModelState.AddModelError("", Resources.Resources.RegisterErrorMessage);
                }
            }

            return View(userData);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            RemoveCurrentUserCookies();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel newUser)
        {
            if (ModelState.IsValid && newUser.Password.Equals(newUser.ConfirmPassword))
            {
                try
                {
                    User user = new User();
                    user.Name = newUser.UserName;
                    user.Email = newUser.Email;
                    user.Salt = CreateSalt();
                    user.UserPassword = CreatePasswordHash(newUser.Password, user.Salt);
                    context.RegisterUser(user);
                    AddOrReplaceUserCookie(user.UserID);
                    FormsAuthentication.SetAuthCookie(user.Name, false);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", Resources.Resources.LoginOccupiedErrorMessage);
                }
            }
            else
            {
                ModelState.AddModelError("", Resources.Resources.RegisterErrorMessage);

            }

            return View(newUser);
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

        public bool isUserAuthenticated(User user, string providedPassword)
        {
            String actualpassword = CreatePasswordHash(providedPassword, user.Salt);
            return user.UserPassword.Equals(actualpassword);
        }

        private static string CreatePasswordHash(string password, string salt)
        {
            string passwrodSalt = String.Concat(password, salt);
            string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(passwrodSalt, "sha1");
            return hashedPwd;
        }

        private static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] byteArr = new byte[64];
            rng.GetBytes(byteArr);

            return Convert.ToBase64String(byteArr);
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
