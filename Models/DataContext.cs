using Microsoft.EntityFrameworkCore;
using System;

namespace Inmobiliaria_Peluffo.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<Propietario> propietarios { get; set; }
        public DbSet<Inquilino> inquilinos { get; set; }
        public DbSet<Inmueble> inmuebles { get; set; }
        public DbSet<Contrato> contratos{ get; set; }
        public DbSet<Pago> pagos{ get; set; }
    }
}