using CapaDatos;
using Microsoft.Win32;
using ModeloNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wallet_Payment.Interfaces.Paginas.Administrador
{
    /// <summary>
    /// Lógica de interacción para Registrar.xaml
    /// </summary>
    public partial class Registrar : Page
    {
        public Registrar()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verificar si el texto ingresado es un número
            if (e.Text.Any(char.IsDigit))
            {
                // Si es un número, marcamos el evento como manejado para evitar que se ingrese
                e.Handled = true;
            }
        }

        private void TextBox_PreviewTextInput2(object sender, TextCompositionEventArgs e)
        {
            // Verificar si el texto ingresado no es un número
            if (!int.TryParse(e.Text, out _))
            {
                // Si no es un número, marcamos el evento como manejado para evitar que se ingrese
                e.Handled = true;
            }
        }

        private void Registrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Crear un nuevo objeto Usuario con los datos ingresados en la interfaz
                Usuarios nuevoUsuario = new Usuarios
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Telefono = txtTelefono.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text,
                    Foto = Controller.fotoBytesGlobal,
                    NombreUsuario = txtNombreUsuario.Text
                };

                if (txtRol.SelectedItem != null)
                {
                    ComboBoxItem selectedRoleItem = txtRol.SelectedItem as ComboBoxItem;
                    if (selectedRoleItem != null)
                    {
                        nuevoUsuario.Rol = selectedRoleItem.Content.ToString();
                    }
                }

                // Llamar al método CrearUsuario del controlador para registrar el nuevo usuario
                Controller controller = new Controller();
                controller.CrearUsuario(nuevoUsuario);

                // Mostrar mensaje de éxito al usuario
                MessageBox.Show("Usuario registrado correctamente.");
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si ocurre alguna excepción
                MessageBox.Show($"Error al registrar usuario: {ex.Message}");
            }
        }

        private void btnCargarFoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif";

                if (openFileDialog.ShowDialog() == true)
                {
                    // Actualizar dinámicamente el ImageSource del ImageBrush
                    imgBrush.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName));

                    // Obtener los bytes de la imagen seleccionada utilizando el método en Controller
                    byte[] fotoBytes = Controller.ObtenerBytesDesdeArchivo(openFileDialog.FileName);

                    if (fotoBytes != null)
                    {
                        // Asignar los bytes a la variable global en Controller
                        Controller.fotoBytesGlobal = fotoBytes;
                    }
                    else
                    {
                        MessageBox.Show("No se pudieron obtener los bytes de la imagen.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la imagen: {ex.Message}");
            }
        }
    }
}
