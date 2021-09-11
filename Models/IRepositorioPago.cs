using System.Collections.Generic;
namespace Inmobiliaria_Peluffo.Models
{
    public interface IRepositorioPago : IRepositorio<Pago>
    {
        IList<Pago> ObtenerTodosPorContrato(int id);
    }
}