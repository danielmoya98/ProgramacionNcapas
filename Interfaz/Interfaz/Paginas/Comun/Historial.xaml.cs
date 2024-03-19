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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wallet_Payment.Interfaces.Paginas.Comun
{
    /// <summary>
    /// Lógica de interacción para Historial.xaml
    /// </summary>
    public partial class Historial : Page
    {
        public Historial()
        {
            InitializeComponent();
            CargarAuditoria();
        }
        private void CargarAuditoria()
        {
            try
            {
                // Obtener todos los registros de auditoría llamando al método estático del controlador
                var Auditoria = controllerAuditoria.ObtenerTodasLasAuditorias();

                // Asignar la lista de registros de auditoría al DataGrid
                datagridAuditoria.ItemsSource = Auditoria;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la auditoría: {ex.Message}");
            }
        }
    }
}
