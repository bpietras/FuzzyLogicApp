using System.Collections.Generic;
using System.Web.Mvc;
using FuzzyLogicModel;
using System.Web.Helpers;
using System.Web.Routing;


namespace FuzzyLogicWebService.Helpers
{
    public static class ChartExtensions
    {
        public static MvcHtmlString MembershipFunctionChart(this HtmlHelper helper, object variableId)
        {
            string imgUrl = UrlHelper.GenerateUrl(null, "CreateChart", "Chart", new RouteValueDictionary(variableId), helper.RouteCollection, helper.ViewContext.RequestContext, false);

            var builder = new TagBuilder("img");
            //builder.MergeAttributes<string, object>(htmlAttributes); // alt and title attributes
            builder.MergeAttribute("src", imgUrl);

            return MvcHtmlString.Create(builder.ToString());
        }


    }
}