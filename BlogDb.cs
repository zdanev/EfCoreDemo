using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo
{
    public class Blog
    {
        string _tenantId;
        // public string TenantId { get; set; }
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public ICollection<Post> Posts { get; set; }
    }

    public class Post 
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        // public bool IsDeleted { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }

    public class BlogDb : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        private readonly string _tenantId;
        public BlogDb(string tenantId)
        {
            this._tenantId = tenantId;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .Property<string>("TenantId")
                .HasField("_tenantId")
                .Metadata.IsReadOnlyAfterSave = true;

            modelBuilder.Entity<Blog>()
                .HasQueryFilter(b => EF.Property<string>(b, "TenantId") == _tenantId);

            // modelBuilder.Entity<Blog>()
            //     .HasQueryFilter(b => b.TenantId == tenantId);

            modelBuilder.Entity<Post>()
                .Property<bool>("IsDeleted");

            // modelBuilder.Entity<Post>()
            //     .HasQueryFilter(b => !b.IsDeleted);

            modelBuilder.Entity<Post>()
                .HasQueryFilter(p => !EF.Property<bool>(p, "IsDeleted"));
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Added
                 && e.Metadata.GetProperties().Any(p => p.Name == "TenantId")))
            {
                item.CurrentValues["TenantId"] = _tenantId;
            }

            foreach (var item in ChangeTracker.Entries<Post>().Where(e => e.State == EntityState.Deleted))
            {
                item.State = EntityState.Modified;
                item.CurrentValues["IsDeleted"] = true;
            }

            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=.\sse2014;Database=BlogDemo;Trusted_Connection=True;");
        }
    }
}