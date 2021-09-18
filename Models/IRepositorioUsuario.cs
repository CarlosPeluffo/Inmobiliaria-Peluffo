namespace Inmobiliaria_Peluffo.Models
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        Usuario ObtenerPorMail(string mail);
        int CambiarContraseña(Usuario usuario);
    }
}