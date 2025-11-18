using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Biblioteca
{
    class LibroViewModel : ViewModelBase
    {
        private string RutaLibro = "libros.json";
        public ObservableCollection<Libro> ListaLibros { get; set; }

        private Libro libroSeleccionado;
        public Libro LibroSeleccionado
        {
            get => libroSeleccionado;
            set
            {
                libroSeleccionado = value;
                OnPropertyChanged();
                CargarDatosParaEdicion();
            }
        }

        private string titulo;
        public string Titulo
        {
            get => titulo;
            set { titulo = value; OnPropertyChanged(); }
        }

        private string autor;
        public string Autor
        {
            get => autor;
            set { autor = value; OnPropertyChanged(); }
        }

        private bool estado;
        public bool Estado
        {
            get => estado;
            set
            {
                estado = value;
                OnPropertyChanged();
            }
        }
        public ICommand GuardarCommand { get; private set; }
        public ICommand EliminarCommand { get; private set; }

        public LibroViewModel()
        {
            CargarLibro();
            GuardarCommand = new RelayCommand(GuardarLibroExecute, CanExecuteGuardar);
            EliminarCommand = new RelayCommand(EliminarLibroExecute, CanExecuteEliminar);
        }

        private void CargarLibro()
        {
            if (File.Exists(RutaLibro))
            {
                try
                {
                    var json = File.ReadAllText(RutaLibro);
                    var libro = JsonSerializer.Deserialize<ObservableCollection<Libro>>(json);
                    ListaLibros = libro ?? new ObservableCollection<Libro>();
                }
                catch { ListaLibros = new ObservableCollection<Libro>(); }
            }
            else
            {
                ListaLibros = new ObservableCollection<Libro>();
            }
        }
        private void CargarDatosParaEdicion()
        {
            if (LibroSeleccionado != null)
            {
                Titulo = LibroSeleccionado.Titulo;
                Autor = LibroSeleccionado.Autor;
            }
            else
            {
                LimpiarFormulario();
            }
        }
        private void LimpiarFormulario()
        {
            Titulo = string.Empty; 
            Autor=string.Empty;
        }

        private bool CanExecuteGuardar(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Titulo) && !string.IsNullOrWhiteSpace(Autor);
        }

        private void GuardarLibroExecute(object parameter)
        {
            if (LibroSeleccionado != null)
            {
                LibroSeleccionado.Titulo = Titulo;
                LibroSeleccionado.Autor = Autor;
                var temp = LibroSeleccionado;
                LibroSeleccionado = null;
                LibroSeleccionado = temp;
            }
            else
            {
                var nuevoLibro = new Libro (Titulo, Autor, Estado);
                ListaLibros.Add(nuevoLibro);
            }

            GuardarPersistencia();
            LibroSeleccionado = null;
        }
        private bool CanExecuteEliminar(object parameter)
        {
            return LibroSeleccionado != null;
        }
        private void EliminarLibroExecute(object parameter)
        {
            if (LibroSeleccionado != null)
            {
                ListaLibros.Remove(libroSeleccionado);
                GuardarPersistencia();
                LibroSeleccionado = null;
            }
        }
        private void GuardarPersistencia()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(ListaLibros, options);
            File.WriteAllText(RutaLibro, json);
        }
    }
}
