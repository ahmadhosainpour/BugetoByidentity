using Buget_store.Application.Interface.Context;
using Bugeto_store.Domain.Entities.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugeto_store.Persistence.Context
{
    public class DataBaseContext:IdentityDbContext,IDatabaseContext
    {


        public DataBaseContext(DbContextOptions options):base(options) 
        {
            
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)  //data seeding
        {
           base.OnModelCreating(modelBuilder);
        //    //modelBuilder.Entity<Role>().HasData(
        //    //new Role { Id = 1, Name = "Admin" },
        //    //new Role { Id = 2, Name = "User" },
        //    //new Role { Id = 3, Name = "Guest" }
        //);
        }
    }
}
