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

        public ActionResult CreateChart(int variableId, double? outcomePoint)
        {
            FuzzyVariable currentVariable = rep.GetVariableById(variableId);
            Chart chart = new Chart()
            {
                Width = 400,
                Height = 270,
                AlternateText = @Resources.Resources.MissingGraphErrMsg + currentVariable.Name
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
            if (!ChartExtensions.BACKGROUND_COLORS_LIST.MoveNext())
            {
                ChartExtensions.BACKGROUND_COLORS_LIST.Reset();
            }
            ChartArea area = new ChartArea()
            {
                BackColor = ChartExtensions.BACKGROUND_COLORS_LIST.Current,
                BackGradientStyle = GradientStyle.HorizontalCenter,
                AxisX = xAxis,
                AxisY = yAxis
            };
            chart.ChartAreas.Add(area);
            
            if (outcomePoint != null)
            {
                StripLine outcomeLine = new StripLine();
                outcomeLine.BorderColor = Color.Black;
                outcomeLine.BackColor = Color.Black;
                outcomeLine.Interval = currentVariable.MaxValue;
                outcomeLine.IntervalOffset = (double)outcomePoint - currentVariable.MinValue;
                outcomeLine.StripWidth = 0.1;
                outcomeLine.Font = new Font("Trebuchet MS", 10.0f);
                outcomeLine.Text = String.Format("{0} = {1}", currentVariable.Name, outcomePoint);
               chart.ChartAreas[0].AxisX.StripLines.Add(outcomeLine); ;
            }

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
                if (!ChartExtensions.SERIES_COLORS_LIST.MoveNext())
                {
                    ChartExtensions.SERIES_COLORS_LIST.Reset();
                }; 
                Series series = new Series();
                series.ChartType = SeriesChartType.Line;
                series.Name = func.Name;
                series.BorderWidth = 5;
                series.Palette = ChartColorPalette.None;
                series.Color = ChartExtensions.SERIES_COLORS_LIST.Current;
                series.Points.AddXY(func.FirstValue, 0);
                series.Points.AddXY(func.SecondValue, 1);
                if (func.Type == FuzzyLogicService.TriangleFunction)
                {
                    series.Points.AddXY(func.ThirdValue, 0);
                }
                else
                {
                    series.Points.AddXY(func.ThirdValue, 1);
                    series.Points.AddXY(func.FourthValue, 0);
                }
                seriesList.Add(series);
                
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
