using System.Collections.Generic;
using System.Linq;
using AI.Fuzzy.Library;
using FuzzyLogicWebService.Helpers;
using System;

namespace FuzzyLogicWebService.Models.Functions
{
    public class FuzzyCalculator
    {
        public double CalculateTheOutput(FuzzyLogicModel.FuzzyModel model, List<FuzzyLogicWebService.Models.InputValue> inputValues)
        {
            MamdaniFuzzySystem mamdaniModel = CreateMamdaniSystem(model);
            if (mamdaniModel != null)
            {
                Dictionary<FuzzyVariable, double> mamdaniInputValues = new Dictionary<FuzzyVariable, double>();
                foreach (FuzzyLogicWebService.Models.InputValue inputValue in inputValues)
                {
                    mamdaniInputValues.Add(mamdaniModel.InputByName(inputValue.VariableName), inputValue.VariableValue);
                }
                Dictionary<FuzzyVariable, double> result = mamdaniModel.Calculate(mamdaniInputValues);
                FuzzyLogicModel.FuzzyVariable outputVariable = model.FuzzyVariables.Where(m => m.VariableType == 1).First();
                return result[mamdaniModel.OutputByName(outputVariable.Name)];
            }
            else
            {
                throw new Exception("Cannot execute calculations!");
            }
            
        }

        private MamdaniFuzzySystem CreateMamdaniSystem(FuzzyLogicModel.FuzzyModel currentModel)
        {
            MamdaniFuzzySystem mamdaniModel = null;
            try
            {
                mamdaniModel = new MamdaniFuzzySystem();
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
            }catch(Exception){
                return mamdaniModel;
            }
            return mamdaniModel;
        }
    }
}