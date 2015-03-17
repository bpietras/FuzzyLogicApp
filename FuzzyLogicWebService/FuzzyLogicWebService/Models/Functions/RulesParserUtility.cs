using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FuzzyLogicModel;
using FuzzyLogicWebService.Models.ViewModels;

namespace FuzzyLogicWebService.Models.Functions
{
    public class RulesParserUtility
    {

        public IEnumerable<FuzzyRule> ParseStringRules(IEnumerable<FuzzyRule> fuzzyRules, FuzzyModel fuzzyModel)
        {
            foreach (FuzzyRule rule in fuzzyRules)
            {
                rule.FISRuleContent = CreateFISRuleContent(rule.StringRuleContent, fuzzyModel);
            }
            
            return fuzzyRules;
        }

        private string CreateFISRuleContent(string fuzzyRuleContent, FuzzyModel model)
        {
            string fisRuleContent = null;
            RuleViewModel ruleParserModel = new RuleViewModel();
            foreach (FuzzyVariable variable in model.FuzzyVariables)
            {
                string variableIs = variable.Name + " is ";
                int startIndex = fuzzyRuleContent.IndexOf(variableIs) + variableIs.Length;
                int parenthisesIndex = fuzzyRuleContent.Substring(startIndex).IndexOf(")");
                int spaceIndex = fuzzyRuleContent.Substring(startIndex).IndexOf(" ");
                int endIndex = parenthisesIndex > spaceIndex ? spaceIndex : parenthisesIndex;
                string membFunctValue = fuzzyRuleContent.Substring(startIndex, endIndex);
                string connection = fuzzyRuleContent.Contains("and") ? "and" : "or";
                if (variable.MembershipFunctions.Where(m => m.Name == membFunctValue).First() != null)
                {
                    ruleParserModel.AddVariable(variable.Name, connection, membFunctValue, variable.VariableType);
                }
                else
                {
                    throw new Exception(String.Format("Cannot parse rule - {0} membership function does not exist for {1} variable!", membFunctValue.ToUpper(), variable.Name.ToLower()));
                }
            }
            fisRuleContent = CreateFISRuleContent(ruleParserModel, model);
            return fisRuleContent;
        }

        private String CreateFISRuleContent(RuleViewModel rule, FuzzyModel fuzzyModel)
        {
            int inputsNumber = rule.InputsValues.Count;
            int outputsNumber = rule.OutputsValues.Count;
            int allVariables = inputsNumber + outputsNumber;
            int ruleLength = (inputsNumber + outputsNumber) * 10;
            string[] ruleContent = new string[ruleLength];
            foreach (VariableValue varVal in rule.InputsValues)
            {
                FuzzyVariable inputVariable = fuzzyModel.FuzzyVariables.First(m => m.Name == varVal.VariableName);
                int membIndex = inputVariable.MembershipFunctions.First(m => m.Name == varVal.FunctionName).FunctionIndex;
                ruleContent[inputVariable.VariableIndex * 2] = membIndex.ToString();
            }
            ruleContent[inputsNumber * 2 - 1] = ",";
            foreach (VariableValue varVal in rule.OutputsValues)
            {
                int outputBase = inputsNumber * 2 + 1;
                FuzzyVariable outputVariable = fuzzyModel.FuzzyVariables.First(m => m.Name == varVal.VariableName);
                int membIndex = outputVariable.MembershipFunctions.First(m => m.Name == varVal.FunctionName).FunctionIndex;
                ruleContent[outputBase + outputVariable.VariableIndex * 2] = membIndex.ToString();
            }
            ruleContent[allVariables * 2 + 1] = "(";
            ruleContent[allVariables * 2 + 2] = "1";
            ruleContent[allVariables * 2 + 3] = ")";
            ruleContent[allVariables * 2 + 5] = ":";
            ruleContent[allVariables * 2 + 7] = rule.InputsValues.ElementAt(0).Connection.Trim().Equals("and", StringComparison.OrdinalIgnoreCase) ? "1" : "2";
            string fisRuleString = "";
            foreach (string numberInRule in ruleContent)
            {
                if (numberInRule != null)
                {
                    fisRuleString += numberInRule;
                }
                else
                {
                    fisRuleString += " ";
                }
            }
            return fisRuleString.Trim();
        }

    }
}