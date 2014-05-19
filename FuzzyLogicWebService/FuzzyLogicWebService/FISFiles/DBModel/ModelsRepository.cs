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

        public IQueryable<User> Users
        {
            get
            {
                return context.Users;
            }
        }
    }
}