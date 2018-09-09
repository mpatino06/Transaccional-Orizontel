using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Tipocuenta
    {
        public Tipocuenta()
        {
            Cuentamaestro = new HashSet<Cuentamaestro>();
        }

        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Siglas { get; set; }
        public bool Esoperativo { get; set; }
        public int Secuencialempresa { get; set; }
        public bool Aceptamulticuenta { get; set; }
        public bool Aceptasobregiroautomatico { get; set; }
        public bool Provisionainteres { get; set; }
        public decimal Saldominimo { get; set; }
        public bool Estaactivo { get; set; }
        public int Numeroverificador { get; set; }

        public ICollection<Cuentamaestro> Cuentamaestro { get; set; }
    }
}
