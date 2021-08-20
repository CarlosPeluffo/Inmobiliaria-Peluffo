using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Inmobiliaria_Peluffo.Models
{
    public class RepositorioInquilino
    {
        public string ConnectionString = "server=localhost;user=root;password=;database=inmobiliaria;SslMode=none";
        public RepositorioInquilino()
        {
            
        }
        public IList<Inquilino> ObtenerTodos(){
            IList<Inquilino> res = new List<Inquilino>();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
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
                            id = reader.GetInt32(0),
                            apellido = reader.GetString(1),
                            nombre = reader.GetString(2),
                            dni = reader.GetString(3),
                            mail = reader[nameof(Inquilino.mail)].ToString(),
                            telefono = reader.GetString(5),
                            lugarDeTrabajo = reader.GetString(6),
                            dniGarante = reader.GetString(7),
                            nombreGarante = reader.GetString(8),
                            telefonoGarante = reader.GetString(9),
                            mailGarante = reader["mail_garante"].ToString(),
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
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                string sql = @"INSERT INTO inquilinos(apellido, nombre, dni, mail, telefono,
                lugar_trabajo, dni_garante, nombre_garante, telefono_garante, mail_garante)
                VALUES(@apellido, @nombre, @dni, @mail, @telefono, @lugar_trabajo, 
                @dni_garante, @nombre_garante, @telefono_garante, @mail_garante);
                SELECT last_insert_id();";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@apellido", i.apellido);
                    comm.Parameters.AddWithValue("@nombre", i.nombre);
                    comm.Parameters.AddWithValue("@dni", i.dni);
                    comm.Parameters.AddWithValue("@mail", i.mail);
                    comm.Parameters.AddWithValue("@telefono", i.telefono);
                    comm.Parameters.AddWithValue("@lugar_trabajo", i.lugarDeTrabajo);
                    comm.Parameters.AddWithValue("@dni_garante", i.dniGarante);
                    comm.Parameters.AddWithValue("@nombre_garante", i.nombreGarante);
                    comm.Parameters.AddWithValue("@telefono_garante", i.telefonoGarante);
                    comm.Parameters.AddWithValue("@mail_garante", i.mailGarante);

                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    i.id = res;
                }
            }
            return res;
        }
        public int Baja(Inquilino i){
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                string sql= @"DELETE FROM inquilinos
                WHERE id_inquilino = @id";
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", i.id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificar(Inquilino i){
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
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
                    comm.Parameters.AddWithValue("@apellido", i.apellido);
                    comm.Parameters.AddWithValue("@nombre", i.nombre);
                    comm.Parameters.AddWithValue("@dni", i.dni);
                    comm.Parameters.AddWithValue("@mail", i.mail);
                    comm.Parameters.AddWithValue("@telefono", i.telefono);
                    comm.Parameters.AddWithValue("@lugar_trabajo",i.lugarDeTrabajo);
                    comm.Parameters.AddWithValue("@dni_garante",i.dniGarante);
                    comm.Parameters.AddWithValue("@nombre_garante",i.nombreGarante);
                    comm.Parameters.AddWithValue("@telefono_garante",i.telefonoGarante);
                    comm.Parameters.AddWithValue("@mail_garante",i.mailGarante);
                    comm.Parameters.AddWithValue("@id", i.id);

                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public Inquilino ObtenerPorId(int id){
            Inquilino i = null;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
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
                            id= reader.GetInt32(0),
                            apellido = reader.GetString(1),
                            nombre = reader.GetString(2),
                            dni = reader.GetString(3),
                            mail = reader[nameof(Inquilino.mail)].ToString(),
                            telefono = reader.GetString(5),
                            lugarDeTrabajo = reader.GetString(6),
                            dniGarante = reader.GetString(7),
                            nombreGarante = reader.GetString(8),
                            telefonoGarante = reader.GetString(9),
                            mailGarante = reader["mail_garante"].ToString(),
                        };
                    }
                    conn.Close();
                }
            }
            return i;
        }
    }
}