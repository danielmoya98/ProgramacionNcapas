using CapaDatos;
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
            CargarAuditorias();
            CargarAuditorias2();
        }

        private void CargarAuditorias()
        {
            // Obtener las auditorías desde el controlador
            List<Auditoria_Usuarios> auditorias = controllerAuditoria.ObtenerTodasLasAuditorias();

            // Asignar la lista de auditorías al DataGrid
            dataGridAuditorias.ItemsSource = auditorias;
        }
        private void CargarAuditorias2()
        {
            // Obtener las auditorías desde el controlador
            List<Auditoria_Usuarios_Sesion> auditorias1 = controllerAuditoria.ObtenerTodasLasAuditoriasSesion();

            // Asignar la lista de auditorías al DataGrid
            dataGridAuditoriassesion.ItemsSource = auditorias1;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            dataGridAuditorias.Visibility = Visibility.Visible;
            dataGridAuditoriassesion.Visibility = Visibility.Collapsed;
        }

        private void aud2_Click(object sender, RoutedEventArgs e)
        {
            dataGridAuditorias.Visibility = Visibility.Collapsed;
            dataGridAuditoriassesion.Visibility = Visibility.Visible;
        }
    }
}
