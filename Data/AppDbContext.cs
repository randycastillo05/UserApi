using Microsoft.EntityFrameworkCore;
using UsuariosAPI.Models;

namespace UsuariosAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                
                entity.HasIndex(e => e.Email).IsUnique();

               
                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime2");
            });
        }
    }
}