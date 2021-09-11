using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Inmobiliaria_Peluffo.Models
{
    public enum enumeradoRoles{
        Administrador = 1,
        Empleado = 2,
    }
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        [EmailAddress]
        public string Mail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Clave { get; set; }
        public string Avatar { get; set; }
        public IFormFile AvatarFile { get; set; }
        [Required]
        public int Rol { get; set; }
        public string RolNombre => Rol > 0 ? ((enumeradoRoles)Rol).ToString() : "";
        public static IDictionary<int, string> ObtenerRoles(){
            SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
            Type tipoEnumerableRoles = typeof(enumeradoRoles);
            foreach (var item in Enum.GetValues(tipoEnumerableRoles))
            {
                roles.Add((int) item, Enum.GetName(tipoEnumerableRoles, item));
            }
            return roles;
        }
    }
}