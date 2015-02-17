using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace FuzzyLogicWebService.Helpers
{
    public static class ChartExtensions
    {
        public static MvcHtmlString MembershipFunctionChart(this HtmlHelper helper, object variableId)
        {
            string imgUrl = UrlHelper.GenerateUrl(null, "CreateChart", "Chart", new RouteValueDictionary(variableId), helper.RouteCollection, helper.ViewContext.RequestContext, false);

            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", imgUrl);

            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString OutcomeVariableChart(this HtmlHelper helper, object routeData)
        {
            //var routeValues = new RouteValueDictionary();
            
            //    routeValues.Add("variableId", variableId.ToString());
                //routeValues.Add("outputPoint", outputPoint.ToString());
            
            //var data = new Dictionary<string, object>
            //            {
            //                { "variableId", variableId },
            //                { "outcomePoint", outputPoint }
            //            };
            string imgUrl = UrlHelper.GenerateUrl("Default", "CreateOutcomeChart", "Chart", new RouteValueDictionary(routeData), helper.RouteCollection, helper.ViewContext.RequestContext, false);

            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", imgUrl);

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}