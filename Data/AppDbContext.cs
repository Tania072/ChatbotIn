using Microsoft.EntityFrameworkCore;
using Prueba2.Models;

namespace Prueba2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configuración del modelo Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                // Clave primaria
                entity.HasKey(u => u.Id);
                
                // Configuración de propiedades
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(256);
                    
                entity.Property(u => u.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(u => u.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);
                    
                entity.Property(u => u.GoogleId)
                    .HasMaxLength(100);
                    
                entity.Property(u => u.FotoUrl)
                    .HasMaxLength(500);
                    
                entity.Property(u => u.Telefono)
                    .HasMaxLength(20);
                    
                entity.Property(u => u.Rol)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("cliente");
                
                // Índices únicos
                entity.HasIndex(u => u.Email)
                    .IsUnique();
                    
                entity.HasIndex(u => u.GoogleId)
                    .IsUnique();
                    
                // Configuración de fechas
                entity.Property(u => u.FechaRegistro)
                    .HasDefaultValueSql("NOW()");
            });
        }
    }
}