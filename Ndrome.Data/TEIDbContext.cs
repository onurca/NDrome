using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using Ndrome.Model.Authentication;
using Ndrome.Model.Business;

namespace Ndrome.Data
{
    public class NdromeDbContext : DbContext
    {
        public NdromeDbContext(DbContextOptions<NdromeDbContext> options) : base(options) { }

        public DbSet<Content> Contents { get; set; }
        public DbSet<ContentDetail> ContentDetails { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => base.OnModelCreating(modelBuilder);
    }

    public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<NdromeDbContext>
    {
        NdromeDbContext IDesignTimeDbContextFactory<NdromeDbContext>.CreateDbContext(string[] args)
        {
            var ob = new DbContextOptionsBuilder<NdromeDbContext>();
            ob.UseSqlServer(ConnectionString);
            return new NdromeDbContext(ob.Options);
        }

        private string ConnectionString
        {
            get
            {
                var currentPath = Directory.GetCurrentDirectory();
                return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=" + currentPath + @"\DATABASE\NdromeDB.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;";
            }
        }
    }
}

