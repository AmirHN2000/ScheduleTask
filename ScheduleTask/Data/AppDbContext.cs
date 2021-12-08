using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ScheduleTask.Data
{
    public class AppDbContext:IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options):base(options)
        {
        }
        
        public DbSet<NewTask> Tasks { get; set; }
        public DbSet<NecessaryTime> Times { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<User>().Ignore(x => x.Email);
            builder.Entity<User>().Ignore(x => x.EmailConfirmed);
            builder.Entity<User>().Ignore(x => x.NormalizedEmail);
            builder.Entity<User>().Ignore(x => x.PhoneNumber);
            builder.Entity<User>().Ignore(x => x.PhoneNumberConfirmed);
            builder.Entity<User>().Ignore(x => x.TwoFactorEnabled);
            
            builder.Entity<IdentityRole>().HasData(new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id = "07351063-71c9-48bc-9a84-60c1e2711ca1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole()
                {
                    Id = "a597799a-fb1b-44b2-939f-48731dec318e",
                    Name = "User",
                    NormalizedName = "USER"
                }
            });
            builder.Entity<User>().HasData(new User()
            {
                Id = "07351063-71c9-48bc-9a84-60c1e27111ca",
                Name = "admin",
                Family = "admin",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PasswordHash = "AQAAAAEAACcQAAAAEOacqtJX5VRYz8/FZ+fkvAncB9R4WARR2j32jMo3t1+oX2+Jb99KHJPRsV4ElRYWyA==",
                SecurityStamp = "TJJ3A4MT57QJJUADYYUDL7ZMUVDQDXWR",
                AccessFailedCount = 0
                //ConcurrencyStamp = "0fe01dfb-57f3-4008-901e-3039a5878cb5"
            });

            builder.Entity<NecessaryTime>().HasData(new NecessaryTime()
            {
                Name = "LimitedTimeInsertTask",
                Hour = 12,
                Minute = 0
            });
            
        }
    }
}