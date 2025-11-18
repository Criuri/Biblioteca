using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Biblioteca
{
    class Usuario
    {


        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Numero { get; set; }

        public string Correo { get; set; }

        public Usuario(string nombre, string apellido, string numero,string correo)
        {
            Nombre = nombre;
            Apellido = apellido;
            Correo = correo;
            Numero = numero;
        }

        public override string ToString()
        {
            return $"{Nombre} {Apellido}";
        }
    }
}
