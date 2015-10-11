﻿using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyLogicModel;
using FuzzyLogicWebService.Helpers;

namespace FuzzyLogicWebService.Models
{
    public class FuzzyLogicDbRepository : IDatabaseRepository
    {
        private FuzzyLogicDBEntities dbContext;

        public FuzzyLogicDbRepository(FuzzyLogicDBEntities context){
            dbContext = context;
        }


        public void RegisterUser(User user)
        {
            dbContext.AddToUsers(user);
            dbContext.SaveChanges();
        }

        public User GetuserByLogin(string login)
        {
            User user = dbContext.Users.First(x => x.Name == login);
            return user;
        }

        public FuzzyModel GetModelById(int modelId)
        {
            FuzzyModel model = dbContext.FuzzyModels.First(x=>x.ModelID == modelId);
            return model;
        }
        
        public FuzzyModel EditModel(FuzzyModel newModel)
        {
            dbContext.FuzzyModels.Attach(new FuzzyModel { ModelID = newModel.ModelID });
            FuzzyModel updatedModel = dbContext.FuzzyModels.ApplyCurrentValues(newModel);
            dbContext.SaveChanges();
            return updatedModel;

        }

        public IQueryable<FuzzyModel> GetUserModels(int userID)
        {
            IQueryable<FuzzyModel> allUserModels = dbContext.FuzzyModels.Where(x => x.UserID == userID);
            return allUserModels.AsQueryable();
        }

        private void DeleteAllUnsavedModels(List<FuzzyModel> modelsToDelete)
        {
            string bulkDeleteQuery = "DELETE FROM FuzzyModels WHERE ModelID IN (";
            foreach (FuzzyModel model in modelsToDelete)
            {
                bulkDeleteQuery = bulkDeleteQuery + model.ModelID + ", ";
            }

            bulkDeleteQuery = bulkDeleteQuery.Remove(bulkDeleteQuery.Length -2, 2) + ')';
            dbContext.ExecuteStoreCommand(bulkDeleteQuery);
        }
        
        public void DeleteModelById(int id)
        {
            if (id != 0)
            {
                FuzzyModel modelToDelete = dbContext.FuzzyModels.First(x=>x.ModelID == id);
                if (modelToDelete != null)
                {
                    dbContext.FuzzyModels.DeleteObject(modelToDelete);
                    dbContext.SaveChanges();
                }
            }
        }

        public FuzzyModel AddModelForUser(int userId, FuzzyModel model)
        {
            model.UserID = userId;
            dbContext.FuzzyModels.AddObject(model);
            dbContext.SaveChanges();
            return GetModelByName(model.Name, model.Description, userId);
        }

        private FuzzyModel GetModelByName(string name, string desc, int userId)
        {
            return dbContext.FuzzyModels.First(x => x.Name == name && x.Description == desc && x.UserID == userId);
        }

        public void AddInputVariableForModel(int modelId, IEnumerable<FuzzyVariable> variables)
        {
            FuzzyModel currentModel = GetModelById(modelId);
            int counter = 0;
            foreach (FuzzyVariable v in variables)
            {
                //v.ModelID = modelId;
                v.VariableType = FuzzyLogicService.InputVariable;
                v.VariableIndex = counter;
                v.Name = v.Name.Replace(" ", "");
                currentModel.FuzzyVariables.Add(v);
                //dbContext.AddToFuzzyVariables(v);
                counter++;
            }
            dbContext.SaveChanges();
        }

        public FuzzyVariable GetVariableById(int variableId)
        {
            return dbContext.FuzzyVariables.First(x => x.VariableID == variableId);
        }

        public void AddOutputVariableForModel(int modelId, IEnumerable<FuzzyVariable> variables)
        {
            int counter = 0;
            foreach (FuzzyVariable v in variables)
            {
                v.ModelID = modelId;
                v.VariableType = FuzzyLogicService.OutputVariable;
                v.VariableIndex = counter;
                v.Name = v.Name.Replace(" ", "");
                dbContext.AddToFuzzyVariables(v);
                counter++;
            }
            dbContext.SaveChanges();
        }

        public void AddRulesToModel(int modelId, IEnumerable<FuzzyRule> rules)
        {
            FuzzyModel currentModel = GetModelById(modelId);
            foreach (FuzzyRule rule in rules)
            {
                //rule.ModelID = modelId;
                //dbContext.AddToFuzzyRules(rule);
                currentModel.FuzzyRules.Add(rule);
                dbContext.SaveChanges();
            }
        }

        public List<MembershipFunction> AddMembFuncForVariable(int variableId, List<MembershipFunction> listOfMfs)
        {
            List<MembershipFunction> updatedFunctions = new List<MembershipFunction>();
            if (GetMfForVariable(variableId).Count() > 0)
            {
                foreach (MembershipFunction mf in listOfMfs)
                {
                    mf.Name = mf.Name.Replace(" ", "");
                    dbContext.MembershipFunctions.Attach(new MembershipFunction { FunctionID = mf.FunctionID });
                    MembershipFunction func = dbContext.MembershipFunctions.ApplyCurrentValues(mf);
                    updatedFunctions.Add(func);
                }
                dbContext.SaveChanges();
            }
            else
            {
                int counter = 1;
                foreach (MembershipFunction mf in listOfMfs)
                {
                    mf.Name = mf.Name.Replace(" ", "");
                    mf.FunctionIndex = counter;
                    mf.VariableID = variableId;
                    if (mf.FourthValue != null)
                    {
                        mf.Type = "Trapezoid";
                    }
                    else
                    {
                        mf.Type = "Triangle";
                    }
                    dbContext.AddToMembershipFunctions(mf);
                    updatedFunctions.Add(mf);
                    counter++;
                }
                dbContext.SaveChanges();
            }
            return updatedFunctions;
        }

