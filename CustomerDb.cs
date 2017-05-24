using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo
{
    public class CustomerDb : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .OwnsOne(c => c.BillingAddress)
                    .OwnsOne(a => a.Location);

            modelBuilder.Entity<Customer>()
                .OwnsOne(c => c.ShippingAddress)
                    //.ToTable("ShippingAddress");
                    .OwnsOne(a => a.Location);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=.\sse2014;Database=CustomerDemo;Trusted_Connection=True;");
        }
    }

    public class Customer 
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public Address BillingAddress { get; set; }
        public Address ShippingAddress { get; set; }
    }

    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }

        // public string City { get; set; }
        // public string State { get; set; }
        // public string Zip { get; set; }
        public Location Location { get; set; }
    }

    public class Location
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public string CountryId { get; set; }
        public Country Country { get; set; }
    }

    public class Country 
    {
        public string CountryId { get; set; }
    }
}