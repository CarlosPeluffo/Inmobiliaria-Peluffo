using Microsoft.Extensions.Configuration;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Inmobiliaria_Peluffo.Models
{
    public class RepositorioContrato : Base
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {
            
        }
        public int Alta(Contrato c){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql =@"INSERT INTO contratos(fecha_inicio, fecha_fin, monto,
                id_inquilino, id_inmueble)
                VALUES(@inicio, @fin, @monto, @inquilino, @inmueble);
                SELECT last_insert_id();";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@inicio", c.FechaInicio);
                    comm.Parameters.AddWithValue("@fin", c.FechaFin);
                    comm.Parameters.AddWithValue("@monto", c.Monto);
                    comm.Parameters.AddWithValue("@inquilino", c.InquilinoId);
                    comm.Parameters.AddWithValue("@inmueble", c.InmuebleId);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    c.Id = res;
                    conn.Close();
                }
            }
            return res;
        }
        public int Baja(Contrato c){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql =@"DELETE FROM contratos WHERE id_contrato=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", c.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificacion(Contrato c){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql =@"UPDATE contratos
                SET fecha_inicio=@inicio, fecha_fin=@fin, monto=@monto, 
                id_inquilino=@inquilino, id_inmueble=@inmueble
                WHERE id_contrato=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@inicio",c.FechaInicio);
                    comm.Parameters.AddWithValue("@fin",c.FechaFin);
                    comm.Parameters.AddWithValue("@monto",c.Monto);
                    comm.Parameters.AddWithValue("@inquilino",c.InquilinoId);
                    comm.Parameters.AddWithValue("@inmueble",c.InmuebleId);
                    comm.Parameters.AddWithValue("@id",c.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public IList<Contrato> ObtenerTodos(){
            IList<Contrato> lista = new List<Contrato>();
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql =@"SELECT id_contrato, fecha_inicio, fecha_fin, monto,
                    c.id_inquilino, c.id_inmueble,
                    inq.dni, inq.apellido, inq.nombre,
                    i.direccion, i.uso, i.tipo
                    FROM contratos c 
                    INNER JOIN inmuebles i ON c.id_inmueble = i.id_inmueble
                    INNER JOIN inquilinos inq ON c.id_inquilino = inq.id_inquilino";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while(reader.Read()){
                        Contrato c = new Contrato{
                          Id = reader.GetInt32(0),
                          FechaInicio = reader.GetDateTime(1),
                          FechaFin = reader.GetDateTime(2),
                          Monto = reader.GetDouble(3),
                          InquilinoId = reader.GetInt32(4),
                          InmuebleId = reader.GetInt32(5),
                          Inquilino = new Inquilino{
                              Id = reader.GetInt32(4),
                              Dni = reader.GetString(6),
                              Apellido = reader.GetString(7),
                              Nombre = reader.GetString(8)
                          },
                          Inmueble = new Inmueble{
                              Id = reader.GetInt32(5),
                              Direccion = reader.GetString(9),
                              Uso = reader.GetString(10),
                              Tipo = reader.GetString(11)
                          },
                        };
                        lista.Add(c);
                    }
                    conn.Close();
                }
            }
            return lista;
        }
        public Contrato ObtenerPorId(int id){
            Contrato c = null;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql =@"SELECT id_contrato, fecha_inicio, fecha_fin, monto,
                    c.id_inquilino, c.id_inmueble,
                    inq.dni, inq.apellido, inq.nombre,
                    i.direccion, i.uso, i.tipo
                    FROM contratos c 
                    INNER JOIN inmuebles i ON c.id_inmueble = i.id_inmueble
                    INNER JOIN inquilinos inq ON c.id_inquilino = inq.id_inquilino
                    WHERE id_contrato=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if(reader.Read()){
                        c = new Contrato{
                          Id = reader.GetInt32(0),
                          FechaInicio = reader.GetDateTime(1),
                          FechaFin = reader.GetDateTime(2),
                          Monto = reader.GetDouble(3),
                          InquilinoId = reader.GetInt32(4),
                          InmuebleId = reader.GetInt32(5),
                          Inquilino = new Inquilino{
                              Id = reader.GetInt32(4),
                              Dni = reader.GetString(6),
                              Apellido = reader.GetString(7),
                              Nombre = reader.GetString(8)
                          },
                          Inmueble = new Inmueble{
                              Id = reader.GetInt32(5),
                              Direccion = reader.GetString(9),
                              Uso = reader.GetString(10),
                              Tipo = reader.GetString(11)
                          },
                        };
                    }
                    conn.Close();
                }
            }
            return c;
        }
    }
}