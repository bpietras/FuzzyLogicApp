using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FuzzyLogicWebService.FISFiles.FISModel;
using FuzzyLogicModel;
using FuzzyLogicWebService.Models.ViewModels;

namespace FuzzyLogicWebService.Models.Functions
{
    public class FisFunctionUtils
    {

        private FISSystem mapFuzzyModelToFISSystem(FuzzyModel dbModel)
        {
            FISSystem fisSystem = new FISSystem();
            fisSystem.Name = dbModel.Name;
            fisSystem.InputsNumber = dbModel.InputsNumber;
            fisSystem.OutputsNumber = dbModel.OutputsNumber;
            fisSystem.RulesNumber = dbModel.RulesNumber;
            return fisSystem;
        }

        private FISVariable mapFuzzyVariableToFISVariable(FuzzyVariable dbVariable)
        {
            FISVariable fisVariable = new FISVariable();
            fisVariable.Name = dbVariable.Name;
            fisVariable.Type = dbVariable.VariableType;
            fisVariable.MinValue = dbVariable.MinValue;
            fisVariable.MaxValue = dbVariable.MaxValue;
            fisVariable.Index = dbVariable.VariableIndex + 1;
            fisVariable.NumberOfMembFunc = dbVariable.NumberOfMembFunc;
            fisVariable.ListOfMF = mapFuzzyMembFuncToFISMembFunc(dbVariable.MembershipFunctions.AsEnumerable());
            return fisVariable;
        }

        private List<FISMembershipFunction> mapFuzzyMembFuncToFISMembFunc(IEnumerable<MembershipFunction> listOfDbFunctions)
        {
            List<FISMembershipFunction> listOfFisFunctions = new List<FISMembershipFunction>();
            foreach (MembershipFunction dbFunc in listOfDbFunctions)
            {
                FISMembershipFunction fisFunc = new FISMembershipFunction();
                fisFunc.Name = dbFunc.Name;
                fisFunc.Type = dbFunc.Type;
                List<Double> listOfCusps = new List<Double>();
                listOfCusps.Add(dbFunc.FirstValue);
                listOfCusps.Add(dbFunc.SecondValue);
                listOfCusps.Add(dbFunc.ThirdValue);
                if (dbFunc.FourthValue != null)
                {
                    listOfCusps.Add((Double) dbFunc.FourthValue);
                }
                fisFunc.ListOfCusps = listOfCusps;
                fisFunc.Index = dbFunc.FunctionIndex;
                listOfFisFunctions.Add(fisFunc);
            }
            return listOfFisFunctions;
        }

        private List<string> mapFuzzyRulesToFisRules(List<FuzzyRule> fuzzyRules)
        {
            List<string> fisRules = new List<string>();
            foreach (FuzzyRule fuzzyRule in fuzzyRules)
            {
                fisRules.Add(fuzzyRule.FISRuleContent);
            }
            return fisRules;
        }

        public FISFileContent mapFuzzyModelToFisFileContent(FuzzyModel fuzzyModel)
        {
            FISFileContent fileContent = new FISFileContent();
            fileContent.SystemProperties = mapFuzzyModelToFISSystem(fuzzyModel);
            List<FISVariable> inputFisVars = new List<FISVariable>();
            List<FISVariable> outputFisVars = new List<FISVariable>();
            foreach(FuzzyVariable fuzzyVar in fuzzyModel.FuzzyVariables){
                FISVariable variable = mapFuzzyVariableToFISVariable(fuzzyVar);
                if (variable.Type.Equals("Input"))
                {
                    inputFisVars.Add(variable);
                }else
                {
                    outputFisVars.Add(variable);
                }
            }
            fileContent.InputVariables = inputFisVars;
            fileContent.OutputVariables = outputFisVars;
            fileContent.ListOfRules = mapFuzzyRulesToFisRules(fuzzyModel.FuzzyRules.ToList());
            return fileContent;
        }
        
    }
}