using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FuzzyLogicModel;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;

namespace FuzzyLogicWebService.Models
{
    public class ModelsRepository
    {
        private FuzzyLogicDBContext context = new FuzzyLogicDBContext();

        public void RegisterUser(User user)
        {
            context.AddToUsers(user);
            context.SaveChanges();
        }

        public User GetuserByLogin(string login)
        {
            User user = context.Users.First(x => x.Name == login);
            return user;
        }

        public FuzzyModel GetModelById(int? modelId)
        {
            FuzzyModel model = context.FuzzyModels.First(x=>x.ModelID == modelId);
            return model;
        }
        
        public FuzzyModel EditModel(FuzzyModel newModel)
        {
            //context.FuzzyModels.Attach(new FuzzyModel { ModelID = newModel.ModelID });
            FuzzyModel updatedModel = context.FuzzyModels.ApplyCurrentValues(newModel);
            context.SaveChanges();
            return updatedModel;

        }

        public IQueryable<FuzzyModel> GetUserModels(int userID)
        {
            IQueryable<FuzzyModel> allUserModels = context.FuzzyModels.Where(x => x.UserID == userID);
            //List<FuzzyModel> modelsToDisplay = new List<FuzzyModel>();
            //List<FuzzyModel> modelsToDelete = new List<FuzzyModel>();
            //foreach (FuzzyModel userModel in allUserModels)
            //{
            //    if (userModel.IsSaved == 1)
            //    {
            //        modelsToDisplay.Add(userModel);
            //    }
            //    else
            //    {
            //        modelsToDelete.Add(userModel);
            //    }
            //}
            //if (modelsToDelete.Capacity > 0)
            //{
            //    DeleteAllUnsavedModels(modelsToDelete);
            //}

            //return modelsToDisplay.AsQueryable();
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
            context.ExecuteStoreCommand(bulkDeleteQuery);
        }

        public void SetAsSaved(int? modelId)
        {
            FuzzyModel modelToBeSaved = context.FuzzyModels.First(x => x.ModelID == modelId);
            modelToBeSaved.IsSaved = 1;
            EditModel(modelToBeSaved);
        }

        public void DeleteModelById(int? id)
        {
            if (id != null)
            {
                FuzzyModel modelToDelete = context.FuzzyModels.First(x=>x.ModelID == id);
                if (modelToDelete != null)
                {
                    context.FuzzyModels.DeleteObject(modelToDelete);
                    context.SaveChanges();
                }
            }
        }

        public FuzzyModel AddModelForUser(int userId, FuzzyModel model)
        {
            model.UserID = userId;
            context.FuzzyModels.AddObject(model);
            context.SaveChanges();
            return GetModelByName(model.Name, model.Description, userId);
        }

        private FuzzyModel GetModelByName(string name, string desc, int userId)
        {
            return context.FuzzyModels.First(x => x.Name == name && x.Description == desc && x.UserID == userId);
        }

        public void AddInputVariableForModel(int modelId, IEnumerable<FuzzyVariable> variables)
        {
            int counter = 0;
            foreach (FuzzyVariable v in variables)
            {
                v.ModelID = modelId;
                v.VariableType = 0;
                v.VariableIndex = counter;
                context.AddToFuzzyVariables(v);
                counter++;
            }
            context.SaveChanges();
        }

        public FuzzyVariable GetVariableById(int variableId)
        {
            return context.FuzzyVariables.First(x => x.VariableID == variableId);
        }

        public void AddOutputVariableForModel(int modelId, IEnumerable<FuzzyVariable> variables)
        {
            int counter = 0;
            foreach (FuzzyVariable v in variables)
            {
                v.ModelID = modelId;
                v.VariableType = 1;
                v.VariableIndex = counter;
                context.AddToFuzzyVariables(v);
                counter++;
            }
            context.SaveChanges();
        }

        public void AddRulesToModel(int modelId, IEnumerable<FuzzyRule> rules)
        {
            foreach (FuzzyRule rule in rules)
            {
                rule.ModelID = modelId;
                context.AddToFuzzyRules(rule);
                context.SaveChanges();
            }
        }

        public List<MembershipFunction> AddMembFuncForVariable(int variableId, IEnumerable<MembershipFunction> listOfMfs)
        {
            List<MembershipFunction> updatedFunctions = new List<MembershipFunction>();
            if (GetMfForVariable(variableId).Count() > 0)
            {
                foreach (MembershipFunction mf in listOfMfs)
                {
                    context.MembershipFunctions.Attach(new MembershipFunction { FunctionID = mf.FunctionID });
                    MembershipFunction func = context.MembershipFunctions.ApplyCurrentValues(mf);
                    updatedFunctions.Add(func);
                }
                context.SaveChanges();
            }
            else
            {
                int counter = 1;
                foreach (MembershipFunction mf in listOfMfs)
                {
                    mf.VariableID = variableId;
                    if (mf.FourthValue != null)
                    {
                        mf.Type = "Trapezoid";
                    }
                    else
                    {
                        mf.Type = "Triangle";
                    }
                    mf.FunctionID = counter;
                    context.AddToMembershipFunctions(mf);
                    counter++;
                    updatedFunctions.Add(mf);
                }
                context.SaveChanges();
            }
            return updatedFunctions;
        }

        public IQueryable<FuzzyVariable> GetVariablesForModel(int? modelId)
        {
            IQueryable<FuzzyVariable> allVariables = context.FuzzyVariables.Where(x => x.ModelID == modelId);
            return allVariables;
        }

        public IQueryable<MembershipFunction> GetMfForVariable(int variableId)
        {
            return context.MembershipFunctions.Where(x=>x.VariableID == variableId);
        }

        public IQueryable<FuzzyRule> GetRulesForModel(int? modelId)
        {
            return context.FuzzyRules.Where(x => x.ModelID == modelId);
        }

        public void UpdateModelStatus(int modelID, int status){
            FuzzyModel model = GetModelById(modelID);
            model.IsSaved = status;
            EditModel(model);
        }

        public void UpdateModelStatus(FuzzyModel model, int status)
        {
            model.IsSaved = status;
            EditModel(model);
        }
                        
    }
}