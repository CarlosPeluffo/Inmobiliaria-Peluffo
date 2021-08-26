using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria_Peluffo.Models
{
    public class Propietario
    {
        [Display(Name = "ID.")]
        public int Id { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "D.N.I.")]
        public string Dni { get; set; }
        [EmailAddress]
        [Display(Name = "E-Mail")] 
        public string Mail { get; set; }
        [Required, Phone]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }
    }
}