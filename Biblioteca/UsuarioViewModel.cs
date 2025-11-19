using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Biblioteca
{
    class UsuarioViewModel : ViewModelBase
    {
        private string RutaUsuario = "usuarios.json";
        public ObservableCollection<Usuario> ListaUsuarios { get; set; }

        private Usuario usuarioSeleccionado;
        public Usuario UsuarioSeleccionado
        {
            get => usuarioSeleccionado;
            set
            {
                usuarioSeleccionado = value;
                OnPropertyChanged();
                CargarDatosParaEdicion();
            }
        }

        private string nombre;
        public string Nombre
        {
            get => nombre;
            set { nombre = value; OnPropertyChanged(); }
        }

        private string apellido;
        public string Apellido
        {
            get => apellido;
            set { apellido = value; OnPropertyChanged(); }
        }

        private string numero;
        public string Numero
        {
            get => numero;
            set {numero = value; OnPropertyChanged(); }
        }

        private string correo;
        public string Correo
        {
            get => correo;
            set { correo = value; OnPropertyChanged(); }
        }

        public ICommand GuardarCommand { get; private set; }
        public ICommand EliminarCommand { get; private set; }

        public UsuarioViewModel()
        {
            CargarUsuarios();
            GuardarCommand = new RelayCommand(GuardarUsuarioExecute, CanExecuteGuardar);
            EliminarCommand = new RelayCommand(EliminarUsuarioExecute, CanExecuteEliminar);
        }

        private void CargarUsuarios()
        {
            if (File.Exists(RutaUsuario))
            {
                try
                {
                    var json = File.ReadAllText(RutaUsuario);
                    var usuarios = JsonSerializer.Deserialize<ObservableCollection<Usuario>>(json);
                    ListaUsuarios = usuarios ?? new ObservableCollection<Usuario>();
                }
                catch { ListaUsuarios = new ObservableCollection<Usuario>(); }
            }
            else
            {
                ListaUsuarios = new ObservableCollection<Usuario>();
            }
        }

        private void CargarDatosParaEdicion()
        {
            if (UsuarioSeleccionado != null)
            {
                Nombre = UsuarioSeleccionado.Nombre;
                Apellido = UsuarioSeleccionado.Apellido;
                Numero = UsuarioSeleccionado.Numero;
                Correo = UsuarioSeleccionado.Correo;
            }
            else
            {
                LimpiarFormulario();
            }
        }
        private void LimpiarFormulario()
        {
            Nombre = string.Empty;
            Apellido = string.Empty;
            Numero = string.Empty;
            Correo = string.Empty;
        }
        private bool CanExecuteGuardar(object parameter)
        {
            bool NumeroValido = long.TryParse(Numero, out _);
            return !string.IsNullOrWhiteSpace(Nombre) && !string.IsNullOrWhiteSpace(Numero) && (Correo?.Contains("@")==true) && NumeroValido ;
        }

        private void GuardarUsuarioExecute(object parameter)
        {
            if (UsuarioSeleccionado != null)
            {
                UsuarioSeleccionado.Nombre = Nombre;
                UsuarioSeleccionado.Apellido = Apellido;
                UsuarioSeleccionado.Numero = Numero;
                UsuarioSeleccionado.Correo = Correo;
                var temp = UsuarioSeleccionado;
                UsuarioSeleccionado = null;
                UsuarioSeleccionado = temp;
            }
            else
            {
                var nuevoUsuario = new Usuario(Nombre, Apellido, Numero,Correo);
                ListaUsuarios.Add(nuevoUsuario);
            }

            GuardarPersistencia();
            UsuarioSeleccionado = null;
        }

        private bool CanExecuteEliminar(object parameter)
        {
            return UsuarioSeleccionado != null;
        }

        private void EliminarUsuarioExecute(object parameter)
        {
            if (UsuarioSeleccionado != null)
            {
                ListaUsuarios.Remove(UsuarioSeleccionado);
                GuardarPersistencia();
                UsuarioSeleccionado = null;
            }
        }

        private void GuardarPersistencia()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(ListaUsuarios, options);
            File.WriteAllText(RutaUsuario, json);
        }
    }
}
