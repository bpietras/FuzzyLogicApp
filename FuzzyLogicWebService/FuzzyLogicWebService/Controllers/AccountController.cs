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

        public ActionResult LogOn()
        {
            return View();
        }

        public AccountController(IDatabaseRepository modelRepository) : base(modelRepository)
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
                    user = repository.GetuserByLogin(userData.Name);
                }catch(Exception){
                    ModelState.AddModelError("", Resources.Resources.CannotGetUserData);
                    return View(userData);
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
                    ModelState.AddModelError("", Resources.Resources.IncorrectLoginErrorMsg);
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
                    repository.RegisterUser(user);
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

        public bool isUserAuthenticated(User user, string providedPassword)
        {
            String actualpassword = CreatePasswordHash(providedPassword, user.Salt);
            return user.UserPassword.Equals(actualpassword);
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
