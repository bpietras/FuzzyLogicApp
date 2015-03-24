using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace FuzzyLogicWebService.Helpers
{
    public static class MembershipFunctionType
    {
        public static string TriangleFunction = "Triangle";
        public static string TrapezoidFunction = "Trapezoid";

        public static Dictionary<int, string> data = new Dictionary<int, string>
                {
                    { 0, "Missing inputs" },
                    { 1, "Missing outputs" },
                    { 2, "Missing membership functions" },
                    { 3, "Missing rules" },
                    { 4, "Created" }
                };
    }
}