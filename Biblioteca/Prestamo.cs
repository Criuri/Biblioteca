using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Biblioteca
{
    class Prestamo
    {
        private DateTime? fechaPrestamo;
        private DateTime? fechaDevolucion;

        public Usuario Usuario { get; set; }

        public Libro Libro { get; set; }

        public DateTime? FechaPrestamo { get; set; }

        public DateTime? FechaDevolucion { get; set; }

        public bool Estado { get; set; }
        public Prestamo()
        {
        }
        public Prestamo(Usuario usuario, Libro libro, DateTime? fechaPrestamo, DateTime? fechaDevolucion, bool estado)
        {
            Usuario = usuario;
            Libro = libro;
            FechaPrestamo = fechaPrestamo;
            FechaDevolucion = fechaDevolucion;
            Estado = estado;
        }
    }
}
