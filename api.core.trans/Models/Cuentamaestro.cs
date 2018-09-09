using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Cuentamaestro
    {
        public Cuentamaestro()
        {
            Cuentacliente = new HashSet<Cuentacliente>();
            CuentacomponenteVista = new HashSet<CuentacomponenteVista>();
            MovimientocuentacompVista = new HashSet<MovimientocuentacompVista>();
            MovimientodetalleCuenta = new HashSet<MovimientodetalleCuenta>();
            Movimientoimpresion = new HashSet<Movimientoimpresion>();
        }

        public int Secuencial { get; set; }
        public string Codigo { get; set; }
        public string Codigotipocuenta { get; set; }
        public string Codigoproductovista { get; set; }
        public string Codigoestado { get; set; }
        public int Secuencialmoneda { get; set; }
        public int Secuencialoficina { get; set; }
        public string Codigousuariooficial { get; set; }
        public DateTime Fechasistemacreacion { get; set; }
        public DateTime Fechamaquinacreacion { get; set; }
        public int Numerolibreta { get; set; }
        public int Numerolineaimprimelibreta { get; set; }
        public bool Esanverso { get; set; }
        public bool Tieneseguroactivo { get; set; }
        public DateTime? Fechacorte { get; set; }
        public bool Bloqueadatransaccionoperativa { get; set; }
        public int Numeroverificador { get; set; }

        public Estadocuenta CodigoestadoNavigation { get; set; }
        public Tipocuenta CodigotipocuentaNavigation { get; set; }
        public Usuario CodigousuariooficialNavigation { get; set; }
        public Oficina SecuencialoficinaNavigation { get; set; }
        public ICollection<Cuentacliente> Cuentacliente { get; set; }
        public ICollection<CuentacomponenteVista> CuentacomponenteVista { get; set; }
        public ICollection<MovimientocuentacompVista> MovimientocuentacompVista { get; set; }
        public ICollection<MovimientodetalleCuenta> MovimientodetalleCuenta { get; set; }
        public ICollection<Movimientoimpresion> Movimientoimpresion { get; set; }
    }
}
