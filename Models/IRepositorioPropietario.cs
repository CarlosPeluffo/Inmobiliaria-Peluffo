namespace Inmobiliaria_Peluffo.Models
{
    public interface IRepositorioPropietario : IRepositorio<Propietario>
    {
        Propietario ObtenerPorMail(string mail);
    }
}