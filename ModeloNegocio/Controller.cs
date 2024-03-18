using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using CapaDatos;

namespace ModeloNegocio
{
    public class Controller
    {
        private static readonly Coneccion conexion = Coneccion.Instance;
        
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

        public string IniciarSesion(string email, string contraseña)
        {
            try
            {
                using (var conn = conexion.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Usuarios WHERE Email = @Email AND Password = @Contraseña", conn);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Contraseña", contraseña);
            
                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // Si se encuentra al menos un usuario con el correo electrónico y la contraseña proporcionados
                        return "Sesión iniciada correctamente";
                    }
                    else
                    {
                        // Si no se encuentra ningún usuario con el correo electrónico y la contraseña proporcionados
                        return "Correo electrónico o contraseña incorrectos";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al iniciar sesión: {ex.Message}");
                return "Error al iniciar sesión";
            }
        }

        public void InsertarAuditoria(int usuarioID, string campoAfectado, string nuevoDato)
        {
            try
            {
                using (var conn = conexion.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_InsertarAuditoria", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros al procedimiento almacenado
                    cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                    cmd.Parameters.AddWithValue("@CampoAfectado", campoAfectado);
                    cmd.Parameters.AddWithValue("@NuevoDato", nuevoDato);

                    // Ejecutar el procedimiento almacenado
                    cmd.ExecuteNonQuery();
            
                    Console.WriteLine("Auditoría insertada correctamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar auditoría: {ex.Message}");
            }
        }

        public List<Auditoria_Usuarios> LeerAuditoria(int usuarioID)
        {
            List<Auditoria_Usuarios> auditoria = new List<Auditoria_Usuarios>();

            try
            {
                using (var conn = conexion.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_LeerAuditoria", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetro de entrada al procedimiento almacenado
                    cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Auditoria_Usuarios registroAuditoria = new Auditoria_Usuarios()
                        {
                            IDAuditoria = Convert.ToInt32(reader["IDAuditoria"]),
                            Operacion = reader["Operacion"].ToString(),
                            UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                            FechaOperacion = Convert.ToDateTime(reader["FechaOperacion"]),
                            CampoAfectado = reader["CampoAfectado"].ToString(),
                            DatoAnterior = reader["DatoAnterior"].ToString(),
                            NuevoDato = reader["NuevoDato"].ToString()
                        };

                        auditoria.Add(registroAuditoria);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer auditoría: {ex.Message}");
            }

            return auditoria;
        }

        public void ActualizarAuditoria(int idAuditoria, string campoAfectado, string datoAnterior, string nuevoDato)
        {
            try
            {
                using (var conn = conexion.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_ActualizarAuditoria", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros al procedimiento almacenado
                    cmd.Parameters.AddWithValue("@IDAuditoria", idAuditoria);
                    cmd.Parameters.AddWithValue("@CampoAfectado", campoAfectado);
                    cmd.Parameters.AddWithValue("@DatoAnterior", datoAnterior);
                    cmd.Parameters.AddWithValue("@NuevoDato", nuevoDato);

                    // Ejecutar el procedimiento almacenado
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Auditoría actualizada correctamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar auditoría: {ex.Message}");
            }
        }

        public void RegistrarEliminacionAuditoria(int usuarioID, string campoAfectado, string datoAnterior)
        {
            try
            {
                using (var conn = conexion.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_EliminarAuditoria", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros al procedimiento almacenado
                    cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                    cmd.Parameters.AddWithValue("@CampoAfectado", campoAfectado);
                    cmd.Parameters.AddWithValue("@DatoAnterior", datoAnterior);

                    // Ejecutar el procedimiento almacenado
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Registro de eliminación de auditoría insertado correctamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar eliminación de auditoría: {ex.Message}");
            }
        }

        public void CrearUsuario(Usuarios usuario)
        {
            try
            {
                using (var conn = conexion.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO Usuarios (Nombre, Rol, Apellido, Telefono, Email, Password) " +
                                   "VALUES (@Nombre, @Rol, @Apellido, @Telefono, @Email, @Password);";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Password", usuario.Password);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear usuario: {ex.Message}");
            }
        }
        
        public void ActualizarUsuario(Usuarios usuario)
        {
            try
            {
                using (var conn = conexion.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE Usuarios SET Nombre = @Nombre, Rol = @Rol, Apellido = @Apellido, " +
                                   "Telefono = @Telefono, Email = @Email, Password = @Password WHERE ID = @ID;";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Password", usuario.Password);
                    cmd.Parameters.AddWithValue("@ID", usuario.ID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar usuario: {ex.Message}");
            }
        }
        
        public void EliminarUsuario(int usuarioID)
        {
            try
            {
                using (var conn = conexion.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM Usuarios WHERE ID = @ID;";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", usuarioID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar usuario: {ex.Message}");
            }
        }
    }
}