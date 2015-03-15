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
                int index = fuzzyRuleContent.IndexOf(variable.Name) + variable.Name.Length;
                int parenthisesIndex = fuzzyRuleContent.Substring(index).IndexOf(")");
                string membFunctValue = fuzzyRuleContent.Substring(index, parenthisesIndex);
                if (variable.MembershipFunctions.Where(m => m.Name == membFunctValue).First() != null)
                {
                    ruleParserModel.AddVariable(variable.Name, membFunctValue, variable.VariableType);
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
            int ruleLength = (inputsNumber + outputsNumber) * 3;
            char[] ruleContent = new char[ruleLength];
            foreach (VariableValue varVal in rule.InputsValues)
            {
                FuzzyVariable inputVariable = fuzzyModel.FuzzyVariables.First(m => m.Name == varVal.InputName);
                int membIndex = inputVariable.MembershipFunctions.First(m => m.Name == varVal.FunctionName).FunctionIndex;
                ruleContent[inputVariable.VariableIndex * 2] = (char) membIndex;
            }
            ruleContent[inputsNumber * 2] = ',';
            foreach (VariableValue varVal in rule.OutputsValues)
            {
                int outputBase = inputsNumber * 2 + 1;
                FuzzyVariable outputVariable = fuzzyModel.FuzzyVariables.First(m => m.Name == varVal.InputName);
                int membIndex = outputVariable.MembershipFunctions.First(m => m.Name == varVal.FunctionName).FunctionIndex;
                ruleContent[outputBase + outputVariable.VariableIndex * 2] = (char) membIndex;
            }
            ruleContent[allVariables * 2] = '(';
            ruleContent[allVariables * 2 + 1] = '1';
            ruleContent[allVariables * 2 + 2] = ')';
            ruleContent[allVariables * 2 + 4] = ':';
            ruleContent[allVariables * 2 + 6] = rule.InputsValues.ElementAt(0).Connection == "and" ? '1' : '0';

            return new String(ruleContent).Trim();
        }

    }
}