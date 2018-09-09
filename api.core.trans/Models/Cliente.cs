using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Comentariocliente = new HashSet<Comentariocliente>();
            Cuentacliente = new HashSet<Cuentacliente>();
            Movimientoimpresion = new HashSet<Movimientoimpresion>();
        }

        public int Secuencial { get; set; }
        public int Secuencialoficina { get; set; }
        public int Secuencialpersona { get; set; }
        public int Numerocliente { get; set; }
        public DateTime Fechaingreso { get; set; }
        public string Codigousuariooficial { get; set; }
        public string Codigosectoreconomico { get; set; }
        public string Codigotipovinculacion { get; set; }
        public string Codigocalificacioninterna { get; set; }
        public int Secuencialdivisionmercado { get; set; }
        public string Codigoestadocliente { get; set; }
        public int Numeroverificador { get; set; }

        public Usuario CodigousuariooficialNavigation { get; set; }
        public Division SecuencialdivisionmercadoNavigation { get; set; }
        public Oficina SecuencialoficinaNavigation { get; set; }
        public Persona SecuencialpersonaNavigation { get; set; }
        public ICollection<Comentariocliente> Comentariocliente { get; set; }
        public ICollection<Cuentacliente> Cuentacliente { get; set; }
        public ICollection<Movimientoimpresion> Movimientoimpresion { get; set; }
    }
}
