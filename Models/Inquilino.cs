using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Inmobiliaria_Peluffo.Models
{
    public class Inquilino
    {
        [Column ("id_inquilino")]
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
        [Required]
        [Display(Name = "Lugar de Trabajo")]
        [Column ("lugar_trabajo")]
        public string LugarDeTrabajo { get; set; }
        [Required]
        [Display(Name = "D.N.I. del Garante")]
        [Column ("dni_garante")]
        public string DniGarante { get; set; }
        [Required]
        [Display(Name = "Nombre Completo del Garante")]
        [Column ("nombre_garante")]
        public string NombreGarante { get; set; }
        [Required, Phone]
        [Display(Name = "Teléfono del Garante")]
        [Column ("telefono_garante")]
        public string TelefonoGarante { get; set; }
        [EmailAddress]
        [Display(Name = "E-Mail del Garante")]
        [Column ("mail_garante")]
        public string MailGarante { get; set; }
    }
}