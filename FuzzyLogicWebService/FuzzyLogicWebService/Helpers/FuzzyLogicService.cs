using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace FuzzyLogicWebService.Helpers
{
    public static class FuzzyLogicService
    {
        public static string TriangleFunction = "Triangle";
        public static string TrapezoidFunction = "Trapezoid";

        public static string InputVariable = "Input";
        public static string OutputVariable = "Output";

        public static Dictionary<int, string> CreateModelCheckpointsList = new Dictionary<int, string>
                {
                    { 0, "Missing inputs" },
                    { 1, "Missing outputs" },
                    { 2, "Missing membership functions" },
                    { 3, "Missing rules" }
                };

        public static Dictionary<int, string> CreateModelActionList = new Dictionary<int, string>
                {
                    { 0, "AddInputVariables" },
                    { 1, "AddOutputVariables" },
                    { 2, "AddFunctionsForVariables" },
                    { 3, "AddRulesToModel" }
                };
    }
}