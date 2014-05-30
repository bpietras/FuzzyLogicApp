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
        public DbSet<InVariable> InputVariables { get; set; }
        public DbSet<OVariable> OutputVariables { get; set; }
        public DbSet<Rule> Rules { get; set; }


    }
}