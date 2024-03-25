using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace InterfacesOnion.Pages;

public partial class Eliminar : Page
{
    private ObservableCollection<Member> allData;
    private ObservableCollection<Member> displayedData;
    private int itemsPerPage = 11;
    private int currentPage = 1;
    public Eliminar()
    {
        InitializeComponent();
        InitializeMembers();
        SetPage(1);
    }
    
    
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        // Obtener la lista de ítems seleccionados en el DataGrid
        List<object> selectedItems = new List<object>(membersDataGrid.SelectedItems.Cast<object>());

        // Obtener la fuente de datos del DataGrid
        ObservableCollection<Member> dataSource = membersDataGrid.ItemsSource as ObservableCollection<Member>;

        if (dataSource != null)
        {
            // Eliminar cada ítem seleccionado de la fuente de datos
            foreach (var selectedItem in selectedItems)
            {
                dataSource.Remove(selectedItem as Member);
            }
        }
    }
    
    
    /*private void btnDeleteSelected_Click(object sender, RoutedEventArgs e)
    {
        // Obtener la fuente de datos del DataGrid
        ObservableCollection<Member> dataSource = membersDataGrid.ItemsSource as ObservableCollection<Member>;

        if (dataSource != null)
        {
            // Crear una lista para almacenar los elementos seleccionados
            List<Member> selectedItems = new List<Member>();

            // Iterar sobre cada fila del DataGrid
            foreach (Member item in dataSource)
            {
                // Obtener el CheckBox asociado a la fila actual
                DataGridRow row = (DataGridRow)membersDataGrid.ItemContainerGenerator.ContainerFromItem(item);
                CheckBox checkBox = FindVisualChild<CheckBox>(row);

                // Verificar si el CheckBox está marcado
                if (checkBox.IsChecked == true)
                {
                    // Agregar el elemento correspondiente a la fila actual a la lista de elementos seleccionados
                    selectedItems.Add(item);
                }
            }

            // Eliminar los elementos seleccionados de la fuente de datos
            foreach (Member selectedItem in selectedItems)
            {
                dataSource.Remove(selectedItem);
            }
        }
    }*/
    
    private void btnDeleteSelected_Click(object sender, RoutedEventArgs e)
    {
        // Obtener la fuente de datos del DataGrid
        ObservableCollection<Member> dataSource = membersDataGrid.ItemsSource as ObservableCollection<Member>;

        if (dataSource != null)
        {
            // Crear una lista para almacenar los elementos seleccionados
            List<Member> selectedItems = new List<Member>();

            // Contador para contar el número de checkboxes marcados
            int checkedCount = 0;

            // Iterar sobre cada fila del DataGrid
            foreach (Member item in dataSource)
            {
                // Obtener el CheckBox asociado a la fila actual
                DataGridRow row = (DataGridRow)membersDataGrid.ItemContainerGenerator.ContainerFromItem(item);
                CheckBox checkBox = FindVisualChild<CheckBox>(row);

                // Verificar si el CheckBox está marcado
                if (checkBox.IsChecked == true)
                {
                    // Agregar el elemento correspondiente a la fila actual a la lista de elementos seleccionados
                    selectedItems.Add(item);
                    checkedCount++; // Incrementar el contador de checkboxes marcados
                }
            }

            // Mostrar u ocultar el botón según la cantidad de checkboxes marcados
            if (checkedCount > 1)
            {
                btnDeleteSelected.Visibility = Visibility.Visible;
            }
            else
            {
                btnDeleteSelected.Visibility = Visibility.Collapsed;
            }

            // Mostrar el número de checkboxes marcados en un MessageBox
            MessageBox.Show("Número de checkboxes marcados: " + checkedCount);

            // Eliminar los elementos seleccionados de la fuente de datos
            foreach (Member selectedItem in selectedItems)
            {
                dataSource.Remove(selectedItem);
            }
        }
    }
    
    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        // Obtener la fila correspondiente al CheckBox
        DataGridRow row = (DataGridRow)membersDataGrid.ItemContainerGenerator.ContainerFromItem(((FrameworkElement)sender).DataContext);

        if (row != null)
        {
            row.IsSelected = true; // Seleccionar la fila
        }
    }

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        // Obtener la fila correspondiente al CheckBox
        DataGridRow row = (DataGridRow)membersDataGrid.ItemContainerGenerator.ContainerFromItem(((FrameworkElement)sender).DataContext);

        if (row != null)
        {
            row.IsSelected = false; // Desseleccionar la fila
        }
    }


    
    
    private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(obj, i);
            if (child != null && child is T)
            {
                return (T)child;
            }
            else
            {
                T childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
        }
        return null;
    }

    
    private void HeaderCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        foreach (DataGridRow row in membersDataGrid.Items)
        {
            row.Background = Brushes.Black;
        }
    }

    private void HeaderCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        foreach (DataGridRow row in membersDataGrid.Items)
        {
            row.Background = SystemColors.WindowBrush;
        }
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

    
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
    
    private void NextPageButton_Click(object sender, RoutedEventArgs e)
    {
        int totalPages = (int)Math.Ceiling((double)allData.Count / itemsPerPage);
        if (currentPage < totalPages)
        {
            SetPage(currentPage + 1);
        }
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
    
    private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
    {
        
        DependencyObject parent = VisualTreeHelper.GetParent(this);
        Window parentWindow = Window.GetWindow(parent);
    
        if (parentWindow != null)
        {
            parentWindow.WindowState = System.Windows.WindowState.Minimized;
        }
    }

    private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Obtener el texto del TextBox
        string searchText = searchTextBox.Text;

        // Obtener la vista de la colección asociada al DataGrid
        ICollectionView view = CollectionViewSource.GetDefaultView(membersDataGrid.ItemsSource);

        // Si la vista es nula o no se puede filtrar, salir
        if (view == null || !view.CanFilter)
            return;

        // Aplicar el filtro
        view.Filter = (item) =>
        {
            Member member = item as Member;
            if (member != null)
            {
                // Comparar el texto del TextBox con las propiedades relevantes del objeto Member
                return member.Name.Contains(searchText) ||
                       member.Position.Contains(searchText) ||
                       member.Email.Contains(searchText) ||
                       member.Phone.Contains(searchText);
            }

            return false;
        };
    }
}