using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Inmobiliaria_Peluffo.Models
{
    public class Contrato
    {
        [Display(Name = "ID.")]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Fin")]
        public DateTime FechaFin { get; set;}
        [Required]
        [Display(Name = "Monto")]
        public double Monto { get; set; }
        [Required]
        [Display(Name = "Inquilino")]
        public int InquilinoId { get; set; }
        [ForeignKey(nameof(InquilinoId))]
        public Inquilino Inquilino { get; set; }
        [Required]
        [Display(Name = "Inmueble")]
        public int InmuebleId { get; set; }
        [ForeignKey(nameof(InmuebleId))]
        public Inmueble Inmueble { get; set; }
    }
}