using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace InterfacesOnion.UserControls;

public partial class Rol : UserControl
{
    private static Rol _selectedRol;
    public Rol()
    {
        InitializeComponent();
        // Habilitar la interacción con el control
        this.IsHitTestVisible = true;

        // Agregar un manejador de eventos para el clic
        this.MouseLeftButtonDown += Rol_Click;
    }
    
    private void UserControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        // Si ya hay un Rol seleccionado, restaura su estilo original
        if (_selectedRol != null)
        {
            _selectedRol.border.BorderBrush = Brushes.Transparent;
            _selectedRol.border.BorderThickness = new Thickness(0);
        }

        // Establece el nuevo UserControl como seleccionado y cambia su estilo
        _selectedRol = this;
        border.BorderBrush = Brushes.Yellow;
        border.BorderThickness = new Thickness(1);
    }
    
    // Define el evento Click
    public event RoutedEventHandler Click;

    private void Rol_Click(object sender, MouseButtonEventArgs e)
    {
        // Disparar el evento Click si está suscrito
        Click?.Invoke(this, new RoutedEventArgs());
    }

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Rol));

    public string UpPrice
    {
        get { return (string)GetValue(UpPriceProperty); }
        set { SetValue(UpPriceProperty, value); }
    }

    public static readonly DependencyProperty UpPriceProperty = DependencyProperty.Register("UpPrice", typeof(string), typeof(Rol));

    public string DownPrice
    {
        get { return (string)GetValue(DownPriceProperty); }
        set { SetValue(DownPriceProperty, value); }
    }

    public static readonly DependencyProperty DownPriceProperty = DependencyProperty.Register("DownPrice", typeof(string), typeof(Rol));

    public bool IsActive
    {
        get { return (bool)GetValue(IsActiveProperty); }
        set { SetValue(IsActiveProperty, value); }
    }

    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(Rol));

    public ImageSource Image
    {
        get { return (ImageSource)GetValue(ImageProperty); }
        set { SetValue(ImageProperty, value); }
    }

    public static readonly DependencyProperty ImageProperty =
        DependencyProperty.Register("Image", typeof(ImageSource), typeof(Rol));

}