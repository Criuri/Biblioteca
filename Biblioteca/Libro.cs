using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Biblioteca
{
    class Libro : ViewModelBase
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        private bool estado;

        public bool Estado
        {
            get => estado;
            set
            {
                if (estado != value)
                {
                    estado = value;
                    OnPropertyChanged();
                }
            }
        }
        public Libro() { }
        public Libro(string titulo, string autor, bool estado)
        {
            Titulo = titulo;
            Autor = autor;
            Estado = estado;
        }
    }
}
