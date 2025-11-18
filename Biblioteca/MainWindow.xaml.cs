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

namespace Biblioteca
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LibroViewModel libroViewModel = new LibroViewModel();
        private UsuarioViewModel usuarioViewModel = new UsuarioViewModel();
        private PrestamoViewModel prestamoViewModel;

        public MainWindow()
        {
            InitializeComponent();
            spLibros.DataContext = libroViewModel;
            spUsuarios.DataContext = usuarioViewModel;
            prestamoViewModel = new PrestamoViewModel(usuarioViewModel.ListaUsuarios, libroViewModel.ListaLibros);
            spPrestamos.DataContext = prestamoViewModel;
        }

        private void Libros_click(object sender, RoutedEventArgs e)
        {
            spLibros.Visibility = Visibility.Visible;
            spUsuarios.Visibility = Visibility.Hidden;
            spPrestamos.Visibility = Visibility.Hidden;
        }

        private void Usuarios_click(object sender, RoutedEventArgs e)
        {
            spLibros.Visibility = Visibility.Hidden;
            spUsuarios.Visibility = Visibility.Visible;
            spPrestamos.Visibility = Visibility.Hidden;

        }

        private void Prestamos_click(object sender, RoutedEventArgs e)
        {
            spLibros.Visibility = Visibility.Hidden;
            spUsuarios.Visibility = Visibility.Hidden;
            spPrestamos.Visibility = Visibility.Visible;

        }
    }
}