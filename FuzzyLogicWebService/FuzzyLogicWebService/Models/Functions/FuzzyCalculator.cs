using System.Collections.Generic;
using System.Linq;
using AI.Fuzzy.Library;
using FuzzyLogicWebService.Helpers;
using System;

namespace FuzzyLogicWebService.Models.Functions
{
    public class FuzzyCalculator
    {
        public Double CalculateTheOutput(FuzzyLogicModel.FuzzyModel model, List<FuzzyLogicWebService.Models.InputValue> inputValues)
        {
            MamdaniFuzzySystem mamdaniModel = CreateMamdaniSystem(model);
            if (mamdaniModel != null)
            {
                Dictionary<FuzzyVariable, double> mamdaniInputValues = new Dictionary<FuzzyVariable, double>();
                foreach (FuzzyLogicWebService.Models.InputValue inputValue in inputValues)
                {
                    mamdaniInputValues.Add(mamdaniModel.InputByName(inputValue.VariableName), (double) inputValue.VariableValue);
                }
                Dictionary<FuzzyVariable, double> result = mamdaniModel.Calculate(mamdaniInputValues);
                FuzzyLogicModel.FuzzyVariable outputVariable = model.FuzzyVariables.First(m => m.VariableType == FuzzyLogicService.OutputVariable);
                Double defuzziedResult = result[mamdaniModel.OutputByName(outputVariable.Name)];
                return defuzziedResult;
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
                    FuzzyVariable mamdaniVariable = new FuzzyVariable(variable.Name, Convert.ToDouble(variable.MinValue), Convert.ToDouble(variable.MaxValue));
                    foreach (FuzzyLogicModel.MembershipFunction function in variable.MembershipFunctions)
                    {
                        if (function.Type == FuzzyLogicService.TriangleFunction.ToString())
                        {
                            mamdaniVariable.Terms.Add(new FuzzyTerm(function.Name, new TriangularMembershipFunction(Convert.ToDouble(function.FirstValue),
                                Convert.ToDouble(function.SecondValue),Convert.ToDouble(function.ThirdValue))));
                        }
                        if (function.Type == FuzzyLogicService.TrapezoidFunction.ToString()&& function.FourthValue != null)
                        {
                            mamdaniVariable.Terms.Add(new FuzzyTerm(function.Name, new TrapezoidMembershipFunction(Convert.ToDouble(function.FirstValue),
                                Convert.ToDouble(function.SecondValue), Convert.ToDouble(function.ThirdValue), Convert.ToDouble(function.FourthValue))));
                        }
                    }
                    if (variable.VariableType == FuzzyLogicService.InputVariable)
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
                    MamdaniFuzzyRule mamdaniRule = mamdaniModel.ParseRule(rule.StringRuleContent);
                    mamdaniModel.Rules.Add(mamdaniRule);
                }
            }catch(Exception){
                return mamdaniModel;
            }
            return mamdaniModel;
        }
    }
}