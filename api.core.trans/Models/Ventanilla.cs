using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Ventanilla
    {
        public Ventanilla()
        {
            VentanillacomponenteCaja = new HashSet<VentanillacomponenteCaja>();
        }

        public int Secuencial { get; set; }
        public string Codigousuario { get; set; }
        public int Secuencialoficina { get; set; }
        public DateTime Fecha { get; set; }
        public bool Abiertaautomaticamente { get; set; }
        public bool Estacerrada { get; set; }
        public bool Estacuadrada { get; set; }
        public int Numerovecescuadrada { get; set; }
        public int Numeroverificador { get; set; }

        public Usuario CodigousuarioNavigation { get; set; }
        public Oficina SecuencialoficinaNavigation { get; set; }
        public ICollection<VentanillacomponenteCaja> VentanillacomponenteCaja { get; set; }
    }
}
