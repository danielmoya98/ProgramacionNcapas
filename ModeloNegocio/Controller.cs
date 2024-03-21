using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using CapaDatos;


namespace ModeloNegocio
{
    public class Controller
    {
        private static readonly Coneccion conexion = Coneccion.Instance;

        public class Usuario
        {
            public string NombreCompleto { get; set; }
            public string Rol { get; set; }
            public string Email { get; set; }
            public string Telefono { get; set; }
        }

        public class RespuestaInicioSesion
        {
            public bool SesionIniciada { get; set; }
            public string Mensaje { get; set; }
            public Usuario Usuario { get; set; }
        }

        public RespuestaInicioSesion IniciarSesion(string nombreusuario, string contraseña)
        {
            RespuestaInicioSesion respuesta = new RespuestaInicioSesion();

            try
            {
                string contraseñaHash = CalcularHash(contraseña);
                using (SqlConnection conn = conexion.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT NombreCompleto, Rol, Email, Telefono FROM Usuarios WHERE NombreUsuario = @NombreUsuario AND PasswordHash = @Contraseña", conn);
                    cmd.Parameters.AddWithValue("@NombreUsuario", nombreusuario);
                    cmd.Parameters.AddWithValue("@Contraseña", contraseñaHash);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        respuesta.SesionIniciada = true;
                        respuesta.Mensaje = "Sesión iniciada correctamente";
                        respuesta.Usuario = new Usuario
                        {
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            Rol = reader["Rol"].ToString(),
                            Email = reader["Email"].ToString(),
                            Telefono = reader["Telefono"].ToString()
                        };
                    }
                    else
                    {
                        respuesta.SesionIniciada = false;
                        respuesta.Mensaje = "Nombre de usuario o contraseña incorrectos";
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.SesionIniciada = false;
                respuesta.Mensaje = "Error al iniciar sesión. Por favor, inténtelo de nuevo más tarde.";
                // Registra la excepción para propósitos de depuración
                Console.WriteLine($"Error al iniciar sesión: {ex.Message}");
            }

            return respuesta;
        }
        public void CrearUsuario(Usuarios usuario)
        {
            try
            {
                byte[] fotoBytes = ObtenerBytesDesdeImagen(fotoBytesGlobal);

                using (SqlConnection conn = conexion.GetConnection())
                {
                    conn.Open();

                    // Iniciar una transacción
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO Usuarios (Nombre, Apellido, Rol, Telefono, Email, Foto, Estado, NombreUsuario, Password) VALUES (@Nombre, @Apellido, @Rol, @Telefono, @Email, @Foto, @Estado, @NombreUsuario, @Password); SELECT SCOPE_IDENTITY();", conn, transaction);
                        cmd.Parameters.AddWithValue("@Email", usuario.Email);
                        cmd.Parameters.AddWithValue("@Password", CalcularHash(usuario.Password)); // Cifrar la contraseña
                        cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                        cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                        cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                        cmd.Parameters.AddWithValue("@Foto", fotoBytes);
                        cmd.Parameters.AddWithValue("@Estado", 1);
                        cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                        int newUserID = Convert.ToInt32(cmd.ExecuteScalar());

                        if (newUserID > 0)
                        {
                            // Usuario creado correctamente
                            Console.WriteLine("Usuario creado correctamente");

                            // Obtener todos los datos del usuario para el registro de auditoría
                            string nuevoDato = $"Nombre: {usuario.Nombre}, Apellido: {usuario.Apellido}, Rol: {usuario.Rol}, Telefono: {usuario.Telefono}, Email: {usuario.Email}, Estado: {usuario.Estado}, NombreUsuario: {usuario.NombreUsuario}";

                            // Llamada al procedimiento almacenado de auditoría
                            SqlCommand cmdAuditoria = new SqlCommand("sp_InsertarAuditoria", conn, transaction);
                            cmdAuditoria.CommandType = CommandType.StoredProcedure;
                            cmdAuditoria.Parameters.AddWithValue("@UsuarioID", newUserID);
                            cmdAuditoria.Parameters.AddWithValue("@CampoAfectado", "Nuevo usuario creado");
                            cmdAuditoria.Parameters.AddWithValue("@NuevoDato", nuevoDato);
                            cmdAuditoria.ExecuteNonQuery();

                            // Confirmar la transacción
                            transaction.Commit();
                        }
                        else
                        {
                            Console.WriteLine("No se pudo crear el usuario");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Si ocurre algún error, hacer rollback de la transacción
                        transaction.Rollback();
                        Console.WriteLine($"Error al crear usuario: {ex.Message}");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear usuario: {ex.Message}");
                throw;
            }
        }
        private string CalcularHash(string contraseña)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convertir la contraseña en un array de bytes
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(contraseña));

                // Convertir el array de bytes en una cadena hexadecimal
                System.Text.StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static byte[] fotoBytesGlobal { get; set; }
        public static byte[] ObtenerBytesDesdeArchivo(string filePath)
        {
            try
            {
                return File.ReadAllBytes(filePath);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error de entrada/salida al leer el archivo: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"No se tiene acceso al archivo: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer el archivo: {ex.Message}");
            }

            return null;
        }
        private byte[] ObtenerBytesDesdeImagen(byte[] fotoBytesGlobal)
        {
            // Supongamos que la imagen ya está en formato de bytes
            return fotoBytesGlobal;
        }
        //editar users
        public void ActualizarUsuario(Usuarios usuario)
        {
            try
            {
                // Obtener el usuario anterior para obtener el dato anterior
                Usuarios usuarioAnterior = ObtenerUsuarioPorApellido(usuario.Apellido);

                // Hashear la nueva contraseña
                string hashContraseña = CalcularHash(usuario.Password);

                // Una vez validado, puedes proceder a actualizar el usuario en la base de datos
                using (SqlConnection conn = CapaDatos.Coneccion.Instance.GetConnection())
                {
                    conn.Open();

                    // Actualizar el usuario en la base de datos
                    SqlCommand cmd = new SqlCommand("UPDATE Usuarios SET Nombre = @Nombre, Rol = @Rol, Telefono = @Telefono, Email = @Email, Foto = @Foto, Estado = @Estado, NombreUsuario = @NombreUsuario, Password = @Password WHERE Apellido = @Apellido", conn);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Foto", usuario.Foto);
                    cmd.Parameters.AddWithValue("@Estado", usuario.Estado);
                    cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@Password", hashContraseña); // Usar el hash de la contraseña
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        Console.WriteLine("Usuario actualizado correctamente.");

                        // Llamar al procedimiento almacenado de auditoría
                        SqlCommand cmdAuditoria = new SqlCommand("sp_ActualizarAuditoria", conn);
                        cmdAuditoria.CommandType = CommandType.StoredProcedure;
                        cmdAuditoria.Parameters.AddWithValue("@UsuarioID", usuarioAnterior.Id); // Usamos el ID del usuario encontrado por apellido
                        cmdAuditoria.Parameters.AddWithValue("@CampoAfectado", "Usuario actualizado");
                        cmdAuditoria.Parameters.AddWithValue("@DatoAnterior", ObtenerDatoAnterior(usuarioAnterior)); // Obtener el dato anterior
                        cmdAuditoria.Parameters.AddWithValue("@NuevoDato", ObtenerNuevoDato(usuario)); // Obtener el nuevo dato
                        cmdAuditoria.ExecuteNonQuery();
                    }
                    else
                    {
                        Console.WriteLine("No se pudo actualizar el usuario.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar usuario: {ex.Message}");
            }
        }

        // Método para obtener un usuario por su apellido
        private Usuarios ObtenerUsuarioPorApellido(string apellido)
        {
            Usuarios usuarioEncontrado = null;
            try
            {
                // Crear la conexión a la base de datos
                using (SqlConnection conn = CapaDatos.Coneccion.Instance.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM Usuarios WHERE Apellido = @Apellido ORDER BY Id DESC", conn);
                    cmd.Parameters.AddWithValue("@Apellido", apellido);

                    // Ejecutar la consulta SQL y leer el resultado
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        // Por cada usuario encontrado, crear un objeto Usuarios y asignar los valores
                        usuarioEncontrado = new Usuarios
                        {
                            // Asignar los valores de las columnas de la tabla Usuarios a las propiedades del objeto usuario
                            //Id = (int)reader["Id"],
                            Nombre = (string)reader["Nombre"],
                            Apellido = (string)reader["Apellido"],
                            Rol = (string)reader["Rol"],
                            Telefono = (string)reader["Telefono"],
                            Email = (string)reader["Email"],
                            NombreUsuario = (string)reader["NombreUsuario"],
                            Password = (string)reader["Password"],
                            // Recuperar la foto del usuario como un arreglo de bytes
                            Foto = reader["Foto"] != DBNull.Value ? (byte[])reader["Foto"] : null
                        };

                        // Una vez que encontramos el usuario anterior, podemos llamar a ObtenerDatoAnterior
                        string datoAnterior = ObtenerDatoAnterior(usuarioEncontrado);
                        Console.WriteLine($"Datos anteriores:\n{datoAnterior}");
                    }
                    // Cerrar el lector
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                Console.WriteLine($"Error al buscar usuario por apellido: {ex.Message}");
                // Puedes lanzar una excepción aquí si lo deseas
                // throw new Exception("Error al buscar usuario por apellido", ex);
            }
            return usuarioEncontrado;
        }

        private string ObtenerDatoAnterior(Usuarios usuario)
        {
            if (usuario == null)
            {
                // Si no hay usuario anterior, retorna una cadena vacía
                return "";
            }

            StringBuilder datoAnterior = new StringBuilder();
            // Compara las propiedades del usuario con los datos actuales
            datoAnterior.AppendLine($"Nombre anterior: {usuario.Nombre}");
            datoAnterior.AppendLine($"Apellido anterior: {usuario.Apellido}");
            datoAnterior.AppendLine($"Rol anterior: {usuario.Rol}");
            datoAnterior.AppendLine($"Teléfono anterior: {usuario.Telefono}");
            datoAnterior.AppendLine($"Email anterior: {usuario.Email}");
            datoAnterior.AppendLine($"Nombre de usuario anterior: {usuario.NombreUsuario}");

            // En este punto, puedes agregar la lógica para comparar más propiedades según tus necesidades.

            return datoAnterior.ToString();
        }
        private string ObtenerNuevoDato(Usuarios usuario)
        {
            if (usuario == null)
            {
                // Si no hay usuario proporcionado, retorna una cadena vacía
                return "";
            }

            StringBuilder nuevoDato = new StringBuilder();
            // Concatena los valores de las propiedades del usuario actualizado
            nuevoDato.AppendLine($"Nombre: {usuario.Nombre}");
            nuevoDato.AppendLine($"Apellido: {usuario.Apellido}");
            nuevoDato.AppendLine($"Rol: {usuario.Rol}");
            nuevoDato.AppendLine($"Teléfono: {usuario.Telefono}");
            nuevoDato.AppendLine($"Email: {usuario.Email}");
            nuevoDato.AppendLine($"Nombre de usuario: {usuario.NombreUsuario}");

            // En este punto, puedes agregar la lógica para incluir más propiedades según tus necesidades.

            return nuevoDato.ToString();
        }

        public Usuarios BuscarUsuarioPorApellido(string apellido)
        {
            Usuarios usuario = null;
            try
            {
                // Crear la conexión a la base de datos
                using (SqlConnection conn = CapaDatos.Coneccion.Instance.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Usuarios WHERE Apellido = @Apellido", conn);
                    cmd.Parameters.AddWithValue("@Apellido", apellido);

                    // Ejecutar la consulta SQL y leer el resultado
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        // Si se encuentra un usuario con el apellido proporcionado, crear un objeto Usuarios y asignar los valores
                        usuario = new Usuarios
                        {
                            Nombre = (string)reader["Nombre"],
                            Apellido = (string)reader["Apellido"],
                            Rol = (string)reader["Rol"],
                            Telefono = (string)reader["Telefono"],
                            Email = (string)reader["Email"],
                            // Asigna otros campos si es necesario
                        };
                    }
                    // Cerrar el lector
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                Console.WriteLine($"Error al buscar usuario por apellido: {ex.Message}");
                // Puedes lanzar una excepción aquí si lo deseas
                // throw new Exception("Error al buscar usuario por apellido", ex);
            }
            return usuario;
        }
        public List<Usuarios> BuscarUsuarioPorApellidos(string apellido)
        {
            List<Usuarios> usuariosEncontrados = new List<Usuarios>();
            try
            {
                // Crear la conexión a la base de datos
                using (SqlConnection conn = CapaDatos.Coneccion.Instance.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Usuarios WHERE Apellido = @Apellido", conn);
                    cmd.Parameters.AddWithValue("@Apellido", apellido);

                    // Ejecutar la consulta SQL y leer el resultado
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // Por cada usuario encontrado, crear un objeto Usuarios y asignar los valores
                        Usuarios usuario = new Usuarios
                        {
                            // Asignar los valores de las columnas de la tabla Usuarios a las propiedades del objeto usuario
                            //Id = (int)reader["Id"],
                            Nombre = (string)reader["Nombre"],
                            Apellido = (string)reader["Apellido"],
                            Rol = (string)reader["Rol"],
                            Telefono = (string)reader["Telefono"],
                            Email = (string)reader["Email"],
                            NombreUsuario = (string)reader["NombreUsuario"],
                            Password = (string)reader["Password"],
                            // Recuperar la foto del usuario como un arreglo de bytes

                            Foto = reader["Foto"] != DBNull.Value ? (byte[])reader["Foto"] : null
                        };
                        // Agregar el usuario a la lista de usuarios encontrados
                        usuariosEncontrados.Add(usuario);
                    }
                    // Cerrar el lector
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                Console.WriteLine($"Error al buscar usuarios por apellido: {ex.Message}");
                // Puedes lanzar una excepción aquí si lo deseas
                // throw new Exception("Error al buscar usuarios por apellido", ex);
            }
            return usuariosEncontrados;
        }
        //eliminarrrr
        public void EliminarUsuario(int usuarioID)
        {
            try
            {
                string datoAnterior = ObtenerDatoAnterior(usuarioID); // Obtener los datos del usuario antes de eliminarlo

                // Crear la conexión a la base de datos
                using (SqlConnection conn = CapaDatos.Coneccion.Instance.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Usuarios WHERE Id = @UsuarioID", conn);
                    cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);

                    // Ejecutar la consulta SQL
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Verificar si se eliminó correctamente al menos una fila
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Usuario eliminado correctamente.");
                        // Realizar la auditoría
                        RealizarAuditoriaEliminacion(usuarioID, "Usuario eliminado", datoAnterior);
                    }
                    else
                    {
                        Console.WriteLine("No se encontró ningún usuario con el ID proporcionado.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                Console.WriteLine($"Error al eliminar usuario: {ex.Message}");
            }
        }

        private void RealizarAuditoriaEliminacion(int usuarioID, string campoAfectado, string datoAnterior)
        {
            try
            {
                // Crear la conexión a la base de datos
                using (SqlConnection conn = CapaDatos.Coneccion.Instance.GetConnection())
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
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                Console.WriteLine($"Error al realizar la auditoría de eliminación: {ex.Message}");
            }
        }

        private string ObtenerDatoAnterior(int usuarioID)
        {
            try
            {
                string datoAnterior = "";

                // Crear la conexión a la base de datos
                using (SqlConnection conn = CapaDatos.Coneccion.Instance.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Nombre, Apellido, Rol, Telefono, Email, NombreUsuario FROM Usuarios WHERE Id = @UsuarioID", conn);
                    cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);

                    // Ejecutar la consulta SQL y leer el resultado
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        // Construir la cadena de texto con los datos del usuario
                        datoAnterior = $"Nombre: {reader["Nombre"]}, Apellido: {reader["Apellido"]}, Rol: {reader["Rol"]}, Telefono: {reader["Telefono"]}, Email: {reader["Email"]}, Nombre de usuario: {reader["NombreUsuario"]}";
                    }
                    // Cerrar el lector
                    reader.Close();
                }

                return datoAnterior;
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                Console.WriteLine($"Error al obtener los datos del usuario anterior: {ex.Message}");
                // En caso de error, devuelve una cadena vacía
                return "";
            }
        }

        //lista
        public static List<Usuarios> ObtenerTodosLosUsuarios()
        {
            List<Usuarios> usuarios = new List<Usuarios>();

            try
            {
                // Crear la conexión a la base de datos
                using (SqlConnection conn = CapaDatos.Coneccion.Instance.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Usuarios", conn);

                    // Ejecutar la consulta SQL y leer los resultados
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // Por cada usuario encontrado, crear un objeto Usuarios y asignar los valores
                        Usuarios usuario = new Usuarios
                        {
                            //Id = (int)reader["Id"],
                            Nombre = (string)reader["Nombre"],
                            Apellido = (string)reader["Apellido"],
                            Rol = (string)reader["Rol"],
                            Telefono = (string)reader["Telefono"],
                            Email = (string)reader["Email"],
                            NombreUsuario = (string)reader["NombreUsuario"],
                            Password = (string)reader["Password"],
                            // Si hay más campos en tu tabla Usuarios, asegúrate de asignarlos aquí
                        };

                        // Agregar el usuario a la lista de usuarios
                        usuarios.Add(usuario);
                    }

                    // Cerrar el lector
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                Console.WriteLine($"Error al obtener todos los usuarios: {ex.Message}");
            }

            return usuarios;
        }
    }
}