using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria_Peluffo.Models
{
    public class Inquilino
    {
        public int Id { get; set; }
        [Display(Name = "ID.")]
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
        [Required]
        [Display(Name = "Lugar de Trabajo")]
        public string LugarDeTrabajo { get; set; }
        [Required]
        [Display(Name = "D.N.I. del Garante")]
        public string DniGarante { get; set; }
        [Required]
        [Display(Name = "Nombre Completo del Garante")]
        public string NombreGarante { get; set; }
        [Required, Phone]
        [Display(Name = "Teléfono del Garante")]
        public string TelefonoGarante { get; set; }
        [EmailAddress]
        [Display(Name = "E-Mail del Garante")]
        public string MailGarante { get; set; }
    }
}