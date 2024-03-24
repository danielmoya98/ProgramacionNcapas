using System.Windows;
using System.Windows.Controls;

namespace InterfacesOnion.Pages;

public partial class Registrar : Page
{
    public Registrar()
    {
        InitializeComponent();
    }
    
    private void Rol_Click(object sender, RoutedEventArgs e)
    {
        // Obtener el UserControl:Rol que generó el evento Click
        var rolControl = (sender as UserControls.Rol);

        // Acceder al valor de UpPrice del UserControl y usarlo según necesites
        string upPrice = rolControl.UpPrice;
        MessageBox.Show($"El rol seleccionado es: {upPrice}");
    }
}