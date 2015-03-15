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
            FISSystem fisModel = new FISSystem();
            fisModel.Name = dbModel.Name;
            fisModel.InputsNumber = dbModel.InputsNumber;
            fisModel.OutputsNumber = dbModel.OutputsNumber;
            fisModel.RulesNumber = dbModel.RulesNumber;
            return fisModel;
        }

        private FISVariable mapFuzzyVariableToFISVariable(FuzzyVariable dbVariable)
        {
            FISVariable fisVariable = new FISVariable();
            fisVariable.Name = dbVariable.Name;
            fisVariable.Type = dbVariable.VariableType == 0? "Input":"Output";
            fisVariable.MinValue = dbVariable.MinValue;
            fisVariable.MaxValue = dbVariable.MaxValue;
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
                listOfFisFunctions.Add(fisFunc);
            }
            return listOfFisFunctions;
        }

        
        
    }
}