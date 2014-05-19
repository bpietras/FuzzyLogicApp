using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;


namespace FuzzyLogicWebService.Views.Shared.Custom
{
    public static class CustomHelpers
    {
        public static MvcHtmlString MenuItem(this HtmlHelper htmlHelper, string text, string action)
        {
            var menu = new TagBuilder("div");
            var currentAction = (string)htmlHelper.ViewContext.RouteData.Values["action"];
            if (string.Equals(
                    currentAction,
                    action,
                    StringComparison.CurrentCultureIgnoreCase)
            )
            {
                menu.AddCssClass("highlight");
            }
            menu.SetInnerText(text);
            return MvcHtmlString.Create(menu.ToString());
        }

    }
}