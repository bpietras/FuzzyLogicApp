using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FuzzyLogicWebService.Models;
using FuzzyLogicModel;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.IO;

namespace FuzzyLogicWebService.Controllers
{
    public class ChartController : Controller
    {
        public ModelsRepository rep = new ModelsRepository();


        public ActionResult CreateChart(int variableId)
        {
            IEnumerable<MembershipFunction> listOfFunctions = rep.GetMfForVariable(variableId);
            // Create the Chart object and set some properties
            Chart chart = new Chart()
            {
                Width = 600,
                Height = 400
            };

            List<Series> allSeries = CreateSeries(listOfFunctions);
            foreach (var series in allSeries)
            {
                chart.Series.Add(series);
            }
            Title title = new Title() { Text = "This is specific title" };
            chart.Titles.Add(title);

            ChartArea area = new ChartArea()
            {
                BackColor = Color.BlanchedAlmond,
                BackSecondaryColor = Color.Black,
                BackGradientStyle = GradientStyle.TopBottom
            };
            chart.ChartAreas.Add(area);
            // Save the chart to a MemoryStream
            var imgStream = new MemoryStream();
            chart.SaveImage(imgStream, ChartImageFormat.Png);
            imgStream.Seek(0, SeekOrigin.Begin);

            // Return the contents of the Stream to the client
            return File(imgStream, "image/png");
        }
        private List<Series> CreateSeries(IEnumerable<MembershipFunction> functions)
        {
            List<Series> seriesList = new List<Series>();
            foreach (MembershipFunction func in functions)
            {
                Series series = new Series();
                series.ChartType = SeriesChartType.Line;
                series.Points.AddXY(func.FirstValue, 0);
                series.Points.AddXY(func.SecondValue, double.Parse("0,5"));
                series.Points.AddXY(func.ThirdValue, 1);
                series.Points.AddXY(0, func.FirstValue);
                seriesList.Add(series);
            }
            return seriesList;

        }

    }
}
