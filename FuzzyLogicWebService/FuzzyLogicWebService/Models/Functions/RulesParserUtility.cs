using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FuzzyLogicModel;
using FuzzyLogicWebService.Models.ViewModels;
using FuzzyLogicWebService.Logging;

namespace FuzzyLogicWebService.Models.Functions
{
    public class RulesParserUtility
    {

        public IEnumerable<FuzzyRule> ParseStringRules(FuzzyModel fuzzyModel)
        {
            return ParseStringRules(fuzzyModel.FuzzyRules, fuzzyModel);
        }

        public IEnumerable<FuzzyRule> ParseStringRules(IEnumerable<FuzzyRule> fuzzyRules,FuzzyModel fuzzyModel)
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
            try
            {
                foreach (FuzzyVariable variable in model.FuzzyVariables)
                {
                    string variableIs = variable.Name + " is ";
                    int variableIndex = fuzzyRuleContent.IndexOf(variableIs);
                    if (variableIndex > 0)
                    {
                        int startIndex = variableIndex + variableIs.Length;
                        int parenthisesIndex = fuzzyRuleContent.Substring(startIndex).IndexOf(")");
                        int spaceIndex = fuzzyRuleContent.Substring(startIndex).IndexOf(" ");
                        int endIndex = ((spaceIndex != -1) && (parenthisesIndex > spaceIndex)) ? spaceIndex : parenthisesIndex;
                        string membFunctValue = fuzzyRuleContent.Substring(startIndex, endIndex);
                        string connection = fuzzyRuleContent.Contains("or") ? "or" : "and";
                        if (variable.MembershipFunctions.Where(m => m.Name == membFunctValue).First() != null)
                        {
                            int membIndex = variable.MembershipFunctions.First(m => m.Name == membFunctValue).FunctionIndex;
                            ruleParserModel.AddVariable(variable.Name, connection, membIndex.ToString(), variable.VariableType);
                        }
                        else
                        {
                            throw new ParsingRuleException(String.Format("Cannot parse rule - {0} membership function does not exist for {1} variable!", membFunctValue.ToUpper(), variable.Name.ToLower()));
                        }
                    }
                    else if (variableIndex == 0)
                    {
                        ruleParserModel.AddVariable(variable.Name, "and", "0", variable.VariableType);
                    }
                    else
                    {
                        throw new ParsingRuleException(String.Format("Cannot parse rule - variable with name: {0} does not exist!", variable.Name.ToLower()));
                    }
                }
                fisRuleContent = CreateFISRuleContent(ruleParserModel, model);

            }catch(ParsingRuleException parsingExc){
                throw parsingExc;
            }catch(Exception){
                throw new ParsingRuleException(String.Format("Cannot parse rule - there is a problem with format! Please check \n {0}", fuzzyRuleContent));
            }

            return fisRuleContent;
        }

        private String CreateFISRuleContent(RuleViewModel rule, FuzzyModel fuzzyModel)
        {
            int inputsNumber = rule.InputsValues.Count;
            int outputsNumber = rule.OutputsValues.Count;
            int allVariables = inputsNumber + outputsNumber;
            int ruleLength = allVariables * 10;
            string[] ruleContent = new string[ruleLength];
            foreach (VariableValue varVal in rule.InputsValues)
            {
                FuzzyVariable inputVariable = fuzzyModel.FuzzyVariables.First(m => m.Name == varVal.VariableName);
                ruleContent[inputVariable.VariableIndex * 2] = varVal.FunctionIndex;
            }
            ruleContent[inputsNumber * 2 - 1] = ",";
            int outputBase = inputsNumber * 2 + 1;
            foreach (VariableValue varVal in rule.OutputsValues)
            {
                FuzzyVariable outputVariable = fuzzyModel.FuzzyVariables.First(m => m.Name == varVal.VariableName);
                ruleContent[outputBase + outputVariable.VariableIndex * 2] = varVal.FunctionIndex;
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