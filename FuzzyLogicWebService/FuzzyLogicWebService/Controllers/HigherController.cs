using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FuzzyLogicWebService.Controllers
{
    public class HigherController : Controller
    {
        protected static string CURRENT_MODEL_ID = "currentModelId";
        protected static string CURRENT_USER_ID = "currentUserId";
        protected static string CURRENT_VARIABLE_ID = "currentVariableId";

        protected void CreateCookie(string name, Object value)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Value = value.ToString();
            cookie.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Add(cookie);
        }

        protected string GetCookieValue(string name)
        {
            return Request.Cookies.Get(name).Value;
        }

        protected void EditCookie(string name, Object newValue)
        {
            Request.Cookies.Get(name).Value = newValue.ToString();
        }

        protected void RemoveCurrentUserCookies()
        {
            Response.Cookies.Remove(CURRENT_USER_ID);
            Response.Cookies.Remove(CURRENT_MODEL_ID);
            Response.Cookies.Remove(CURRENT_VARIABLE_ID);
        }
    }
}
