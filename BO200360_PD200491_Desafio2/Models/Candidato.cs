using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BO200360_PD200491_Desafio2.Models
{
    public class Candidato
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Apellidos { get; set; } = string.Empty;

        [Required]
        [Phone]
        [MaxLength(8)]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Contrasena { get; set; } = string.Empty;

        // Relación uno a muchos: Un candidato puede tener muchas hojas de vida
        public ICollection<Cv> Cvs { get; set; } = new List<Cv>();
    }
}
