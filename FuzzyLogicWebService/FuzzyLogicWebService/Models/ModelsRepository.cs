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

        public FuzzyModel GetModelById(int? modelId)
        {
            FuzzyModel model = context.FuzzyModels.First(x=>x.ModelID == modelId);
            return model;
        }

        public FuzzyModel EditModel(FuzzyModel newModel)
        {
            context.FuzzyModels.Attach(new FuzzyModel { ModelID = newModel.ModelID });
            FuzzyModel updatedModel = context.FuzzyModels.ApplyCurrentValues(newModel);
            context.SaveChanges();
            return updatedModel;

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
                FuzzyModel modelToDelete = context.FuzzyModels.First(x=>x.ModelID == id);
                context.FuzzyModels.DeleteObject(modelToDelete);
                context.SaveChanges();
            }
        }

        public FuzzyModel AddModelForUser(int userId, FuzzyModel model)
        {
            model.UserID = userId;
            context.FuzzyModels.AddObject(model);
            context.SaveChanges();
            return GetModelByName(model.Name, model.Description, userId);//context.FuzzyModels.Where(x => x.Name == model.Name && x.Description == model.Description).First().ModelID;
        }

        private FuzzyModel GetModelByName(string name, string desc, int userId)
        {
            return context.FuzzyModels.Where(x => x.Name == name && x.Description == desc && x.UserID == userId).First();
        }

        public void AddInputVariableForModel(int modelId, IEnumerable<FuzzyVariable> variables)
        {
            foreach(FuzzyVariable v in variables){
                v.ModelID = modelId;
                v.VariableType = 0;
                context.AddToFuzzyVariables(v);
                context.SaveChanges();
            }
        }

        public FuzzyVariable GetVariableById(int variableId)
        {
            return context.FuzzyVariables.First(x => x.VariableID == variableId);
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
                    context.SaveChanges();
                    updatedFunctions.Add(func);
                }
            }
            else
            {
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
                    context.AddToMembershipFunctions(mf);
                    updatedFunctions.Add(mf);
                    context.SaveChanges();
                }
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
                        
    }
}