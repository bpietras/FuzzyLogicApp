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

        public void AddModelForUser(int userId, FuzzyModel model)
        {
            model.UserID = userId;
            context.Models.Add(model);
            context.SaveChanges();
        }

        public IQueryable<User> Users
        {
            get
            {
                return context.Users;
            }
        }
    }
}