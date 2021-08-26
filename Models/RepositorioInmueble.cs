using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Data;
using MySql.Data.MySqlClient;
namespace Inmobiliaria_Peluffo.Models
{
    public class RepositorioInmueble : Base
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {
            
        }
        public IList<Inmueble> ObtenerTodos(){
            IList<Inmueble> lista = new List<Inmueble>();
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT id_inmueble, i.id_propietario, direccion, uso, tipo, 
                cant_ambientes, precio, estado, prop.nombre, prop.apellido
                FROM inmuebles i JOIN propietarios prop ON i.id_propietario = prop.id_propietario";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while(reader.Read()){
                        Inmueble i = new Inmueble{
                            Id = reader.GetInt32(0),
                            PropietarioId = reader.GetInt32(1),
                            Direccion = reader.GetString(2),
                            Uso = reader.GetString(3),
                            Tipo = reader.GetString(4),
                            Ambientes = reader.GetInt32(5),
                            Precio = reader.GetDouble(6),
                            Estado = reader.GetBoolean(7),
                            Propietario = new Propietario{
                                Id = reader.GetInt32(1),
                                Apellido = reader.GetString(8),
                                Nombre = reader.GetString(9),
                            }

                        };
                        lista.Add(i);
                    }
                    conn.Close();
                }
            }
            return lista;
        }
        public int Alta(Inmueble i){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"INSERT INTO inmuebles (id_propietario, direccion,
                uso, tipo, cant_ambientes, precio, estado)
                VALUES(@id_propietario, @direccion, @uso, @tipo, @cant_ambientes, @precio, 
                @estado);
                SELECT last_insert_id();";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id_propietario", i.PropietarioId);
                    comm.Parameters.AddWithValue("@direccion",i.Direccion);
                    comm.Parameters.AddWithValue("@uso",i.Uso);
                    comm.Parameters.AddWithValue("@tipo",i.Tipo);
                    comm.Parameters.AddWithValue("@cant_ambientes",i.Ambientes);
                    comm.Parameters.AddWithValue("@precio",i.Precio);
                    comm.Parameters.AddWithValue("@estado",i.Estado);
                    
                    conn.Open();
                    res= Convert.ToInt32(comm.ExecuteScalar());
                    i.Id = res;
                    conn.Close();
                }
            }
            return res;
        }
        public int Baja(Inmueble i){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"DELETE FROM inmuebles WHERE id_inmueble=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", i.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificacion(Inmueble i){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"UPDATE inmuebles
                SET id_propietario=@propietario, direccion=@direccion, uso=@uso,
                tipo=@tipo, cant_ambientes=@cant, precio=@precio, estado=@estado
                WHERE id_inmueble=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@propietario", i.PropietarioId);
                    comm.Parameters.AddWithValue("@direccion", i.Direccion);
                    comm.Parameters.AddWithValue("@uso", i.Uso);
                    comm.Parameters.AddWithValue("@tipo", i.Tipo);
                    comm.Parameters.AddWithValue("@cant", i.Ambientes);
                    comm.Parameters.AddWithValue("@precio", i.Precio);
                    comm.Parameters.AddWithValue("@estado", i.Estado);
                    comm.Parameters.AddWithValue("@id", i.Id);

                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public Inmueble ObtenerPorId(int id){
            Inmueble i = null;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT id_inmueble, id_propietario, direccion, uso, tipo,
                cant_ambientes, precio, estado
                FROM inmuebles
                WHERE id_inmueble=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if(reader.Read()){
                        i = new Inmueble{
                            Id = reader.GetInt32(0),
                            PropietarioId = reader.GetInt32(1),
                            Direccion = reader.GetString(2),
                            Uso = reader.GetString(3),
                            Tipo = reader.GetString(4),
                            Ambientes = reader.GetInt32(5),
                            Precio = reader.GetDouble(6),
                            Estado = reader.GetBoolean(7)
                        };
                    }
                    conn.Close();
                }
            }
            return i;
        }
    }
}