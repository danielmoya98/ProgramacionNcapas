using ModeloNegocio;
using System;
using System.Collections.Generic;
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


namespace Wallet_Payment.Interfaces.Ventanas.Login
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private string nombre;
        private string apellido;
        public Login()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nombreusuario = txtUsername.Text;
                string contraseña = txtPassword.Password;

                // Llama al método IniciarSesion del controlador de negocio para verificar las credenciales
                string resultado = new Controller().IniciarSesion(nombreusuario, contraseña); //, nombre, apellido);

                // Muestra el resultado al usuario
                MessageBox.Show(resultado);

                // Si la sesión se inicia correctamente, abre la ventana de registro
                if (resultado == "Sesión iniciada correctamente")
                {
                // Crea una instancia de la ventana "regitrar"
                    MainWindow ventana = new MainWindow();//(nombre, apellido);//(nombreUsuario, cargoUsuario);
                   
                    // Muestra la nueva ventana
                    ventana.Show();                
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción y muestra un mensaje de error genérico
                MessageBox.Show("Error al iniciar sesión. Por favor, inténtelo de nuevo más tarde.");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
