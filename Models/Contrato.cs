using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Inmobiliaria_Peluffo.Models
{
    public class Contrato
    {
        [Column ("id_contrato")]
        [Display(Name = "ID.")]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Column ("fecha_inicio")]
        [FechaInicio(ErrorMessage = "La Fecha de inicio debe ser mayor a la actual")]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Fin")]
        [Column ("fecha_fin")]
        public DateTime FechaFin { get; set;}
        [Required]
        [Display(Name = "Monto")]
        public double Monto { get; set; }
        public bool Cancelado { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Cancelación")]
        [Column ("fecha_cancelado")]
        public DateTime? FechaCancelado { get; set; }
        [Required]
        [Display(Name = "Inquilino")]
        [Column ("id_inquilino")]
        public int InquilinoId { get; set; }
        [ForeignKey(nameof(InquilinoId))]
        public Inquilino Inquilino { get; set; }
        [Required]
        [Display(Name = "Inmueble")]
        [Column ("id_inmueble")]
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