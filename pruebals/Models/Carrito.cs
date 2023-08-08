using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pruebals.Models
{
    public class Carrito
    {
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public decimal valor { get; set; }
        public decimal total { get; set; }
    }
}