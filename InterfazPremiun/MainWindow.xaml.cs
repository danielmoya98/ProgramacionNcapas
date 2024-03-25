using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace InterfacesOnion;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string fullText = "Simplifica la gestión de usuarios y la auditoría, garantizando el cumplimiento.";
    private string displayedText = "";
    private int currentIndex = 0;
    private DispatcherTimer timer;
    public MainWindow()
    {
        InitializeComponent();
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(0.1);
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
       Close();
    }
    
    private void Timer_Tick(object sender, EventArgs e)
    {
        if (currentIndex < fullText.Length)
        {
            displayedText += fullText[currentIndex];
            txtAnimated.Text = displayedText;
            currentIndex++;
        }
        else
        {
            timer.Stop(); // Detener el temporizador cuando se ha mostrado todo el texto
        }
    }


    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void btnMinimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
    private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
    {
        // string rolUsuario = "";
        Dashboard dashboard = new Dashboard(/*rolUsuario*/);
        dashboard.Show();
        this.Close();
        
    }
}