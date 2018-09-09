using System;
using System.Collections.Generic;

namespace Api.core.Coop.Models
{
    public partial class Transaccion
    {
        public Transaccion()
        {
            Movimientodetalle = new HashSet<Movimientodetalle>();
            Transaccioncomponente = new HashSet<Transaccioncomponente>();
            Transaccionrangoaprobacion = new HashSet<Transaccionrangoaprobacion>();
        }

        public int Secuencial { get; set; }
        public string Codigo { get; set; }
        public int Secuencialempresa { get; set; }
        public string Nombre { get; set; }
        public string Siglas { get; set; }
        public bool Esdebito { get; set; }
        public bool Esoperable { get; set; }
        public bool Esvisible { get; set; }
        public bool Requiereconcepto { get; set; }
        public string Codigotipoproducto { get; set; }
        public bool Usuariodefineorigen { get; set; }
        public bool Requierealmacenarpapeleta { get; set; }
        public bool Estaactiva { get; set; }
        public int Numeroverificador { get; set; }
        public bool? Verificahuella { get; set; }
        public bool? Facturarenlinea { get; set; }
        public bool? Esfacturable { get; set; }

        public TransaccionVista TransaccionVista { get; set; }
        public ICollection<Movimientodetalle> Movimientodetalle { get; set; }
        public ICollection<Transaccioncomponente> Transaccioncomponente { get; set; }
        public ICollection<Transaccionrangoaprobacion> Transaccionrangoaprobacion { get; set; }
    }
}
