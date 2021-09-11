using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
namespace Inmobiliaria_Peluffo.Models
{
    public class RepositorioPago : Base, IRepositorioPago
    {
        public RepositorioPago(IConfiguration configuration) : base(configuration)
        {
            
        }
        public int Alta(Pago p){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql=@"INSERT INTO pagos(nro_pago, fecha_pago, monto, id_contrato)
                VALUES (@nro_pago, NOW(), @monto, @contrato);
                SELECT last_insert_id();";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@nro_pago", p.NroPago);
                    comm.Parameters.AddWithValue("@monto", p.Monto);
                    comm.Parameters.AddWithValue("@contrato", p.ContratoId);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    p.Id = res;
                    conn.Close();
                }
            }
            return res;
        }
        public int Baja(Pago p){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql =@"DELETE FROM pagos WHERE id_pago = @id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", p.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificacion(Pago p){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql= @"UPDATE pagos SET monto=@monto WHERE id_pago=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.Parameters.AddWithValue("@monto", p.Monto);
                    comm.Parameters.AddWithValue("@id", p.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public IList<Pago> ObtenerTodos(){
            IList<Pago> lista = new List<Pago>();
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT id_pago, p.nro_pago, p.fecha_pago, p.monto,
                    p.id_contrato, c.monto, c.id_inquilino, c.id_inmueble,
                    i.nombre, i.apellido, i.dni, inm.direccion, inm.tipo 
                    FROM pagos p 
                    INNER JOIN contratos c ON c.id_contrato = p.id_contrato 
                    INNER JOIN inquilinos i ON i.id_inquilino = c.id_inquilino 
                    INNER JOIN inmuebles inm ON inm.id_inmueble = c.id_inmueble";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while(reader.Read()){
                        Pago p = new Pago{
                            Id = reader.GetInt32(0),
                            NroPago = reader.GetInt32(1),
                            FechaPago = reader.GetDateTime(2),
                            Monto = reader.GetDouble(3),
                            ContratoId = reader.GetInt32(4),
                            Contrato = new Contrato{
                                Id = reader.GetInt32(4),
                                Monto = reader.GetDouble(5),
                                InquilinoId = reader.GetInt32(6),
                                Inquilino = new Inquilino{
                                    Id = reader.GetInt32(6),
                                    Nombre = reader.GetString(8),
                                    Apellido = reader.GetString(9),
                                    Dni = reader.GetString(10)
                                },
                                InmuebleId = reader.GetInt32(7),
                                Inmueble = new Inmueble{
                                    Direccion = reader.GetString(11),
                                    Tipo = reader.GetString(12),
                                },
                            },
                        };
                        lista.Add(p);
                    }
                    conn.Close();
                }
            }
            return lista;
        }
        public Pago ObtenerPorId(int id){
            Pago p = null;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT id_pago, p.nro_pago, p.fecha_pago, p.monto,
                    p.id_contrato, c.monto, c.id_inquilino, c.id_inmueble,
                    i.nombre, i.apellido, i.dni, inm.direccion, inm.tipo 
                    FROM pagos p 
                    INNER JOIN contratos c ON c.id_contrato = p.id_contrato 
                    INNER JOIN inquilinos i ON i.id_inquilino = c.id_inquilino 
                    INNER JOIN inmuebles inm ON inm.id_inmueble = c.id_inmueble
                    WHERE id_pago=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if(reader.Read()){
                        p = new Pago{
                            Id = reader.GetInt32(0),
                            NroPago = reader.GetInt32(1),
                            FechaPago = reader.GetDateTime(2),
                            Monto = reader.GetDouble(3),
                            ContratoId = reader.GetInt32(4),
                            Contrato = new Contrato{
                                Id = reader.GetInt32(4),
                                Monto = reader.GetDouble(5),
                                InquilinoId = reader.GetInt32(6),
                                Inquilino = new Inquilino{
                                    Id = reader.GetInt32(6),
                                    Nombre = reader.GetString(8),
                                    Apellido = reader.GetString(9),
                                    Dni = reader.GetString(10)
                                },
                                InmuebleId = reader.GetInt32(7),
                                Inmueble = new Inmueble{
                                    Direccion = reader.GetString(11),
                                    Tipo = reader.GetString(12),
                                },
                            },
                        };
                    }
                    conn.Close();
                }

            }
            return p;
        }
        public IList<Pago> ObtenerTodosPorContrato(int id){
            IList<Pago> lista = new List<Pago>();
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT id_pago, p.nro_pago, p.fecha_pago, p.monto,
                    p.id_contrato, c.monto, c.id_inquilino, c.id_inmueble,
                    i.nombre, i.apellido, i.dni, inm.direccion, inm.tipo 
                    FROM pagos p 
                    INNER JOIN contratos c ON c.id_contrato = p.id_contrato 
                    INNER JOIN inquilinos i ON i.id_inquilino = c.id_inquilino 
                    INNER JOIN inmuebles inm ON inm.id_inmueble = c.id_inmueble
                    WHERE p.id_contrato=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while(reader.Read()){
                        Pago p = new Pago{
                            Id = reader.GetInt32(0),
                            NroPago = reader.GetInt32(1),
                            FechaPago = reader.GetDateTime(2),
                            Monto = reader.GetDouble(3),
                            ContratoId = reader.GetInt32(4),
                            Contrato = new Contrato{
                                Id = reader.GetInt32(4),
                                Monto = reader.GetDouble(5),
                                InquilinoId = reader.GetInt32(6),
                                Inquilino = new Inquilino{
                                    Id = reader.GetInt32(6),
                                    Nombre = reader.GetString(8),
                                    Apellido = reader.GetString(9),
                                    Dni = reader.GetString(10)
                                },
                                InmuebleId = reader.GetInt32(7),
                                Inmueble = new Inmueble{
                                    Direccion = reader.GetString(11),
                                    Tipo = reader.GetString(12),
                                },
                            },
                        };
                        lista.Add(p);
                    }
                    conn.Close();
                }
            }
            return lista;
        }
    }
}