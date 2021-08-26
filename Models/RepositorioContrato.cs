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
                string sql =@"";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;

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
                string sql =@"";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    
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
                string sql =@"";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    
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
                string sql =@"";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    
                }
            }
            return lista;
        }
        public Contrato ObtenerPorId(){
            Contrato c = null;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql =@"";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    
                }
            }
            return c;
        }
    }
}