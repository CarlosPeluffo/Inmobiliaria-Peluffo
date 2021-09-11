using System.ComponentModel.Design;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Inmobiliaria_Peluffo.Models
{
    public class RepositorioContrato : Base, IRepositorioContrato
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {
            
        }
        public int Alta(Contrato c){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql =@"INSERT INTO contratos(fecha_inicio, fecha_fin, monto,
                cancelado, fecha_cancelado
                id_inquilino, id_inmueble)
                VALUES(@inicio, @fin, @monto, false, NULL , @inquilino, @inmueble);
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
                    cancelado=@cancelado, fecha_cancelado=@cancelar,
                    id_inquilino=@inquilino, id_inmueble=@inmueble
                    WHERE id_contrato=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@inicio",c.FechaInicio);
                    comm.Parameters.AddWithValue("@fin",c.FechaFin);
                    comm.Parameters.AddWithValue("@monto",c.Monto);
                    comm.Parameters.AddWithValue("@cancelado", c.Cancelado);
                    comm.Parameters.AddWithValue("@cancelar", c.FechaCancelado);
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
                    cancelado, fecha_cancelado,
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
                          Cancelado = reader.GetBoolean(4),
                          FechaCancelado = reader["fecha_cancelado"] != DBNull.Value ? reader.GetDateTime(5) : null,
                          InquilinoId = reader.GetInt32(6),
                          InmuebleId = reader.GetInt32(7),
                          Inquilino = new Inquilino{
                              Id = reader.GetInt32(6),
                              Dni = reader.GetString(8),
                              Apellido = reader.GetString(9),
                              Nombre = reader.GetString(10)
                          },
                          Inmueble = new Inmueble{
                              Id = reader.GetInt32(7),
                              Direccion = reader.GetString(11),
                              Uso = reader.GetString(12),
                              Tipo = reader.GetString(13)
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
                    cancelado, fecha_cancelado,
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
                          Cancelado = reader.GetBoolean(4),
                          FechaCancelado = reader["fecha_cancelado"] != DBNull.Value ? reader.GetDateTime(5) : null,
                          InquilinoId = reader.GetInt32(6),
                          InmuebleId = reader.GetInt32(7),
                          Inquilino = new Inquilino{
                              Id = reader.GetInt32(6),
                              Dni = reader.GetString(8),
                              Apellido = reader.GetString(9),
                              Nombre = reader.GetString(10)
                          },
                          Inmueble = new Inmueble{
                              Id = reader.GetInt32(7),
                              Direccion = reader.GetString(11),
                              Uso = reader.GetString(12),
                              Tipo = reader.GetString(13)
                          },
                        };
                    }
                    conn.Close();
                }
            }
            return c;
        }
        public int CancelarContrato(Contrato c){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql =@"UPDATE contratos 
                    SET cancelado=true, fecha_cancelado = NOW()
                    WHERE id_contrato=@id";
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
    }
}