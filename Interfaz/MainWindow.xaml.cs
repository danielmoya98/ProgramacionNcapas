using System.Linq;

namespace Interfaz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            lista.ItemsSource = ModeloNegocio.Controller.ObtenerTodosLosUsuarios().ToList();
        }
    }
}