using BoreholeSystem.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoreholeSystem.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<InclinometerModel> InclinometersData { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=BoreholeSystem.db");
        }
    }
}
