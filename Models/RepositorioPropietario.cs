using System.Data;
using System;
using System.Reflection.Metadata;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Inmobiliaria_Peluffo.Models
{
    public class RepositorioPropietario
    {
        public string ConnectionString = "server=localhost;user=root;password=;database=inmobiliaria;SslMode=none";
        public RepositorioPropietario()
        {
            
        }
        public IList<Propietario> obtenerTodos(){
            IList<Propietario> res = new List<Propietario>();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
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
                            id = reader.GetInt32(0),
                            apellido = reader.GetString(1),
                            nombre = reader.GetString(2),
                            dni = reader.GetString(3),
                            mail = reader[nameof(Propietario.mail)].ToString(),
                            telefono = reader.GetString(5),
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
            using (MySqlConnection conn = new MySqlConnection(ConnectionString) )
            {
                string sql = @"INSERT INTO propietarios (apellido, nombre, dni, mail, telefono)
                VALUES(@apellido, @nombre, @dni, @mail, @telefono);
                SELECT last_insert_id();";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@apellido", p.apellido);
                    comm.Parameters.AddWithValue("@nombre", p.nombre);
                    comm.Parameters.AddWithValue("@dni", p.dni);
                    comm.Parameters.AddWithValue("@mail", p.mail);
                    comm.Parameters.AddWithValue("@telefono", p.telefono);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    p.id = res;
                }
            }
            return res;
        }
        public int Baja(Propietario p){
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                string sql = @"DELETE FROM propietarios
                WHERE id_propietario = @id";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", p.id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificacion(Propietario p){
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                string sql = @"UPDATE propietarios 
                SET apellido=@apellido, nombre=@nombre, dni=@dni, mail=@mail, telefono=@telefono
                WHERE id_propietario=@id";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@apellido", p.apellido);
                    comm.Parameters.AddWithValue("@nombre", p.nombre);
                    comm.Parameters.AddWithValue("@dni", p.dni);
                    comm.Parameters.AddWithValue("@mail", p.mail);
                    comm.Parameters.AddWithValue("@telefono", p.telefono);
                    comm.Parameters.AddWithValue("@id", p.id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public Propietario ObtenerPorId(int id){
            Propietario p = null;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
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
                            id = reader.GetInt32(0),
                            apellido = reader.GetString(1),
                            nombre = reader.GetString(2),
                            dni = reader.GetString(3),
                            mail = reader[nameof(Propietario.mail)].ToString(),
                            telefono = reader.GetString(5),
                        };
                    }
                    conn.Close();
                }
            }
            return p;
        }
    }
}