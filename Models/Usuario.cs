using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prueba2.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public string? GoogleId { get; set; }
        
        [Required]
        public string Nombre { get; set; } = string.Empty;
        
        public string? FotoUrl { get; set; }
        
        public string? Telefono { get; set; }
        
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        
        public string Rol { get; set; } = "cliente"; // cliente, vendedor, admin
        
        // Para el chatbot - preferencias del usuario
        public string? Preferencias { get; set; }
        
        public bool NotificacionesActivas { get; set; } = true;
        
        // Método para usuario de Google
        public static Usuario CreateFromGoogle(string email, string nombre, string googleId, string fotoUrl)
        {
            return new Usuario
            {
                Email = email,
                Nombre = nombre,
                GoogleId = googleId,
                FotoUrl = fotoUrl,
                PasswordHash = "GOOGLE_AUTH", // Marcador para usuarios Google
                FechaRegistro = DateTime.UtcNow,
                Rol = "cliente"
            };
        }
    }
}