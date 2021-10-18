using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria_Peluffo.Models
{
    public class Propietario
    {
        [Column ("id_propietario")]
        [Display(Name = "ID.")]
        public int Id { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "D.N.I.")]
        public string Dni { get; set; }
        [Required, EmailAddress]
        [Display(Name = "E-Mail")] 
        public string Mail { get; set; }
        [Required, Phone]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }
        [Required, DataType(DataType.Password)]
        public string Clave { get; set; }
        public string Avatar { get; set; }
        [NotMapped]
        public IFormFile AvatarFile { get; set; }
    }
}