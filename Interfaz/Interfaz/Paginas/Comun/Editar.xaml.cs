using CapaDatos;
using Microsoft.Win32;
using ModeloNegocio;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Wallet_Payment.Interfaces.Paginas.Comun
{
    /// <summary>
    /// Lógica de interacción para Editar.xaml
    /// </summary>
    public partial class Editar : Page
    {
        public Editar()
        {
            InitializeComponent();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtener el apellido del usuario desde el TextBox
                string apellido = txtape.Text;

                // Llamar a un método en tu controlador para buscar el usuario por su apellido
                Controller controller = new Controller();
                Usuarios usuario = controller.BuscarUsuarioPorApellido(apellido);

                // Verificar si se encontró el usuario
                if (usuario != null)
                {
                    // Actualizar los datos del usuario con los datos ingresados por el usuario en los controles de la interfaz
                    usuario.Nombre = txtname.Text;
                    usuario.Telefono = txttelefono.Text;
                    usuario.Email = txtemail.Text;
                    usuario.Apellido = txtape.Text;
                    usuario.NombreUsuario = txtnameUser.Text;
                    usuario.Password = txtpass.Text;

                    if (cmbRol.SelectedItem != null)
                    {
                        // Assuming you have an instance of Usuarios called usuario
                        ComboBoxItem selectedRoleItem = cmbRol.SelectedItem as ComboBoxItem;
                        if (selectedRoleItem != null)
                        {
                            usuario.Rol = selectedRoleItem.Content.ToString();
                        }
                    }
                    usuario.Foto = Controller.fotoBytesGlobal; // Asegúrate de que fotoBytesGlobal contenga la imagen actualizada
                                                               // Si la contraseña se puede modificar, también deberías obtenerla aquí

                    // Llamar al método ActualizarUsuario del controlador para guardar los cambios en la base de datos
                    controller.ActualizarUsuario(usuario);

                    // Mostrar un mensaje de éxito al usuario
                    MessageBox.Show("Usuario actualizado correctamente.");
                }
                else
                {
                    MessageBox.Show("No se encontró ningún usuario con el apellido proporcionado.");
                }
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si ocurre alguna excepción
                MessageBox.Show($"Error al guardar los cambios: {ex.Message}");
            }
        }


        private void agrgarfoto_Click(object sender, RoutedEventArgs e)
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



        private void txtape_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                // Obtener el apellido ingresado por el usuario en el campo de texto
                string apellido = txtape.Text;

                // Llamar al método BuscarUsuarioPorApellido del controlador para buscar los usuarios que coincidan con el apellido
                Controller controller = new Controller();
                List<Usuarios> usuarios = controller.BuscarUsuarioPorApellidos(apellido);

                // Verificar si se encontró al menos un usuario con ese apellido
                if (usuarios.Count > 0)
                {
                    // Obtener el primer usuario encontrado
                    Usuarios usuarioEncontrado = usuarios[0];

                    // Llenar los datos del usuario encontrado en los campos de texto y otros controles de la interfaz
                    txtname.Text = usuarioEncontrado.Nombre;
                    ComboBoxItem selectedItem = null;
                    foreach (ComboBoxItem item in cmbRol.Items)
                    {
                        if (item.Content.ToString() == usuarioEncontrado.Rol)
                        {
                            selectedItem = item;
                            break;
                        }
                    }

                    // Set the selected item if found
                    if (selectedItem != null)
                    {
                        cmbRol.SelectedItem = selectedItem;
                    }
                    txttelefono.Text = usuarioEncontrado.Telefono;
                    txtemail.Text = usuarioEncontrado.Email;
                    txtnameUser.Text = usuarioEncontrado.NombreUsuario;
                    txtpass.Text = usuarioEncontrado.Password;
                    // Verificar si el usuario tiene una foto
                    if (usuarioEncontrado.Foto != null && usuarioEncontrado.Foto.Length > 0)
                    {
                        // Crear un nuevo BitmapImage
                        BitmapImage image = new BitmapImage();

                        // Inicializar el BitmapImage con los bytes de la imagen
                        using (MemoryStream stream = new MemoryStream(usuarioEncontrado.Foto))
                        {
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = stream;
                            image.EndInit();
                        }

                        // Asignar el BitmapImage al ImageBrush
                        imgBrush.ImageSource = image;
                    }
                    else
                    {
                        // Si el usuario no tiene foto, limpiar la imagen
                        imgBrush.ImageSource = null;
                    }



                }
                else
                {
                    // Limpiar los campos si no se encontró ningún usuario con ese apellido
                    txtname.Text = "";
                    //txtrol.Text = "";
                    txttelefono.Text = "";
                    txtemail.Text = "";
                    txtnameUser.Text = "";
                    txtpass.Text = "";
                    // También puedes limpiar la imagen si es necesario
                    imgBrush.ImageSource = null;
                }
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si ocurre alguna excepción
                MessageBox.Show($"Error al buscar usuarios por apellido: {ex.Message}");
            }
        }
    }
}
