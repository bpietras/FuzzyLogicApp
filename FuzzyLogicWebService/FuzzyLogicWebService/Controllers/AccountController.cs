using System;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Web.Security;
using FuzzyLogicModel;
using FuzzyLogicWebService.Models;
using FuzzyLogicWebService.Logging;

namespace FuzzyLogicWebService.Controllers
{
    public class AccountController : HigherController
    {

        public ActionResult LogOn()
        {
            return View();
        }

        public AccountController(IDatabaseRepository modelRepository, ILogger appLogger)
            : base(modelRepository, appLogger)
        {            
        }

        [HttpPost]
        public ActionResult LogOn(User userData, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                try
                {
                    user = repository.GetUserByLogin(userData.Name);
                }catch(Exception){
                    logger.Error(String.Format(Resources.Resources.CannotGetUserData + " for user \"{0}\"", userData.Name));
                    ModelState.AddModelError("", Resources.Resources.CannotGetUserData);
                    return View(userData);
                }

                if (user!=null && isUserAuthenticated(user, userData.UserPassword))
                {
                    logger.Debug(String.Format("User \"{0}\" just log in.", user.Name));
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
                    logger.Debug(String.Format(Resources.Resources.IncorrectLoginErrorMsg + " for user \"{0}\"", user.Name));
                    ModelState.AddModelError("", Resources.Resources.IncorrectLoginErrorMsg);
                }
            }

            return View(userData);
        }

        public ActionResult LogOff(string userName)
        {
            FormsAuthentication.SignOut();
            RemoveCurrentUserCookies();
            logger.Debug(String.Format("User \"{0}\" just log out.", userName));
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
                    repository.RegisterUser(user);
                    AddOrReplaceUserCookie(user.UserID);
                    FormsAuthentication.SetAuthCookie(user.Name, false);
                    logger.Debug("New user just rergistered: " + user.Name);
                    String justRegisterMsg = "Welcome!! I can see you have just registered. Please, take a look what we have,  " + user.Name;
                    return RedirectToAction("Index", "Home", new { messageToUser = justRegisterMsg });
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

        public bool isUserAuthenticated(User user, string providedPassword)
        {
            String actualHashedPassword = CreatePasswordHash(providedPassword, user.Salt);
            return user.UserPassword.Equals(actualHashedPassword);
        }

        private static string CreatePasswordHash(string password, string salt)
        {
            string passwordSalt = String.Concat(password, salt);
            string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(passwordSalt, "sha1");
            return hashedPwd;
        }

        private static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] byteArr = new byte[64];
            rng.GetBytes(byteArr);

            return Convert.ToBase64String(byteArr);
        }

    }
}
