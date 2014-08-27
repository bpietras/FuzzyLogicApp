using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FuzzyLogicModel;

namespace FuzzyLogicWebService.Models
{
    public class ModelsRepository
    {
        private FuzzyLogicDBContext context = new FuzzyLogicDBContext();        

        public FuzzyModel GetModelById(int? modelId)
        {
            FuzzyModel model = context.FuzzyModels.Where(x=>x.ModelID == modelId).First();
            return model;
        }

        public void EditModel(FuzzyModel newModel)
        {
            context.ApplyCurrentValues("FuzzyModel", newModel);
            context.SaveChanges();

        }

        public IQueryable<FuzzyModel> GetUserModels(int userID)
        {
            IQueryable<FuzzyModel> modelss = context.FuzzyModels.Where(x=>x.UserID == userID);
            return modelss;
        }

        public void DeleteModelById(int? id)
        {
            if (id != null)
            {
                FuzzyModel modelToDelete = context.FuzzyModels.Where(x=>x.ModelID == id).First();
                context.FuzzyModels.DeleteObject(modelToDelete);
                context.SaveChanges();
            }
        }

        public int AddModelForUser(int userId, FuzzyModel model)
        {
            model.UserID = userId;
            context.AddToFuzzyModels(model);
            context.SaveChanges();
            return context.FuzzyModels.Where(x => x.Name == model.Name && x.Description == model.Description).First().ModelID;
        }

        public void AddInputVariableForModel(int modelId, IEnumerable<FuzzyVariable> variables)
        {
            foreach(FuzzyVariable v in variables){
                v.ModelID = modelId;
                v.VariableType = 0;
                context.AddToFuzzyVariables(v);
                context.SaveChanges();
            }

            //IEnumerable<FVariable> all = context.FuzzyVariables.Where(x => x.ModelID == modelId && x.VariableType == 0).AsEnumerable();
            //return all;
        }

        public FuzzyVariable GetVariableById(int variableId)
        {
            return context.FuzzyVariables.Where(x=>x.VariableID == variableId).First();
        }

        public void AddOutputVariableForModel(int modelId, IEnumerable<FuzzyVariable> variables)
        {
            foreach (FuzzyVariable v in variables)
            {
                v.ModelID = modelId;
                v.VariableType = 1;
                context.AddToFuzzyVariables(v);
                context.SaveChanges();
            }
            //IEnumerable<FVariable> all = context.FuzzyVariables.Where(x => x.ModelID == modelId && x.VariableType == 1).AsEnumerable();
            //return all;
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

        public void AddMembFuncForVariable(int variableId, IEnumerable<MembershipFunction> listOfMfs)
        {
            foreach (MembershipFunction mf in listOfMfs)
            {
                mf.VariableID = variableId;
                context.AddToMembershipFunctions(mf);
                context.SaveChanges();
            }
        }

        public IQueryable<FuzzyVariable> GetVariablesForModel(int? modelId, bool isEagerLoad)
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
                        
    }
}