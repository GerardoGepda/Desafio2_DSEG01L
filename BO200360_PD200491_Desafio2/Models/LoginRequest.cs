using System.ComponentModel.DataAnnotations;

namespace BO200360_PD200491_Desafio2.Models
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Contrasena { get; set; }
    }
}
