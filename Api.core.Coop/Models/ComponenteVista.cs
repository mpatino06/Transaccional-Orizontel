using System;
using System.Collections.Generic;

namespace Api.core.Coop.Models
{
    public partial class ComponenteVista
    {
        public ComponenteVista()
        {
            CuentacomponenteVista = new HashSet<CuentacomponenteVista>();
            MovimientocuentacompVista = new HashSet<MovimientocuentacompVista>();
        }

        public int Secuencialcomponente { get; set; }
        public bool Admiteacreditacionprestamo { get; set; }
        public bool Espartesaldo { get; set; }
        public int Secuencialtipocomponentevista { get; set; }

        public ICollection<CuentacomponenteVista> CuentacomponenteVista { get; set; }
        public ICollection<MovimientocuentacompVista> MovimientocuentacompVista { get; set; }
    }
}
