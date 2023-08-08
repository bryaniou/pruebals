using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pruebals.Models
{
    public class Producto
    {
        public int id_producto { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }

        public decimal precio { get; set; }

        public int stock { get; set; }
        public bool status { get; set; }

        public string descripcion { get; set; }
    }
}