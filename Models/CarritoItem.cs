using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prueba2.Models
{
    public class CarritoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; } = 1;

        public DateTime FechaAgregado { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public decimal Subtotal => (Producto?.Precio ?? 0) * Cantidad;
    }
}