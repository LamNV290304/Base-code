using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SEP490.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        #region Logging
        /// <summary>
        /// Logging config
        /// !!!WARNING, DO NOT DELETE THIS SECTION!!! 
        /// If you delete this section, the logging feature will be broken.
        /// </summary>
        public DbSet<LogError> LogErrors => Set<LogError>();
        public DbSet<LogActivity> LogActivities => Set<LogActivity>();
        public DbSet<LogHistory> LogHistories => Set<LogHistory>();
        public DbSet<LogLogin> LogLogins => Set<LogLogin>();
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
