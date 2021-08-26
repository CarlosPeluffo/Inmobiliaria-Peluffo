using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Inmobiliaria_Peluffo.Models
{
    public class RepositorioInquilino : Base
    {
        public RepositorioInquilino(IConfiguration configuration) : base(configuration)
        {
            
        }
        public IList<Inquilino> ObtenerTodos(){
            IList<Inquilino> res = new List<Inquilino>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT id_inquilino, apellido, nombre, dni, mail, telefono,
                lugar_trabajo, dni_garante, nombre_garante, telefono_garante, mail_garante 
                FROM inquilinos";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while(reader.Read()){
                        var i = new Inquilino(){
                            Id = reader.GetInt32(0),
                            Apellido = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Mail = reader[nameof(Inquilino.Mail)].ToString(),
                            Telefono = reader.GetString(5),
                            LugarDeTrabajo = reader.GetString(6),
                            DniGarante = reader.GetString(7),
                            NombreGarante = reader.GetString(8),
                            TelefonoGarante = reader.GetString(9),
                            MailGarante = reader["mail_garante"].ToString(),
                        };
                        res.Add(i);
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public int Alta(Inquilino i){
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO inquilinos(apellido, nombre, dni, mail, telefono,
                lugar_trabajo, dni_garante, nombre_garante, telefono_garante, mail_garante)
                VALUES(@apellido, @nombre, @dni, @mail, @telefono, @lugar_trabajo, 
                @dni_garante, @nombre_garante, @telefono_garante, @mail_garante);
                SELECT last_insert_id();";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@apellido", i.Apellido);
                    comm.Parameters.AddWithValue("@nombre", i.Nombre);
                    comm.Parameters.AddWithValue("@dni", i.Dni);
                    comm.Parameters.AddWithValue("@mail", i.Mail);
                    comm.Parameters.AddWithValue("@telefono", i.Telefono);
                    comm.Parameters.AddWithValue("@lugar_trabajo", i.LugarDeTrabajo);
                    comm.Parameters.AddWithValue("@dni_garante", i.DniGarante);
                    comm.Parameters.AddWithValue("@nombre_garante", i.NombreGarante);
                    comm.Parameters.AddWithValue("@telefono_garante", i.TelefonoGarante);
                    comm.Parameters.AddWithValue("@mail_garante", i.MailGarante);

                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    i.Id = res;
                }
            }
            return res;
        }
        public int Baja(Inquilino i){
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql= @"DELETE FROM inquilinos
                WHERE id_inquilino = @id";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", i.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificacion(Inquilino i){
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE inquilinos 
                SET apellido=@apellido, nombre=@nombre, dni=@dni, mail=@mail, telefono=@telefono,
                lugar_trabajo=@lugar_trabajo, dni_garante=@dni_garante,
                nombre_garante=@nombre_garante, telefono_garante=@telefono_garante,
                mail_garante=@mail_garante
                WHERE id_inquilino = @id";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@apellido", i.Apellido);
                    comm.Parameters.AddWithValue("@nombre", i.Nombre);
                    comm.Parameters.AddWithValue("@dni", i.Dni);
                    comm.Parameters.AddWithValue("@mail", i.Mail);
                    comm.Parameters.AddWithValue("@telefono", i.Telefono);
                    comm.Parameters.AddWithValue("@lugar_trabajo",i.LugarDeTrabajo);
                    comm.Parameters.AddWithValue("@dni_garante",i.DniGarante);
                    comm.Parameters.AddWithValue("@nombre_garante",i.NombreGarante);
                    comm.Parameters.AddWithValue("@telefono_garante",i.TelefonoGarante);
                    comm.Parameters.AddWithValue("@mail_garante",i.MailGarante);
                    comm.Parameters.AddWithValue("@id", i.Id);

                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public Inquilino ObtenerPorId(int id){
            Inquilino i = null;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT id_inquilino, apellido, nombre, dni, mail, telefono,
                lugar_trabajo, dni_garante, nombre_garante, telefono_garante, mail_garante
                FROM inquilinos
                WHERE id_inquilino = @id";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if(reader.Read()){
                        i = new Inquilino{
                            Id= reader.GetInt32(0),
                            Apellido = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Mail = reader[nameof(Inquilino.Mail)].ToString(),
                            Telefono = reader.GetString(5),
                            LugarDeTrabajo = reader.GetString(6),
                            DniGarante = reader.GetString(7),
                            NombreGarante = reader.GetString(8),
                            TelefonoGarante = reader.GetString(9),
                            MailGarante = reader["mail_garante"].ToString(),
                        };
                    }
                    conn.Close();
                }
            }
            return i;
        }
    }
}