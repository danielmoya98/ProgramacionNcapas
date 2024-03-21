using CapaDatos;
using ModeloNegocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wallet_Payment.Interfaces.Ventanas;

using static ModeloNegocio.Controller;


namespace Wallet_Payment.Interfaces.Ventanas.Login
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        

        public Login()
        {
            InitializeComponent();
        }

        private int intentosFallidos = 0; // Contador de intentos fallidos

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (intentosFallidos >= 3)
                {
                    MessageBox.Show("Se ha excedido el número máximo de intentos fallidos. La sesión se cerrará.");
                    // Aquí puedes agregar el código para cerrar la sesión, como limpiar los campos de nombre de usuario y contraseña, o cualquier otra acción necesaria.
                    Close();
                    return;
                }

                string connectionString = "Server=(LocalDB)\\MSSQLLocalDB;Database=ONION_PROJECT;Integrated Security=true; TrustServerCertificate=True";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string nombreusuario = txtUsername.Text;
                    string contraseña = txtPassword.Password;

                    // Validar la entrada del usuario
                    if (string.IsNullOrWhiteSpace(nombreusuario) || string.IsNullOrWhiteSpace(contraseña))
                    {
                        MessageBox.Show("Por favor, ingrese nombre de usuario y contraseña.");
                        return;
                    }

                    // Calcula el hash de la contraseña proporcionada
                    string contraseñaHash = CalcularHash(contraseña);

                    // Consulta SQL para obtener los datos del usuario
                    string query = "SELECT ID, Nombre, Apellido, Rol, Email, Telefono FROM Usuarios WHERE NombreUsuario = @NombreUsuario AND Password = @Password";

                    SqlCommand comando = new SqlCommand(query, conn);
                    comando.Parameters.AddWithValue("@NombreUsuario", nombreusuario);
                    comando.Parameters.AddWithValue("@Password", contraseñaHash);

                    using (SqlDataReader registros = comando.ExecuteReader())
                    {
                        if (registros.Read())
                        {
                            // Si se encontraron registros, la autenticación fue exitosa
                            // Obtenemos los datos del usuario
                            int usuarioID = registros.GetInt32(0);
                            string nombre = registros["Nombre"].ToString();
                            string apellido = registros["Apellido"].ToString();
                            string nombreCompleto = nombre + " " + apellido;
                            string rol = registros["Rol"].ToString();
                            string email = registros["Email"].ToString();
                            string telefono = registros["Telefono"].ToString();

                            // Restablecer el contador de intentos fallidos
                            intentosFallidos = 0;

                            // Obtenemos la fecha y hora de inicio de sesión
                            DateTime fechaHoraInicioSesion = DateTime.Now;

                            MessageBox.Show("Bienvenido, " + nombreCompleto);

                            // Llamamos al procedimiento almacenado para registrar el inicio de sesión
                            using (SqlConnection connAuditoria = new SqlConnection(connectionString))
                            {
                                connAuditoria.Open();
                                SqlCommand comandoAuditoria = new SqlCommand("sp_InsertarAuditoriaInicioSesion", connAuditoria);
                                comandoAuditoria.CommandType = System.Data.CommandType.StoredProcedure;

                                // Pasamos los parámetros al procedimiento almacenado
                                comandoAuditoria.Parameters.AddWithValue("@UsuarioID", usuarioID);
                                comandoAuditoria.Parameters.AddWithValue("@FechaHoraInicio", fechaHoraInicioSesion);
                                comandoAuditoria.Parameters.AddWithValue("@FechaHoraFin", DBNull.Value); // Puedes definir la fecha y hora de finalización de la sesión según sea necesario
                                comandoAuditoria.Parameters.AddWithValue("@Exito", 1); // Indica que el inicio de sesión fue exitoso
                                comandoAuditoria.Parameters.AddWithValue("@Mensaje", "Inicio de sesión exitoso");

                                comandoAuditoria.ExecuteNonQuery();
                            }

                            // Por ejemplo, podrías abrir una nueva ventana pasando los datos del usuario
                            MainWindow ventanaPrincipal = new MainWindow(nombreCompleto, rol, email, telefono);
                            ventanaPrincipal.Show(); // Muestra la ventana principal

                            this.Hide();
                        }
                        else
                        {
                            // Si no se encontraron registros, la autenticación falló
                            MessageBox.Show("datos incorrectos.");

                            // Incrementar el contador de intentos fallidos
                            intentosFallidos++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message);
            }
        }
        private string CalcularHash(string contraseña)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convertir la contraseña en un array de bytes
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(contraseña));

                // Convertir el array de bytes en una cadena hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
