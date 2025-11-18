using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Biblioteca
{
    class PrestamoViewModel :ViewModelBase
    {

        private string RutaPrestamos = "prestamos.json";
        public ObservableCollection<Prestamo> ListaPrestamos { get; set; }
        public ObservableCollection<Usuario> ListaUsuarios { get; set; }
        public ObservableCollection<Libro> ListaLibros { get; set; }

        private Usuario usuarioSeleccionado;
        public Usuario UsuarioSeleccionado
        {
            get => usuarioSeleccionado;
            set { usuarioSeleccionado = value; OnPropertyChanged(); }
        }

        private Libro libroSeleccionado;
        public Libro LibroSeleccionado
        {
            get => libroSeleccionado;
            set { libroSeleccionado = value; OnPropertyChanged(); }
        }

        private Prestamo prestamoSeleccionado;
        public Prestamo PrestamoSeleccionado
        { 
            get => prestamoSeleccionado;
            set {
                if (prestamoSeleccionado == value) return;

                prestamoSeleccionado = value;
                OnPropertyChanged();

                CargarDatosParaEdicion();
            }
            
        
        }

        private Usuario usuario;
        public Usuario Usuario
        { 
            get { return usuario; } 
            set { usuario = value; OnPropertyChanged(); }
        }

        private Libro libro;
        public Libro Libro
        {
            get { return libro; }
            set { libro = value; OnPropertyChanged(); }
        }

        private DateTime? fechaprestamo;
        public DateTime? FechaPrestamo 
        { 
            get => fechaprestamo;
            set { fechaprestamo = value; OnPropertyChanged(); }
        }

        private DateTime? fechadevolucion;
        public DateTime? FechaDevolucion
        {
            get => fechadevolucion;
            set { fechadevolucion = value; OnPropertyChanged(); }
        }

        private bool estado;
        public bool Estado 
        {
            get => estado;
            set {
                if (estado != value)
                {
                    estado = value;
                    
                    OnPropertyChanged(nameof(Estado));
                }
            }
        
        }



        public ICommand PrestarCommand { get; private set; }
        public ICommand DevolverCommand { get; private set; }


        public PrestamoViewModel(ObservableCollection<Usuario> usuarios, ObservableCollection<Libro> libros)
        {
            ListaUsuarios = usuarios;
            ListaLibros = libros;
            CargarPrestamos();

            PrestarCommand = new RelayCommand(GuardarPrestamoExecute, CanExecutePrestar);
            DevolverCommand = new RelayCommand(DevolverExecute, CanExecuteDevolver);

        }
        private void CargarPrestamos()
        {
            if (File.Exists(RutaPrestamos))
            {
                try
                {
                    var json = File.ReadAllText(RutaPrestamos);
                    var prestamos = JsonSerializer.Deserialize<ObservableCollection<Prestamo>>(json);
                    ListaPrestamos = prestamos ?? new ObservableCollection<Prestamo>();
                }
                catch (Exception ex){
                    MessageBox.Show($"Error al cargar {ex}");
                    ListaPrestamos = new ObservableCollection<Prestamo>(); }
            }
            else
            {
                ListaPrestamos = new ObservableCollection<Prestamo>();
            }
        }
        private void CargarDatosParaEdicion()
        {
            if (PrestamoSeleccionado != null)
            {
                Usuario = prestamoSeleccionado.Usuario;
                Libro = prestamoSeleccionado.Libro;
                FechaPrestamo = prestamoSeleccionado.FechaPrestamo;
                FechaDevolucion = prestamoSeleccionado.FechaDevolucion;
                Estado = prestamoSeleccionado.Estado;
            }
            else
            {
                LimpiarFormulario();
            }
        }

        private void LimpiarFormulario()
        {
            Usuario = null;
            Libro = null;
            FechaPrestamo = null;
            FechaDevolucion = null;
            Estado = false;
        }

        private bool CanExecutePrestar(object parameter)
        {
            return UsuarioSeleccionado != null &&
           LibroSeleccionado != null &&
           FechaPrestamo.HasValue &&
           FechaDevolucion.HasValue && LibroSeleccionado.Estado != true;
        }

        private void GuardarPrestamoExecute(object parameter)
        {
            if (prestamoSeleccionado != null)
            {
                prestamoSeleccionado.Usuario = Usuario; 
                prestamoSeleccionado.Libro = Libro;
                prestamoSeleccionado.FechaPrestamo = FechaPrestamo;
                prestamoSeleccionado.FechaDevolucion = FechaDevolucion;
                prestamoSeleccionado.Estado = Estado;

                var temp = prestamoSeleccionado;
                prestamoSeleccionado = null;
                prestamoSeleccionado = temp;
            }
            else
            {
                var nuevoprestamo = new Prestamo(
                    UsuarioSeleccionado,
                    LibroSeleccionado,
                    this.FechaPrestamo,
                    this.FechaDevolucion,
                    Estado = true);

                ListaPrestamos.Add(nuevoprestamo);


                if (LibroSeleccionado != null)
                {
                    LibroSeleccionado.Estado = true;
                }
            }
            OnPropertyChanged(nameof(ListaPrestamosActivos));
            GuardarPersistencia();
            UsuarioSeleccionado = null;
            LibroSeleccionado = null;
            PrestamoSeleccionado = null;
            LimpiarFormulario();
        }

        private void GuardarPersistencia()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(ListaPrestamos, options);
            File.WriteAllText(RutaPrestamos, json);
        }


        private void DevolverExecute(object parameter) 
        {
            if (PrestamoSeleccionado != null)
            {
               
                PrestamoSeleccionado.FechaDevolucion = DateTime.Now;
                PrestamoSeleccionado.Estado = false;

                
                var libroDevuelto = ListaLibros.FirstOrDefault(l => l.Titulo == PrestamoSeleccionado.Libro.Titulo);
                if (libroDevuelto != null)
                {
                    libroDevuelto.Estado = false; 
                }
                PrestamoSeleccionado.Estado = false;
                OnPropertyChanged(nameof(ListaPrestamosActivos));
                GuardarPersistencia();
                PrestamoSeleccionado = null;
                LimpiarFormulario();
            }
        }

        private bool CanExecuteDevolver(object parameter)
        {
            return PrestamoSeleccionado != null &&
                   PrestamoSeleccionado.Estado == true;
        }

        public ObservableCollection<Prestamo> ListaPrestamosActivos
        {
            get
            {
                var activos = ListaPrestamos.Where(p => p.Estado == true).ToList();
                return new ObservableCollection<Prestamo>(activos);
            }
        }


    }
}
