using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AI.Fuzzy.Library;
using FuzzyLogicWebService.Helpers;

namespace FuzzyLogicWebService.Views.ModelCalculation
{
    public class FuzzyCalculator
    {

        public double CalculateTheOutput(FuzzyLogicModel.FuzzyModel model, List<FuzzyLogicWebService.Models.InputValue> inputValues)
        {
            MamdaniFuzzySystem mamdaniModel = CreateMamdaniSystem(model);
            Dictionary<FuzzyVariable, double> mamdaniInputValues = new Dictionary<FuzzyVariable, double>();
            foreach (FuzzyLogicWebService.Models.InputValue inputValue in inputValues)
            {
                mamdaniInputValues.Add(mamdaniModel.InputByName(inputValue.VariableName),inputValue.VariableValue);
            }
            Dictionary<FuzzyVariable, double> result = mamdaniModel.Calculate(mamdaniInputValues);
            return result[mamdaniModel.OutputByName("")];
        }

        private MamdaniFuzzySystem CreateMamdaniSystem(FuzzyLogicModel.FuzzyModel currentModel)
        {
            MamdaniFuzzySystem mamdaniModel = new MamdaniFuzzySystem();
            foreach (FuzzyLogicModel.FuzzyVariable variable in currentModel.FuzzyVariables)
            {
                FuzzyVariable mamdaniVariable = new FuzzyVariable(variable.Name, variable.MinValue, variable.MaxValue);
                foreach (FuzzyLogicModel.MembershipFunction function in variable.MembershipFunctions)
                {
                    if (function.Type == MembershipFunctionType.TriangleFunction.ToString())
                    {
                        mamdaniVariable.Terms.Add(new FuzzyTerm(function.Name, new TriangularMembershipFunction(function.FirstValue,
                            function.SecondValue,function.ThirdValue)));
                    }
                    if (function.Type == MembershipFunctionType.TrapezoidFunction.ToString()&& function.FourthValue != null)
                    {
                        mamdaniVariable.Terms.Add(new FuzzyTerm(function.Name, new TrapezoidMembershipFunction(function.FirstValue,
                            function.SecondValue, function.ThirdValue, (double)function.FourthValue)));
                    }
                }
                if (variable.VariableType == 0)
                {
                    mamdaniModel.Input.Add(mamdaniVariable);
                }
                else
                {
                    mamdaniModel.Output.Add(mamdaniVariable);
                }
            }

            foreach (FuzzyLogicModel.FuzzyRule rule in currentModel.FuzzyRules)
            {
                MamdaniFuzzyRule mamdaniRule = mamdaniModel.ParseRule(rule.RuleContent);
                mamdaniModel.Rules.Add(mamdaniRule);
            }
            return mamdaniModel;
        }
    }
}