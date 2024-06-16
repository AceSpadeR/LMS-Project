using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LMS.Models;

namespace LMS.Data
{
    public class LMSContext : DbContext
    {
        public LMSContext (DbContextOptions<LMSContext> options)
            : base(options)
        {
        }

        public DbSet<LMS.Models.User> User { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => {
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
        public DbSet<LMS.Models.Event> Event { get; set; } = default!;

        public DbSet<LMS.Models.Class> Class { get; set; } = default!;
        public DbSet<LMS.Models.Registration> Registration { get; set; } = default!;

        public DbSet<LMS.Models.Assignment> Assignment { get; set; } = default!;
        public DbSet<LMS.Models.Submission> Submission { get; set; } = default!;
        public DbSet<LMS.Models.Notification> Notification { get; set; } = default!;
    }
}
