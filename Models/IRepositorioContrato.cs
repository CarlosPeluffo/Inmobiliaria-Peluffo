using System.Collections.Generic;
namespace Inmobiliaria_Peluffo.Models
{
    public interface IRepositorioContrato : IRepositorio<Contrato>
    {
        int CancelarContrato(Contrato c);
        IList<Contrato> ObtenerPorInmueble(int id);
        IList<Contrato> ObtenerVigentesFecha(string a, string b);
        int Informe(int id);
    }
}