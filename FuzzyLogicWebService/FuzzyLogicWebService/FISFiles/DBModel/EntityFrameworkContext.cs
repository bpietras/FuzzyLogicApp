using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace FuzzyLogicWebService.FISFiles.DBModel
{
    public class EntityFrameworkContext:DbContext
    {

        public EntityFrameworkContext() : base("FuzzyLogicDB") { }

        public DbSet<FuzzyModel> Models { get; set; }
        public DbSet<User> Users { get; set; }

    }
}