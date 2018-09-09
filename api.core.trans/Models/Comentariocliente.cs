using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Comentariocliente
    {
        public int Secuencial { get; set; }
        public int Secuencialcliente { get; set; }
        public string Comentario { get; set; }
        public string Codigousuarioingreso { get; set; }
        public DateTime Fechaingreso { get; set; }
        public DateTime Fechamaquina { get; set; }
        public bool Estaactivo { get; set; }
        public int Numeroverificador { get; set; }

        public Usuario CodigousuarioingresoNavigation { get; set; }
        public Cliente SecuencialclienteNavigation { get; set; }
    }
}
