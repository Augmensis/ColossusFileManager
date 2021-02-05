using ColossusFileManager.Shared.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ColossusFileManager.WebApi.Services
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<CbFolder> Folders { get; set; }

        public DbSet<CbFile> Files { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions) : base(contextOptions)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CbFolder>().HasMany(x => x.ChildFolders);
            builder.Entity<CbFolder>().HasMany(x => x.Files);

            builder.Entity<CbFile>().HasOne(x => x.ParentFolder).WithMany(x => x.Files);
        }


        
    }
}
