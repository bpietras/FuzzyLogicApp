using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyLogicWebService.FISFiles.DBModel
{
    public class ModelsRepository
    {
        private EntityFrameworkContext context = new EntityFrameworkContext();

        public IQueryable<FuzzyModel> Models
        {
            get
            {
                return context.Models;
            }
        }

        public FuzzyModel GetModelById(int? modelId)
        {
            return context.Models.Find(modelId);
        }

        public void EditModel(FuzzyModel newModel)
        {
            context.Entry(newModel).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

        }

        public IQueryable<FuzzyModel> GetUserModels(int userID)
        {
                return Models.Where(x=>x.UserID == userID);
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
        }

        public IQueryable<User> Users
        {
            get
            {
                return context.Users;
            }
        }

        public IQueryable<FVariable> InputVariables
        {
            get
            {
                return context.FuzzyVariables.Where(m=>m.VariableType == 0);
            }
        }

        public IQueryable<FVariable> OutputVariables
        {
            get
            {
                return context.FuzzyVariables.Where(m => m.VariableType == 1);
            }
        }

        public IQueryable<FVariable> GetVariablesForModel(int modelId)
        {
            return context.FuzzyVariables.Where(x => x.ModelID == modelId);
        }
    }
}