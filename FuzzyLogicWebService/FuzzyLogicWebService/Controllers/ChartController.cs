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
using FuzzyLogicWebService.Helpers;

namespace FuzzyLogicWebService.Controllers
{
    public class ChartController : Controller
    {
        public ModelsRepository rep = new ModelsRepository();

        private static IEnumerator<Color> seriesColorsList = new List<Color> { Color.Tomato, Color.Yellow, Color.Green, Color.Goldenrod, Color.LightSeaGreen, Color.Chartreuse,
                                                                Color.DarkBlue, Color.Wheat, Color.Magenta}.GetEnumerator();
        private static IEnumerator<Color> backgroundsColorsList = new List<Color> { Color.LemonChiffon, Color.BlueViolet, Color.Red }.GetEnumerator();


        public ActionResult CreateChart(int variableId)
        {
            FuzzyVariable currentVariable = rep.GetVariableById(variableId);
            Chart chart = new Chart()
            {
                Width = 600,
                Height = 400,
                AlternateText = "Here should be graph of" + currentVariable.Name
            };

            List<Series> allSeries = BuildSeries(currentVariable.MembershipFunctions);
            foreach (var series in allSeries)
            {
                chart.Series.Add(series);
            }
            Title title = BuildChartTitle(currentVariable.Name);
            chart.Titles.Add(title);

            Axis xAxis = new Axis
            {
                Minimum = currentVariable.MinValue,
                Maximum = currentVariable.MaxValue,
            };

            Axis yAxis = new Axis
            {
                Minimum = 0,
                Maximum = 1,
            };

            if(!backgroundsColorsList.MoveNext()){
                backgroundsColorsList.Reset();
                backgroundsColorsList.MoveNext();
            }
            ChartArea area = new ChartArea()
            {
                BackColor = backgroundsColorsList.Current,
                //BackSecondaryColor = Color.LemonChiffon,
                BackGradientStyle = GradientStyle.HorizontalCenter,
                AxisX = xAxis,
                AxisY = yAxis
            };
            chart.ChartAreas.Add(area);


            // Save the chart to a MemoryStream
            var imgStream = new MemoryStream();
            chart.SaveImage(imgStream, ChartImageFormat.Png);
            imgStream.Seek(0, SeekOrigin.Begin);

            // Return the contents of the Stream to the client
            return File(imgStream, "image/png");
        }


        private List<Series> BuildSeries(IEnumerable<MembershipFunction> functions)
        {
            List<Series> seriesList = new List<Series>();
            foreach (MembershipFunction func in functions)
            {
                Series series = new Series();
                series.ChartType = SeriesChartType.Line;
                series.Name = func.Name;
                series.BorderWidth = 5;
                series.Palette = ChartColorPalette.None;
                series.Color = seriesColorsList.Current;
                series.Points.AddXY(func.FirstValue, 0);
                series.Points.AddXY(func.SecondValue, 1);
                if (func.Type == MembershipFunctionType.TriangleFunction)
                {
                    series.Points.AddXY(func.ThirdValue, 0);
                }
                else
                {
                    series.Points.AddXY(func.ThirdValue, 1);
                    series.Points.AddXY(func.FourthValue, 0);
                }
                seriesList.Add(series);
                if(!seriesColorsList.MoveNext()){
                    seriesColorsList.Reset();
                    seriesColorsList.MoveNext();
                };
            }
            return seriesList;

        }

        private Title BuildChartTitle(string titleContent)
        {
            Title title = new Title()
            {
                Docking = Docking.Top,
                Font = new Font("Trebuchet MS", 18.0f, FontStyle.Bold),
                Text = titleContent,
            };
            return title;
        }
    }
}
