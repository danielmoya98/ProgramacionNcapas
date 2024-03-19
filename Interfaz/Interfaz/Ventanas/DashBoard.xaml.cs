using System.Windows;
using System.Windows.Input;
using Wallet_Payment.Interfaces.Paginas.Administrador;
using Wallet_Payment.Interfaces.Paginas.Comun;

namespace Wallet_Payment
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Inicio());
            InicioBtn.Style = (Style)FindResource("activeMenuButton");
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void InicioBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Inicio());
            InicioBtn.Style = (Style)FindResource("activeMenuButton");
            HistorialBtn.Style = (Style)FindResource("menuButton");
            EditarBtn.Style = (Style)FindResource("menuButton");
            RegistrarBtn.Style = (Style)FindResource("menuButton");
            ListarBtn.Style = (Style)FindResource("menuButton");
        }

        private void HistorialBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Historial());
            InicioBtn.Style = (Style)FindResource("menuButton");
            HistorialBtn.Style = (Style)FindResource("activeMenuButton");
            EditarBtn.Style = (Style)FindResource("menuButton");
            RegistrarBtn.Style = (Style)FindResource("menuButton");
            ListarBtn.Style = (Style)FindResource("menuButton");
        }

        private void EditarBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Editar());
            InicioBtn.Style = (Style)FindResource("menuButton");
            HistorialBtn.Style = (Style)FindResource("menuButton");
            EditarBtn.Style = (Style)FindResource("activeMenuButton");
            RegistrarBtn.Style = (Style)FindResource("menuButton");
            ListarBtn.Style = (Style)FindResource("menuButton");
        }

        private void RegistrarBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Registrar());
            InicioBtn.Style = (Style)FindResource("menuButton");
            HistorialBtn.Style = (Style)FindResource("menuButton");
            EditarBtn.Style = (Style)FindResource("menuButton");
            RegistrarBtn.Style = (Style)FindResource("activeMenuButton");
            ListarBtn.Style = (Style)FindResource("menuButton");

        }

        private void ListarBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Listar());
            InicioBtn.Style = (Style)FindResource("menuButton");
            HistorialBtn.Style = (Style)FindResource("menuButton");
            EditarBtn.Style = (Style)FindResource("menuButton");
            RegistrarBtn.Style = (Style)FindResource("menuButton");
            ListarBtn.Style = (Style)FindResource("activeMenuButton");
        }

        private void SalirBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}