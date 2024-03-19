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


    }
}
