using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class OrdersTaskContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();
        modelBuilder.Entity<Order>().Property(u => u.Status).HasConversion<string>();

        //настройка наследования таблиц
        modelBuilder.Entity<User>()
            .HasDiscriminator(u => u.Role)
            .HasValue<Customer>(UserRole.Customer);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        // автогенерация айдишников 
        modelBuilder.Entity<User>().Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<Customer>().Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<Order>().Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<Item>().Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<OrderItem>().Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

        base.OnModelCreating(modelBuilder);
    }
}