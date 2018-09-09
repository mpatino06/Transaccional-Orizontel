using System;
using System.Collections.Generic;

namespace Api.core.Coop.Models
{
    public partial class Producto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Siglas { get; set; }
        public int Secuencialmoneda { get; set; }
        public string Codigotipoproducto { get; set; }
        public bool Estaactivo { get; set; }
        public int Numeroverificador { get; set; }
    }
}
