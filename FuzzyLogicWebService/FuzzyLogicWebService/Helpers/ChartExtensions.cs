using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace FuzzyLogicWebService.Helpers
{
    public static class ChartExtensions
    {
        public static MvcHtmlString VariableChart(this HtmlHelper helper, object variableId, object outcomePoint)
        {
            var data = new Dictionary<string, object>
                {
                    { "variableId", variableId },
                    { "outcomePoint", outcomePoint }
                };

            string imgUrl = UrlHelper.GenerateUrl(null, "CreateChart", "Chart", new RouteValueDictionary(data), helper.RouteCollection, helper.ViewContext.RequestContext, false);

            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", imgUrl);

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}