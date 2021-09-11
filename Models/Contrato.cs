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
        [FechaInicio(ErrorMessage = "La Fecha de inicio debe ser mayor a la actual")]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Fin")]
        public DateTime FechaFin { get; set;}
        [Required]
        [Display(Name = "Monto")]
        public double Monto { get; set; }
        public bool Cancelado { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Cancelación")]
        public DateTime? FechaCancelado { get; set; }
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
    public class FechaInicio : ValidationAttribute{
        public override bool IsValid(object value)
        {
            DateTime dt = DateTime.Parse(value.ToString());
            DateTime now = DateTime.Parse(DateTime.Now.ToShortDateString());
            int i = DateTime.Compare(dt, now);
            if(i < 0){
                return false;
            }
            return true;
        }
    }
}