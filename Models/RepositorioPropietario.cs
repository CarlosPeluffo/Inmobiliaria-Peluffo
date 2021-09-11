using Microsoft.Extensions.Configuration;
using System.Data;
using System;
using System.Reflection.Metadata;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Inmobiliaria_Peluffo.Models
{
    public class RepositorioPropietario : Base, IRepositorioPropietario
    {
        public RepositorioPropietario(IConfiguration configuration) : base(configuration)
        {
            
        }
        public IList<Propietario> ObtenerTodos(){
            IList<Propietario> res = new List<Propietario>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT id_propietario, apellido, nombre, dni, mail, telefono
                FROM propietarios";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while(reader.Read()){
                        var p = new Propietario{
                            Id = reader.GetInt32(0),
                            Apellido = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Mail = reader[nameof(Propietario.Mail)].ToString(),
                            Telefono = reader.GetString(5),
                        };
                        res.Add(p);
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public int Alta(Propietario p){
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString) )
            {
                string sql = @"INSERT INTO propietarios (apellido, nombre, dni, mail, telefono)
                VALUES(@apellido, @nombre, @dni, @mail, @telefono);
                SELECT last_insert_id();";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@apellido", p.Apellido);
                    comm.Parameters.AddWithValue("@nombre", p.Nombre);
                    comm.Parameters.AddWithValue("@dni", p.Dni);
                    comm.Parameters.AddWithValue("@mail", p.Mail);
                    comm.Parameters.AddWithValue("@telefono", p.Telefono);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    p.Id = res;
                }
            }
            return res;
        }
        public int Baja(Propietario p){
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"DELETE FROM propietarios
                WHERE id_propietario = @id";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", p.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificacion(Propietario p){
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE propietarios 
                SET apellido=@apellido, nombre=@nombre, dni=@dni, mail=@mail, telefono=@telefono
                WHERE id_propietario=@id";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@apellido", p.Apellido);
                    comm.Parameters.AddWithValue("@nombre", p.Nombre);
                    comm.Parameters.AddWithValue("@dni", p.Dni);
                    comm.Parameters.AddWithValue("@mail", p.Mail);
                    comm.Parameters.AddWithValue("@telefono", p.Telefono);
                    comm.Parameters.AddWithValue("@id", p.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public Propietario ObtenerPorId(int id){
            Propietario p = null;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT id_propietario, apellido, nombre, dni, mail, telefono
                FROM propietarios
                WHERE id_propietario = @id";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if(reader.Read()){
                        p = new Propietario{
                            Id = reader.GetInt32(0),
                            Apellido = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Mail = reader[nameof(Propietario.Mail)].ToString(),
                            Telefono = reader.GetString(5),
                        };
                    }
                    conn.Close();
                }
            }
            return p;
        }
    }
}