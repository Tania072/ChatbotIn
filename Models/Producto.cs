using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prueba2.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        public string? DescripcionCorta { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrecioDescuento { get; set; }

        public bool EnPromocion { get; set; } = false;

        [Required]
        public int Stock { get; set; } = 0;

        public string? ImagenUrl { get; set; }

        [Required]
        public string Categoria { get; set; } = string.Empty;

        public string? Etiquetas { get; set; }

        public bool Activo { get; set; } = true;

        public bool Destacado { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Relación con vendedor
        public int VendedorId { get; set; }
        public Usuario? Vendedor { get; set; }

        // Relación con carrito
        public virtual ICollection<CarritoItem> CarritoItems { get; set; } = new List<CarritoItem>();

        // Propiedades calculadas
        [NotMapped]
        public decimal PrecioFinal => EnPromocion && PrecioDescuento.HasValue ? PrecioDescuento.Value : Precio;

        [NotMapped]
        public decimal PorcentajeDescuento => 
            PrecioDescuento.HasValue && Precio > 0 ? ((Precio - PrecioDescuento.Value) / Precio) * 100 : 0;
    }
}