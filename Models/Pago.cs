using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Inmobiliaria_Peluffo.Models
{
    public class Pago
    {
        [Column("id_pago")]
        [Display(Name = "ID.")]
        public int Id { get; set; }

        [Required]
        [Range(0, 999)]
        [Display(Name = "Nro. Pago")]
        [Column("nro_pago")]
        public int NroPago { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Pago")]
        [Column("fecha_pago")]
        public DateTime FechaPago { get; set; }

        [Required]
        [Range(0.01, 9999999.99)]
        public Double Monto { get; set; }

        [Required]
        [Display(Name = "Contrato")]
        [Column("id_contrato")]
        public int ContratoId { get; set; }
        [ForeignKey(nameof(ContratoId))]
        public Contrato Contrato { get; set; }
    }
}