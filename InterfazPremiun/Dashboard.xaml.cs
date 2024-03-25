using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using InterfacesOnion.Pages;

namespace InterfacesOnion;

public partial class Dashboard : Window
{
    // private readonly string usuarioActualRol;
    public Dashboard( /*string rolUsuarioActual*/)
    {
        InitializeComponent();
        frame.NavigationService.Navigate(new Listar());
        // usuarioActualRol = rolUsuarioActual;
        // if (usuarioActualRol != "Administrador")
        // {
        //     // paginaAdmin.Visibility = Visibility.Collapsed;
        // }
    }


    private void CrearUsuarios_OnClick(object sender, RoutedEventArgs e)
    {
        frame.NavigationService.Navigate(new Registrar());
    }

    private void EditarUsuarios_OnClick(object sender, RoutedEventArgs e)
    {
        frame.NavigationService.Navigate(new Editar());
    }

    private void Listar_OnClick(object sender, RoutedEventArgs e)
    {
        frame.NavigationService.Navigate(new Listar());
    }

    private void HistorialButton_OnClick(object sender, RoutedEventArgs e)
    {
        frame.NavigationService.Navigate(new Historial());
    }

    private void EliminarButton_OnClick(object sender, RoutedEventArgs e)
    {
        frame.NavigationService.Navigate(new Eliminar());
    }

    private void SalirButton_OnClick(object sender, RoutedEventArgs e)
    {
        MainWindow login = new MainWindow();
        login.Show();
        this.Close();
    }
}