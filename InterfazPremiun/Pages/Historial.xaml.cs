using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InterfacesOnion.Pages
{
    public partial class Historial : Page
    {
        private ObservableCollection<Member> allData;
        private ObservableCollection<Member> displayedData;
        private int itemsPerPage = 11;
        private int currentPage = 1;

        public Historial()
        {
            InitializeComponent();
            InitializeMembers();
            SetPage(1);
        }

        private void InitializeMembers()
        {
            allData = new ObservableCollection<Member>();

            // Agregar más registros para hacer la prueba
            for (int i = 0; i < 50; i++)
            {
                allData.Add(new Member
                {
                    Number = (i + 1).ToString(),
                    Foto = "C:\\Users\\Alienware\\RiderProjects\\ProgramacionNcapas\\InterfazPremiun\\Imagenes\\male.png",
                    Name = "Name " + (i + 1),
                    Position = "Position " + (i + 1),
                    Email = "email" + (i + 1) + "@example.com",
                    Phone = "415-954-14" + (i + 1)
                });
            }

            membersDataGrid.ItemsSource = allData;
        }

        private void SetPage(int page)
        {
            currentPage = page;
            int startIndex = (page - 1) * itemsPerPage;
            displayedData = new ObservableCollection<Member>(allData.Skip(startIndex).Take(itemsPerPage));
            membersDataGrid.ItemsSource = displayedData;
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                SetPage(currentPage - 1);
            }
        }
        
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    
        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
        
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            Window parentWindow = Window.GetWindow(parent);
    
            if (parentWindow != null)
            {
                parentWindow.WindowState = System.Windows.WindowState.Minimized;
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)allData.Count / itemsPerPage);
            if (currentPage < totalPages)
            {
                SetPage(currentPage + 1);
            }
        }
    }
}
