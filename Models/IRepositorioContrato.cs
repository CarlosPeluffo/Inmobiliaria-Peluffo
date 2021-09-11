namespace Inmobiliaria_Peluffo.Models
{
    public interface IRepositorioContrato : IRepositorio<Contrato>
    {
        int CancelarContrato(Contrato c);
    }
}