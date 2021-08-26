using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using System;

namespace Inmobiliaria_Peluffo.Models
{
    public abstract class Base
    {
        protected readonly IConfiguration configuration; 
        protected readonly string connectionString;
        protected Base(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }
    }
}