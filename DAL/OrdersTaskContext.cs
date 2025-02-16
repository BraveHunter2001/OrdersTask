using DAL.Entities;
using DAL.Sequenser;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class OrdersTaskContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Cart> Carts { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    public OrdersTaskContext(DbContextOptions<OrdersTaskContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();
        modelBuilder.Entity<Order>().Property(u => u.Status).HasConversion<string>();

        modelBuilder.Entity<User>()
            .HasOne(u => u.Customer)
            .WithOne(c => c.User)
            .HasForeignKey<Customer>(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Cart)
            .WithOne(cu => cu.Customer)
            .HasForeignKey<Cart>(cu => cu.CustomerId);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.CardId);

        // автогенерация айдишников 
        modelBuilder.Entity<User>().Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<Customer>().Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<Order>().Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<Item>().Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<OrderItem>().Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<CartItem>().Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<Cart>().Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

        modelBuilder
            .HasSequence<int>(nameof(SequenceType.CustomerSequence))
            .StartsAt(1)
            .IncrementsBy(1)
            .HasMax(int.MaxValue);

        modelBuilder
            .HasSequence<long>(nameof(SequenceType.ItemSequence))
            .StartsAt(1)
            .IncrementsBy(1)
            .HasMax(int.MaxValue);

        modelBuilder
            .HasSequence<int>(nameof(SequenceType.OrderSequence))
            .StartsAt(1)
            .IncrementsBy(1)
            .HasMax(int.MaxValue);

        base.OnModelCreating(modelBuilder);
    }
}