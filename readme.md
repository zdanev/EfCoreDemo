# New features in EF Core 2.0

## Global Query Filters

    modelBuilder.Entity<Post>()
      .HasQueryFilter(p => !p.IsDeleted);
#
    .IgnoreQueryFilters()

## Flexible mapping

    modelBuilder.Entity<Blog>()
      .Property<string>("TenantId")
      .HasField("_tenantId");

    modelBuilder.Entity<Blog>()
      .HasQueryFilter(b => EF.Property<string>(b, "TenantId") == tenantId);
#

    modelBuilder.Entity<Post>()
      .Property<bool>("IsDeleted");

    modelBuilder.Entity<Post>()
      .HasQueryFilter(p => !EF.Property<bool>(p, "IsDeleted"));

## Context pooling

## EF Functions

    return _db.Blogs.Where(b => EF.Functions.Like(b.Url, likeExpr));	

## Compiled queries

    var query = EF.CompiledQuery(
        (MyDbContext db, int id) => db.Customers.Where(c => c.Id == id).Single());

## Owned Entities (nightly builds)

    modelBuilder.Entity<Customer>()
      .OwnsOne(c => c.WorkAddress)
      .ToTable("WorkAddress"); 

## Seed Data (future)

    modelBuilder.Entity<User>().SeedData(
      new User { Id = 1, Name = "admin" },
      new User { Id = 2, Name = "guest" }
    );


[link](https://github.com/rowanmiller/Demo-EFCore/blob/master/CompletedSourceCode/SeedData/Migrations/)

## Setup

md EfCoreDemo

cd EfCoreDemo

dotnet --version

dotnet new console

dotnet restore

dotnet run

add nuget package Microsoft.EntityFrameworkCore 2.0.0-preview1

add nuget package Microsoft.EntityFrameworkCore.SqlServer 2.0.0-preview1