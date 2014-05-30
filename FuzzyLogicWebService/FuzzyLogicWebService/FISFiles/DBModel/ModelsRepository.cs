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

        public void AddInputVariableForModel(int modelId, IEnumerable<InVariable> variables)
        {
            foreach(InVariable v in variables){
                v.ModelID = modelId;
                context.InputVariables.Add(v);
                context.SaveChanges();
            }
        }

        public void AddOutputVariableForModel(int modelId, IEnumerable<OVariable> variables)
        {
            foreach (OVariable v in variables)
            {
                v.ModelID = modelId;
                context.OutputVariables.Add(v);
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

        public IQueryable<InVariable> InputVariables
        {
            get
            {
                return context.InputVariables;
            }
        }

        public IQueryable<OVariable> OutputVariables
        {
            get
            {
                return context.OutputVariables;
            }
        }

        public IQueryable<InVariable> GetVariablesForModel(int modelId)
        {
            return context.InputVariables.Where(x => x.ModelID == modelId);
        }
    }
}