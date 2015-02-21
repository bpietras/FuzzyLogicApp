using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FuzzyLogicWebService.Models.ViewModels
{
    public class VariableValue
    {
        public String InputIndex { get; set; }
        public String ValueIndex { get; set; }
        public String Connection { get; set; }
        public Boolean ShowAnotherConnection { get; set; }
        public List<SelectListItem> ValueItems { get; set; }

        public VariableValue(String inputIndex, Boolean showAnotherConnection, List<SelectListItem> valueItems)
        {
            InputIndex = inputIndex;
            ShowAnotherConnection = showAnotherConnection;
            ValueItems = valueItems;
        }
    }
}