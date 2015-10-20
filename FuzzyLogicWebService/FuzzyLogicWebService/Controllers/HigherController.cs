using System;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using FuzzyLogicModel;
using FuzzyLogicWebService.Models;
using FuzzyLogicWebService.Logging;
using FuzzyLogicWebService.Models.Functions;

namespace FuzzyLogicWebService.Controllers
{
    public class HigherController : Controller
    {
        protected static string CURRENT_MODEL_ID = "currentModelId";
        protected static string CURRENT_USER_ID = "currentUserId";
        protected static string CURRENT_VARIABLE_ID = "currentVariableId";

        protected IDatabaseRepository repository { get; set; }
        protected ILogger logger { get; set; }
        
        public HigherController(IDatabaseRepository modelRepository, ILogger appLogger)
        {
            repository = modelRepository;
            logger = appLogger;
        }

        protected string GetCookieValue(string name)
        {
            return Request.Cookies.Get(name).Value;
        }

        protected void EditCookie(string name, Object newValue)
        {
            Request.Cookies.Get(name).Value = newValue.ToString();
        }

        protected void RemoveCurrentUserCookies()
        {
            if (Request.Cookies[CURRENT_USER_ID] != null)
            {
                var c = new HttpCookie(CURRENT_USER_ID);
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
        }

        protected int GetUserCookieValue()
        {
            return int.Parse(GetCookieValue(CURRENT_USER_ID));
        }

        protected void AddOrReplaceUserCookie(Object value)
        {
            AddOrReplaceCookie(CURRENT_USER_ID, value);
        }

        private void AddOrReplaceCookie(string name, Object value)
        {
            HttpCookie cookie = Request.Cookies.Get(name);
            if (cookie != null)
            {
                cookie.Value = value.ToString();
            }
            else
            {
                cookie = new HttpCookie(name, value.ToString());
                cookie.Expires = DateTime.Now.AddHours(2);
                Response.Cookies.Add(cookie);
            }
        }

        private void AddToSession(string name, string value)
        {
            Session[name] = value;
        }

        protected void AddModelIdToSession(int id)
        {
            AddToSession(CURRENT_MODEL_ID, id.ToString());
        }

        protected void AddVariableIdToSession(int id)
        {
            AddToSession(CURRENT_VARIABLE_ID, id.ToString());
        }

        private string GetAttributeFromSession(string name){
            return Session[name].ToString();
        }

        protected int GetCurrentModelId()
        {
            return int.Parse(GetAttributeFromSession(CURRENT_MODEL_ID));
        }

        protected int GetCurrentVariableId()
        {
            return int.Parse(GetAttributeFromSession(CURRENT_VARIABLE_ID));
        }


        protected List<string> ValidateModel(FuzzyModel model)
        {
            return ValidateMemmbershipFunctionsForVariables(model.FuzzyVariables);
        }

        protected List<string> ValidateMemmbershipFunctionsForVariables(IEnumerable<FuzzyVariable> allVariables)
        {
            List<string> validationErrors = new List<string>();
            //funkcje przynależności nie powinny wykraczać poza zakresy zmiennych
            foreach (FuzzyVariable variable in allVariables)
            {
                foreach (MembershipFunction function in variable.MembershipFunctions)
                {
                    validationErrors.AddRange(ValidateMembershipFunction(function, variable.MinValue, variable.MaxValue));
                }
            }
            return validationErrors;
        }

        protected List<string> ValidateMembershipFunctions(List<MembershipFunction> listOfMfs, Decimal minValue, Decimal maxValue)
        {
            List<string> validationErrors = new List<string>();
            foreach (MembershipFunction function in listOfMfs)
                {
                    validationErrors.AddRange(ValidateMembershipFunction(function, minValue, maxValue));
                }
                return validationErrors;
        }

        protected List<string> ValidateMembershipFunction(MembershipFunction function, Decimal minValue, Decimal maxValue)
        {
            List<string> validationErrors = new List<string>();
                if (IsAnyValueOutsideTheRange(function, minValue, maxValue))
                {
                    validationErrors.Add(String.Format("Wartości funkcji {0} wykraczają poza zakres", function.Name));
                }
                if (!IsOrderRight(function))
                {
                    validationErrors.Add(String.Format("Wartości funkcji {0} nie sa ułożone rosnąco", function.Name));
                }
                return validationErrors;
        }



        private bool IsOrderRight(MembershipFunction function)
        {
            if ((function.FirstValue < function.SecondValue) && (function.SecondValue < function.ThirdValue))
            {
                return function.FourthValue != null ? function.ThirdValue < function.FourthValue : true;
            }
            else
            {
                return false;
            }
        }

        private bool IsAnyValueOutsideTheRange(MembershipFunction function, Decimal minValue, Decimal maxValue)
        {
            if ((function.FirstValue < minValue) || (function.SecondValue < minValue) || (function.ThirdValue < minValue) || (function.FourthValue < minValue))
            {
                return true;
            }
            if ((function.FirstValue > maxValue) || (function.SecondValue > maxValue) || (function.ThirdValue > maxValue) || (function.FourthValue > maxValue))
            {
                return true;
            }
            return false;
        }

        protected Boolean ValidateInputValues(List<InputValue> inputValues)
        {
            foreach (InputValue input in inputValues)
            {
                if(input.VariableValue < input.VariableMinValue || input.VariableValue > input.VariableMaxValue)
                {
                    return false;
                }
            }
            return true;
        }
        
    }
}
