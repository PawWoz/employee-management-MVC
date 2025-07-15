using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseServices
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Models.Department> Departments { get; set; }
        public DbSet<Models.User> Users { get; set; }


        public DatabaseContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
