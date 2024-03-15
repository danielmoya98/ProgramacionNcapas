using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CapaDatos;

namespace ModeloNegocio
{
    public class Controller
    {
        private static readonly Coneccion conexion = Coneccion.Instance;

        // Función para obtener todos los usuarios
        public static List<Usuarios> ObtenerTodosLosUsuarios()
        {
            List<CapaDatos.Usuarios> listaUsuarios = new List<CapaDatos.Usuarios>();

            try
            {
                using (var conn = conexion.GetConnection())
                {
                    conn.Open();
                    // Aquí deberías ejecutar tu consulta SQL para obtener todos los usuarios de la base de datos
                    // Por ejemplo:
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Usuarios", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Recorrer los resultados y agregar cada usuario a la lista
                    while (reader.Read())
                    {
                        Usuarios usuario = new Usuarios
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Nombre = reader["Nombre"].ToString(),
                            Rol = reader["Rol"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString()
                        };
                        listaUsuarios.Add(usuario);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuarios: {ex.Message}");
            }

            return listaUsuarios;
        }
    }
}