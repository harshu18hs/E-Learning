using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ObjectModel;
namespace ObjectModel
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public ApplicationDbContext() : base()
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
       
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Calendar> Events { get; set; }
        public DbSet<ArticleUpload> Resources { get; set; }
        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<FileUpload> Files { get; set; }

    }
}

