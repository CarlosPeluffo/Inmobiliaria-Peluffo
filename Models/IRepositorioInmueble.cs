using System.Collections.Generic;
namespace Inmobiliaria_Peluffo.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        IList<Inmueble> ObtenerLibres(string a, string b);
        IList<Inmueble> ObtenerPorPropietario(int id);
        IList<Inmueble> ObtenerTodosActivos();
    }
}