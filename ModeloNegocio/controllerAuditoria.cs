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
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Por cada registro de auditoría encontrado, crear un objeto AuditoriaUsuarios y asignar los valores
                            Auditoria_Usuarios auditoria = new Auditoria_Usuarios
                            {
                                IDAuditoria = (int)reader["IDAuditoria"],
                                Operacion = reader["Operacion"] as string,
                                UsuarioID = (int)reader["UsuarioID"],
                                FechaOperacion = (DateTime)reader["FechaOperacion"],
                                CampoAfectado = reader["CampoAfectado"] as string,
                                DatoAnterior = reader["DatoAnterior"] as string,
                                NuevoDato = reader["NuevoDato"] as string
                                // Si hay más campos en tu tabla AuditoriaUsuarios, asegúrate de asignarlos aquí
                            };

                            // Agregar la auditoría a la lista de auditorías
                            auditorias.Add(auditoria);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                throw new Exception($"Error al obtener todas las auditorías: {ex.Message}", ex);
            }

            return auditorias;
        }
        public static List<Auditoria_Usuarios_Sesion> ObtenerTodasLasAuditoriasSesion()
        {
            List<Auditoria_Usuarios_Sesion> auditorias2 = new List<Auditoria_Usuarios_Sesion>();

            try
            {
                // Crear la conexión a la base de datos
                using (SqlConnection conn = Coneccion.Instance.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT IDAuditoria, UsuarioID, FechaHoraInicio, FechaHoraFin, Exito, Mensaje FROM AuditoriaIniciarSesion1", conn);

                    // Ejecutar la consulta SQL y leer los resultados
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Por cada registro de auditoría encontrada, crear un objeto Auditoria_Usuarios_Sesion y asignar los valores
                            Auditoria_Usuarios_Sesion auditoria2 = new Auditoria_Usuarios_Sesion
                            {
                                IDAuditoria = (int)reader["IDAuditoria"],
                                UsuarioID = (int)reader["UsuarioID"],
                                FechaHoraInicio = (DateTime)reader["FechaHoraInicio"],
                                FechaHoraFin = (DateTime)reader["FechaHoraFin"],
                                Exito = (bool)reader["Exito"],
                                Mensaje = reader["Mensaje"] as string
                            };

                            // Agregar la auditoría a la lista de auditorías
                            auditorias2.Add(auditoria2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción de manera adecuada
                throw new Exception($"Error al obtener todas las auditorías de inicio de sesión: {ex.Message}", ex);
            }

            return auditorias2;
        }
    }
}
