using System;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace FuzzyLogicWebService.Controllers
{
    public class HigherController : Controller
    {
        protected static string CURRENT_MODEL_ID = "currentModelId";
        protected static string CURRENT_USER_ID = "currentUserId";
        protected static string CURRENT_VARIABLE_ID = "currentVariableId";

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
            if (Request.Cookies[CURRENT_USER_ID] != null)
            {
                var c = new HttpCookie(CURRENT_USER_ID);
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
        }

        protected int GetUserCookieValue()
        {
            return int.Parse(GetCookieValue(CURRENT_USER_ID));
        }

        protected void AddOrReplaceUserCookie(Object value)
        {
            AddOrReplaceCookie(CURRENT_USER_ID, value);
        }

        private void AddOrReplaceCookie(string name, Object value)
        {
            HttpCookie cookie = Request.Cookies.Get(name);
            if (cookie != null)
            {
                cookie.Value = value.ToString();
            }
            else
            {
                cookie = new HttpCookie(name, value.ToString());
                cookie.Expires = DateTime.Now.AddHours(2);
                Response.Cookies.Add(cookie);
            }
        }

        private void AddToSession(string name, string value)
        {
            Session[name] = value;
        }

        protected void AddModelIdToSession(int id)
        {
            AddToSession(CURRENT_MODEL_ID, id.ToString());
        }

        protected void AddVariableIdToSession(int id)
        {
            AddToSession(CURRENT_VARIABLE_ID, id.ToString());
        }

        private string GetAttributeFromSession(string name){
            return Session[name].ToString();
        }

        protected int GetCurrentModelId()
        {
            return int.Parse(GetAttributeFromSession(CURRENT_MODEL_ID));
        }

        protected int GetCurrentVariableId()
        {
            return int.Parse(GetAttributeFromSession(CURRENT_VARIABLE_ID));
        }

        
    }
}
