using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Movimiento
    {
        public Movimiento()
        {
            Movimientodetalle = new HashSet<Movimientodetalle>();
        }

        public int Secuencial { get; set; }
        public string Documento { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Fechamaquina { get; set; }
        public string Codigousuario { get; set; }
        public int Secuencialoficinausuario { get; set; }
        public bool Estaimpreso { get; set; }
        public int Numeroverificador { get; set; }

        public Usuario CodigousuarioNavigation { get; set; }
        public Oficina SecuencialoficinausuarioNavigation { get; set; }
        public ICollection<Movimientodetalle> Movimientodetalle { get; set; }
    }
}
