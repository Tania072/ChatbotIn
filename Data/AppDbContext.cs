using Microsoft.EntityFrameworkCore;
using Prueba2.Models;

namespace Prueba2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<CarritoItem> CarritoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
                entity.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(u => u.GoogleId).HasMaxLength(100);
                entity.Property(u => u.FotoUrl).HasMaxLength(500);
                entity.Property(u => u.Telefono).HasMaxLength(20);
                entity.Property(u => u.Rol).IsRequired().HasMaxLength(20).HasDefaultValue("cliente");
                entity.Property(u => u.FechaRegistro).HasDefaultValueSql("NOW()");

                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.GoogleId).IsUnique();
            });

            // 🔥 **CONFIGURACIÓN ACTUALIZADA: Producto con campos para chatbot**
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(p => p.Id);
                
                // Campos básicos
                entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Descripcion).IsRequired().HasMaxLength(500);
                
                // 🔥 NUEVOS CAMPOS PARA CHATBOT
                entity.Property(p => p.DescripcionCorta).HasMaxLength(150);
                entity.Property(p => p.Etiquetas).HasMaxLength(500);
                
                // Precios
                entity.Property(p => p.Precio).HasColumnType("decimal(18,2)");
                entity.Property(p => p.PrecioDescuento).HasColumnType("decimal(18,2)");
                
                // Imagen y categoría
                entity.Property(p => p.ImagenUrl).HasMaxLength(255);
                entity.Property(p => p.Categoria).IsRequired().HasMaxLength(50);
                
                // Fechas
                entity.Property(p => p.FechaCreacion).HasDefaultValueSql("NOW()");
                entity.Property(p => p.FechaActualizacion).HasDefaultValueSql("NOW()");
                
                // Valores por defecto
                entity.Property(p => p.Activo).HasDefaultValue(true);
                entity.Property(p => p.EnPromocion).HasDefaultValue(false);
                entity.Property(p => p.Destacado).HasDefaultValue(false);
                entity.Property(p => p.Stock).HasDefaultValue(0);

                // Relación con Vendedor
                entity.HasOne(p => p.Vendedor)
                    .WithMany()
                    .HasForeignKey(p => p.VendedorId)
                    .OnDelete(DeleteBehavior.Restrict);

                // 🔥 NUEVOS ÍNDICES PARA BÚSQUEDAS EN CHATBOT
                entity.HasIndex(p => p.Categoria);
                entity.HasIndex(p => p.Activo);
                entity.HasIndex(p => p.VendedorId);
                entity.HasIndex(p => p.EnPromocion); // Para encontrar promociones rápido
                entity.HasIndex(p => p.Destacado);   // Para productos destacados
                
                // Índice para búsqueda por texto (chatbot)
                entity.HasIndex(p => new { p.Nombre, p.DescripcionCorta, p.Etiquetas });
            });

            // Configuración: CarritoItem
            modelBuilder.Entity<CarritoItem>(entity =>
            {
                entity.HasKey(ci => ci.Id);

                // Relación con Producto
                entity.HasOne(ci => ci.Producto)
                    .WithMany(p => p.CarritoItems)
                    .HasForeignKey(ci => ci.ProductoId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación con Usuario
                entity.HasOne(ci => ci.Usuario)
                    .WithMany()
                    .HasForeignKey(ci => ci.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Índice único para evitar duplicados en carrito
                entity.HasIndex(ci => new { ci.UsuarioId, ci.ProductoId }).IsUnique();
                
                // Valor por defecto para fecha
                entity.Property(ci => ci.FechaAgregado).HasDefaultValueSql("NOW()");
            });
        }
    }
}