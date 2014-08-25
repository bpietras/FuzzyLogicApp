using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyLogicWebService.FISFiles.DBModel
{
    public class ModelsRepository
    {
        private EntityFrameworkContext context = new EntityFrameworkContext();

        public ModelsRepository()
        {
            context.Configuration.LazyLoadingEnabled = false;
            context.Configuration.AutoDetectChangesEnabled = true;
        }
        

        public FuzzyModel GetModelById(int? modelId, bool isEagerLoad)
        {
            FuzzyModel model = context.Models.Find(modelId);
            if (isEagerLoad)
            {
                IEnumerable<FVariable> variables = GetVariablesForModel(modelId, true);
                IEnumerable<Rule> rules = GetRulesForModel(modelId);
                    //context.FuzzyVariables.Where(x => x.ModelID == modelId).AsEnumerable();
                model.Variables = variables;
                model.Rules = rules;
            }
            return model;
        }

        public void EditModel(FuzzyModel newModel)
        {
            context.Entry(newModel).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

        }

        public IQueryable<FuzzyModel> GetUserModels(int userID)
        {
            IQueryable<FuzzyModel> modelss = Models.Where(x=>x.UserID == userID);
            return modelss;
        }

        public void DeleteModelById(int? id)
        {
            if (id != null)
            {
                FuzzyModel modelToDelete = context.Models.Find(id);
                context.Models.Remove(modelToDelete);
                context.SaveChanges();
            }
        }

        public int AddModelForUser(int userId, FuzzyModel model)
        {
            model.UserID = userId;
            context.Models.Add(model);
            context.SaveChanges();
            return context.Models.Where(x=>x.Name == model.Name && x.Description == model.Description).First().ModelID;
        }

        public void AddInputVariableForModel(int modelId, IEnumerable<FVariable> variables)
        {
            foreach(FVariable v in variables){
                v.ModelID = modelId;
                v.VariableType = 0;
                context.FuzzyVariables.Add(v);
                context.SaveChanges();
            }

            //IEnumerable<FVariable> all = context.FuzzyVariables.Where(x => x.ModelID == modelId && x.VariableType == 0).AsEnumerable();
            //return all;
        }

        public FVariable GetVariableById(int variableId)
        {
            return context.FuzzyVariables.Find(variableId);
        }

        public void AddOutputVariableForModel(int modelId, IEnumerable<FVariable> variables)
        {
            foreach (FVariable v in variables)
            {
                v.ModelID = modelId;
                v.VariableType = 1;
                context.FuzzyVariables.Add(v);
                context.SaveChanges();
            }
            //IEnumerable<FVariable> all = context.FuzzyVariables.Where(x => x.ModelID == modelId && x.VariableType == 1).AsEnumerable();
            //return all;
        }

        public void AddRulesToModel(int modelId, IEnumerable<Rule> rules)
        {
            foreach (Rule rule in rules)
            {
                rule.ModelID = modelId;
                context.Rules.Add(rule);
                context.SaveChanges();
            }
        }

        public void AddMembFuncForVariable(int variableId, IEnumerable<MembershipFunction> listOfMfs)
        {
            foreach (MembershipFunction mf in listOfMfs)
            {
                mf.VariableID = variableId;
                context.MembershipFunctions.Add(mf);
                context.SaveChanges();
            }
        }

        public IQueryable<FVariable> GetVariablesForModel(int? modelId, bool isEagerLoad)
        {
            IQueryable<FVariable> allVariables = context.FuzzyVariables.Where(x => x.ModelID == modelId);
            foreach(FVariable variable in allVariables){
                IQueryable<MembershipFunction> allMfs = GetMfForVariable(variable.VariableID);
                variable.MfFunctions = allMfs;
            }
            return allVariables;
        }

        public IQueryable<MembershipFunction> GetMfForVariable(int variableId)
        {
            return context.MembershipFunctions.Where(x=>x.VariableID == variableId);
        }

        public IQueryable<Rule> GetRulesForModel(int? modelId)
        {
            return context.Rules.Where(x => x.ModelID == modelId);
        }

        public IQueryable<User> Users
        {
            get
            {
                return context.Users;
            }
        }

        public IQueryable<FuzzyModel> Models
        {
            get
            {
                return context.Models;
            }
        }

        public IQueryable<FVariable> Variables
        {
            get
            {
                return context.FuzzyVariables;
            }
        }

        public IQueryable<Rule> Rules
        {
            get
            {
                return context.Rules;
            }
        }
        
    }
}