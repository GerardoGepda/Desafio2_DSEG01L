using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BO200360_PD200491_Desafio2.Models
{
    public class Cv
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string FormacionAcademica { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string ExperienciaProfesional { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ReferenciasPersonales { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Idiomas { get; set; } = string.Empty;

        [MaxLength(200)]
        public string CamposActualizados { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public bool Estado { get; set; }

        // Clave foránea para el candidato
        [Required]
        [ForeignKey("Candidato")]
        public int CandidatoId { get; set; }

    }
}
