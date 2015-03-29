using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Drawing;

namespace FuzzyLogicWebService.Helpers
{
    public static class ChartExtensions
    {
        public static IEnumerator<Color> SERIES_COLORS_LIST = new List<Color> { Color.Tomato, Color.Yellow, Color.Green, Color.Goldenrod, Color.LightSeaGreen, Color.Chartreuse,
                                                                Color.DarkBlue, Color.Wheat, Color.Magenta, Color.Sienna, Color.Navy, Color.DarkOrchid, Color.DarkOliveGreen,
                                                                Color.SlateGray, Color.OrangeRed, Color.DarkViolet, Color.DarkRed, Color.MediumPurple, Color.Crimson, Color.DarkGreen,
                                                                Color.Red, Color.Indigo, Color.Teal, Color.MediumVioletRed}.GetEnumerator();
        public static IEnumerator<Color> BACKGROUND_COLORS_LIST = new List<Color> { Color.LemonChiffon, Color.BlueViolet, Color.Red, Color.Plum, Color.Lavender, Color.LightBlue, Color.LemonChiffon,
            Color.NavajoWhite, Color.Pink, Color.SkyBlue, Color.Khaki, Color.GreenYellow, Color.Bisque, Color.Gainsboro, Color.MediumAquamarine, Color.PaleGreen, Color.Yellow, Color.LightSteelBlue, Color.SandyBrown }.GetEnumerator();

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