using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace Inmobiliaria_Peluffo.Models
{
    public class RepositorioUsuario : Base, IRepositorioUsuario
    {
        public RepositorioUsuario(IConfiguration configuration) : base(configuration)
        {
            
        }
        public int Alta(Usuario user){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"INSERT INTO usuarios(nombre, apellido, mail, 
                clave, avatar, rol)
                VALUES(@nombre, @apellido, @mail, @clave, @avatar, @rol);
                SELECT last_insert_id();";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@nombre", user.Nombre);
                    comm.Parameters.AddWithValue("@apellido", user.Apellido);
                    comm.Parameters.AddWithValue("@mail", user.Mail);
                    comm.Parameters.AddWithValue("@clave", user.Clave);
                    if (String.IsNullOrEmpty(user.Avatar)){
                        comm.Parameters.AddWithValue("@avatar", DBNull.Value);
                    }else{
                        comm.Parameters.AddWithValue("@avatar", user.Avatar);
                    }
                    comm.Parameters.AddWithValue("@rol", user.Rol);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    user.Id = res;
                }
            }
            return res;
        }
        public int Baja(Usuario user){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"DELETE usuarios WHERE id_usuario=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", user.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificacion(Usuario user){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"UPDATE usuarios
                SET nombre=@nombre, apellido=@apellido, mail=@mail, clave=@clave, 
                avatar=@avatar, rol=@rol
                WHERE id_usuario=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@nombre", user.Nombre);
                    comm.Parameters.AddWithValue("@apellido", user.Apellido);
                    comm.Parameters.AddWithValue("@mail", user.Mail);
                    comm.Parameters.AddWithValue("@clave", user.Clave);
                    comm.Parameters.AddWithValue("@avatar", user.Avatar);
                    comm.Parameters.AddWithValue("@rol", user.Rol);
                    comm.Parameters.AddWithValue("@id", user.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public IList<Usuario> ObtenerTodos(){
            IList<Usuario> lista = new List<Usuario>();
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT id_usuario, nombre, apellido, mail, avatar, rol
                FROM usuarios";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while(reader.Read()){
                        var user = new Usuario{
                            Id = reader.GetInt32(0),
                            Apellido = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            Mail = reader.GetString(3),
                            Avatar = reader[nameof(Usuario.Avatar)] == DBNull.Value ? null : reader.GetString(4),
                            Rol = reader.GetInt32(5)
                        };
                        lista.Add(user);
                    }
                }
            }
            return lista;
        }
        public Usuario ObtenerPorId(int id){
            Usuario user = null;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT id_usuario, nombre, apellido, mail, clave, avatar, rol
                FROM usuarios
                WHERE id_usuario=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if(reader.Read()){
                        user = new Usuario{
                            Id = reader.GetInt32(0),
                            Apellido = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            Mail = reader.GetString(3),
                            Clave = reader.GetString(4),
                            Avatar = reader[nameof(Usuario.Avatar)] == DBNull.Value ? null : reader.GetString(5),
                            Rol = reader.GetInt32(6)
                        };
                    }
                }
            }
            return user;
        }
        public Usuario ObtenerPorMail(string mail){
            Usuario user = null;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT id_usuario, nombre, apellido, mail, clave, avatar, rol
                FROM usuarios
                WHERE mail=@mail";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.Parameters.Add("@mail", MySqlDbType.VarChar).Value = mail;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if(reader.Read()){
                        user = new Usuario{
                            Id = reader.GetInt32(0),
                            Apellido = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            Mail = reader.GetString(3),
                            Clave = reader.GetString(4),
                            Avatar = reader[nameof(Usuario.Avatar)] == DBNull.Value ? null : reader.GetString(5),
                            Rol = reader.GetInt32(6)
                        };
                    }
                }
            }
            return user;
        }
    }
}