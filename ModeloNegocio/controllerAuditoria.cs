using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CapaDatos;

namespace ModeloNegocio
{
    public class controllerAuditoria
    {
        private static readonly Coneccion conexion = Coneccion.Instance;

        public static List<Auditoria_Usuarios> ObtenerTodasLasAuditorias()
        {
            List<Auditoria_Usuarios> auditorias = new List<Auditoria_Usuarios>();

            try
            {
                // Crear la conexión a la base de datos
                using (SqlConnection conn = CapaDatos.Coneccion.Instance.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM AuditoriaUsuarios", conn);

                    // Ejecutar la consulta SQL y leer los resultados
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // Por cada registro de auditoría encontrado, crear un objeto AuditoriaUsuarios y asignar los valores
                        Auditoria_Usuarios auditoria = new Auditoria_Usuarios
                        {
                            //IDAuditoria = (int)reader["IDAuditoria"],
                            Operacion = (string)reader["Operacion"],
                            UsuarioID = (int)reader["UsuarioID"],
                            FechaOperacion = (DateTime)reader["FechaOperacion"],
                            CampoAfectado = (string)reader["CampoAfectado"],
                            DatoAnterior = (string)reader["DatoAnterior"],
                            NuevoDato = (string)reader["NuevoDato"]
                            // Si hay más campos en tu tabla AuditoriaUsuarios, asegúrate de asignarlos aquí
                        };

                        // Agregar la auditoría a la lista de auditorías
                        auditorias.Add(auditoria);
                    }

                    // Cerrar el lector
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                Console.WriteLine($"Error al obtener todas las auditorías: {ex.Message}");
            }

            return auditorias;
        }

    }
}
