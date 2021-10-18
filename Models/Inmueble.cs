using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria_Peluffo.Models
{
    public class Inmueble
    {
        [Column ("id_inmueble")]
        [Display(Name = "ID.")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        [Required]
        public string Uso { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        [Column ("cant_ambientes")]
        [Display(Name = "Cantidad de ambientes")]
        public int Ambientes { get; set; }
        [Required]
        public double Precio { get; set; }
        [Required]
        public bool Estado { get; set; }
        [Required]
        [Display(Name = "Propietario")]
        [Column ("id_propietario")]
        public int PropietarioId { get; set; }
        [ForeignKey(nameof(PropietarioId))]
        public Propietario Propietario { get; set; }
    }
}