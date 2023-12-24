using Gestion_Bib.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gestion_Bib.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Livre> Livres { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configuration de la relation entre Reservation et Livre
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Livre)
                .WithMany()
                .HasForeignKey(r => r.LivreId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}