using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace InterfacesOnion.Pages;

public partial class Registrar : Page
{
    
    private string fullText = "Nuestra empresa se compromete a mantener a salvo sus datos personales";
    private string displayedText = "";
    private int currentIndex = 0;
    private DispatcherTimer timer;
    public Registrar()
    {
        InitializeComponent();
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(0.1);
        timer.Tick += Timer_Tick;
        timer.Start();
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
    
    private void Rol_Click(object sender, RoutedEventArgs e)
    {
        // Obtener el UserControl:Rol que generó el evento Click
        var rolControl = (sender as UserControls.Rol);

        // Acceder al valor de UpPrice del UserControl y usarlo según necesites
        string upPrice = rolControl.UpPrice;
        MessageBox.Show($"El rol seleccionado es: {upPrice}");
    }

    private void OpenFile_OnClick(object sender, RoutedEventArgs e)
    {
        // Crear un cuadro de diálogo para abrir archivo
        Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

        // Establecer filtros de archivo si es necesario
        openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif|Todos los archivos|*.*";

        // Mostrar el cuadro de diálogo y esperar a que el usuario seleccione un archivo
        bool? result = openFileDialog.ShowDialog();

        // Verificar si el usuario ha seleccionado un archivo
        if (result == true)
        {
            // Obtener la ruta del archivo seleccionado
            string selectedFilePath = openFileDialog.FileName;

            // Cargar la imagen seleccionada en el control Image
            BitmapImage bitmap = new BitmapImage(new Uri(selectedFilePath));
            Foto.Source = bitmap; // Reemplaza YourImageControl con el nombre de tu control Image
        }
    }

}