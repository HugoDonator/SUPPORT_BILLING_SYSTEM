using Microsoft.EntityFrameworkCore;
using SupportBilling.DOMAIN.Entities;

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
        public DbSet<InvoiceStatus> InvoiceStatus { get; set; } // Usa el nombre exacto

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // Clave primaria para InvoiceStatus
            modelBuilder.Entity<InvoiceStatus>()
                .HasKey(s => s.Id);

            // Relación entre Invoice y InvoiceStatus
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Status)
                .WithMany()
                .HasForeignKey(i => i.StatusId);

            // Relación entre Invoice y Client
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Client)
                .WithMany()
                .HasForeignKey(i => i.ClientId);

            // Relación entre InvoiceDetail y Invoice
            modelBuilder.Entity<InvoiceDetail>()
                .HasOne(d => d.Invoice)
                .WithMany(i => i.InvoiceDetails)
                .HasForeignKey(d => d.InvoiceId);

            // Relación entre InvoiceDetail y Service
            modelBuilder.Entity<InvoiceDetail>()
                .HasOne(d => d.Service)
                .WithMany()
                .HasForeignKey(d => d.ServiceId);
        }
    }
}
