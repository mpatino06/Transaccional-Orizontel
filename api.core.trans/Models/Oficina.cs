using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Oficina
    {
        public Oficina()
        {
            Cliente = new HashSet<Cliente>();
            Cuentamaestro = new HashSet<Cuentamaestro>();
            Movimiento = new HashSet<Movimiento>();
            Movimientodetalle = new HashSet<Movimientodetalle>();
            Usuario = new HashSet<Usuario>();
            Ventanilla = new HashSet<Ventanilla>();
        }

        public int Secuencialdivision { get; set; }
        public int Secuencialempresa { get; set; }
        public int Secuencialpersonaorganizacion { get; set; }
        public string Codigooficinacontrol { get; set; }
        public string Siglas { get; set; }
        public string Ciudad { get; set; }
        public int Numerocontable { get; set; }
        public bool Esoperativa { get; set; }
        public DateTime Fechacierrecontable { get; set; }
        public string Cadenaconexionbasedatoslocal { get; set; }
        public string Servidorimagenes { get; set; }
        public bool Estaactiva { get; set; }
        public int Numeroverificador { get; set; }
        public string Servidorswitch { get; set; }
        public int Puertoswitch { get; set; }
        public string Codigoregion { get; set; }
        public string Codigoagenciacontrol { get; set; }
        public string Codigooficinaseps { get; set; }

        public Division SecuencialdivisionNavigation { get; set; }
        public ICollection<Cliente> Cliente { get; set; }
        public ICollection<Cuentamaestro> Cuentamaestro { get; set; }
        public ICollection<Movimiento> Movimiento { get; set; }
        public ICollection<Movimientodetalle> Movimientodetalle { get; set; }
        public ICollection<Usuario> Usuario { get; set; }
        public ICollection<Ventanilla> Ventanilla { get; set; }
    }
}