        public IQueryable<FuzzyVariable> GetVariablesForModel(int modelId)
        {
            IQueryable<FuzzyVariable> allVariables = dbContext.FuzzyVariables.Where(x => x.ModelID == modelId);
            return allVariables;
        }

        public IQueryable<MembershipFunction> GetMfForVariable(int variableId)
        {
            return dbContext.MembershipFunctions.Where(x=>x.VariableID == variableId);
        }

        public IQueryable<FuzzyRule> GetRulesForModel(int modelId)
        {
            return dbContext.FuzzyRules.Where(x => x.ModelID == modelId);
        }

        public void UpdateModelStatus(int modelID, int status){
            FuzzyModel model = GetModelById(modelID);
            model.IsSaved = status;
            FuzzyModel updatedModel = dbContext.FuzzyModels.ApplyCurrentValues(model);
            dbContext.SaveChanges();
            //EditModel(model);
        }

        public void UpdateModelStatus(FuzzyModel model, int status)
        {
            model.IsSaved = status;
            FuzzyModel updatedModel = dbContext.FuzzyModels.ApplyCurrentValues(model);
            dbContext.SaveChanges();
            //EditModel(model);
        }

        public void CopyGivenModel(int modelId, int userId)
        {
            FuzzyModel originalObject = GetModelById(modelId);
            FuzzyModel newModelObject = new FuzzyModel();
            newModelObject.Name = Resources.Resources.NewModelName + originalObject.Name;
            newModelObject.Description = originalObject.Description;
            newModelObject.InputsNumber = originalObject.InputsNumber;
            newModelObject.OutputsNumber = 1;
            newModelObject.RulesNumber = originalObject.RulesNumber;
            newModelObject.IsSaved = originalObject.IsSaved;
            CloneFuzzyRules(originalObject.FuzzyRules, newModelObject);
            CloneFuzzyVariables(originalObject.FuzzyVariables, newModelObject);
            AddModelForUser(userId, newModelObject);
        }

        private void CloneFuzzyRules(IEnumerable<FuzzyRule> originalRules, FuzzyModel newModelObject)
        {
            List<FuzzyRule> clonedRules = new List<FuzzyRule>();
            foreach (FuzzyRule originalRule in originalRules)
            {
                FuzzyRule clonedRule = new FuzzyRule();
                clonedRule.StringRuleContent = originalRule.StringRuleContent;
                clonedRule.FISRuleContent = originalRule.FISRuleContent;
                newModelObject.FuzzyRules.Add(clonedRule);
            }
        }

        private void CloneFuzzyVariables(IEnumerable<FuzzyVariable> originalVariables, FuzzyModel newModelObject)
        {
            foreach (FuzzyVariable origVar in originalVariables)
            {
                FuzzyVariable clonedVar = new FuzzyVariable();
                clonedVar.Name = origVar.Name;
                clonedVar.MinValue = origVar.MinValue;
                clonedVar.MaxValue = origVar.MaxValue;
                clonedVar.NumberOfMembFunc = origVar.NumberOfMembFunc;
                clonedVar.VariableType = origVar.VariableType;
                clonedVar.VariableIndex = origVar.VariableIndex;
                foreach (MembershipFunction origMF in origVar.MembershipFunctions)
                {
                    MembershipFunction newMF = new MembershipFunction();
                    newMF.Name = origMF.Name;
                    newMF.Type = origMF.Type;
                    newMF.FirstValue = origMF.FirstValue;
                    newMF.SecondValue = origMF.SecondValue;
                    newMF.ThirdValue = origMF.ThirdValue;
                    newMF.FourthValue = origMF.FourthValue;
                    newMF.FunctionIndex = origMF.FunctionIndex;
                    clonedVar.MembershipFunctions.Add(newMF);
                }
                newModelObject.FuzzyVariables.Add(clonedVar);
            }
        }

        public void SaveEditedModel(FuzzyModel updatedModel)
        {
            string editModelQuery = String.Format("BEGIN TRAN UPDATE FuzzyModels SET Name='{0}', Description='{1}' WHERE ModelID={2} ", updatedModel.Name, updatedModel.Description, updatedModel.ModelID);
            foreach (FuzzyVariable variable in updatedModel.FuzzyVariables)
            {
                editModelQuery += String.Format("UPDATE FuzzyVariables SET Name='{0}', MinValue={1}, MaxValue={2} WHERE VariableID={3} ",variable.Name, variable.MinValue, variable.MaxValue, variable.VariableID);
                foreach(MembershipFunction function in variable.MembershipFunctions)
                {
                    editModelQuery += String.Format("UPDATE MembershipFunctions SET Name='{0}', FirstValue={1}, SecondValue={2}, ThirdValue={3},{4} Type='{5}' WHERE FunctionID={6} ",
                        function.Name, function.FirstValue, function.SecondValue, function.ThirdValue, function.FourthValue!=null? " FourthValue="+function.FourthValue+",":"", function.FourthValue!=null? "Trapezoid":"Triangle", function.FunctionID);
                }
            }
            foreach (FuzzyRule rule in updatedModel.FuzzyRules)
            {
                editModelQuery += String.Format("UPDATE FuzzyRules SET StringRuleContent='{0}', FISRuleContent='{1}' WHERE RuleID={2} ",
                    rule.StringRuleContent, rule.FISRuleContent, rule.RuleID);
            }
            editModelQuery += "COMMIT TRAN";
            try
            {
                dbContext.ExecuteStoreCommand(editModelQuery);
            }catch(Exception)
            {
                throw new Exception("Wystąpił błąd podczas edytowania");
            }
        }

    }
}