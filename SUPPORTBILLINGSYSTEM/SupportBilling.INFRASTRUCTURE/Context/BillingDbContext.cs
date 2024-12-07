using Microsoft.EntityFrameworkCore;
using SupportBilling.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.INFRASTRUCTURE.Context
{
    public class BillingDbContext : DbContext
    {
        public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ServiceEntity> Services { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships and constraints
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Client)
                .WithMany()
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InvoiceDetail>()
                .HasOne(id => id.Invoice)
                .WithMany(i => i.InvoiceDetails)
                .HasForeignKey(id => id.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InvoiceDetail>()
                .HasOne(id => id.Service)
                .WithMany()
                .HasForeignKey(id => id.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Invoice)
                .WithMany()
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
