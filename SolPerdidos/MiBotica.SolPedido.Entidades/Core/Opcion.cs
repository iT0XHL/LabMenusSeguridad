using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiBotica.SolPedido.Entidades.Core
{
    public class Opcion
    {
        public int IdOpcion { get; set; }
        public string NombreOpcion { get; set; }
        public string UrlOpcion { get; set; }
        public string RutaImagen { get; set; }
        public int? NroOrden { get; set; }   // como en SQL puede ser NULL -> nullable
        public int? IdOpcionRef { get; set; } // idem, puede ser NULL

        // Propiedades para el menú
        public string Area { get; set; }
        public string Controladora { get; set; }
        public string Accion { get; set; }
        public List<Opcion> Hijos { get; set; } = new List<Opcion>();
    }
}
