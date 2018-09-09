using System;
using System.Collections.Generic;

namespace Api.core.Coop.Models
{
    public partial class UsuarioComplemento
    {
        public string Codigousuario { get; set; }
        public int Secuencialpersona { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Clave { get; set; }
        public string Emailinterno { get; set; }
        public DateTime Fechaultimocambioclave { get; set; }
        public bool Cambioclaveproximoingreso { get; set; }
        public int Periodicidadcambioclave { get; set; }
        public bool Esinterno { get; set; }
        public int Numeroverificador { get; set; }
        public bool Puedeconsultarotrosusuarios { get; set; }

        public Usuario CodigousuarioNavigation { get; set; }
        public Persona SecuencialpersonaNavigation { get; set; }
    }
}
