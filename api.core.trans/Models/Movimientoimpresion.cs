using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Movimientoimpresion
    {
        public int Secuencial { get; set; }
        public DateTime Fecha { get; set; }
        public string Depositos { get; set; }
        public string Retiros { get; set; }
        public string Saldo { get; set; }
        public string Transaccion { get; set; }
        public int Secuencialcliente { get; set; }
        public int Secuencialcuenta { get; set; }
        public string Operador { get; set; }
        public bool Estaimpresa { get; set; }
        public int Numeoverificador { get; set; }
        public string Efectivo { get; set; }
        public string Cheque { get; set; }
        public string Saldodisponible { get; set; }
        public string Saldoobligatorios { get; set; }
        public string Valortransaccion { get; set; }
        public bool Eslinearendfinanc { get; set; }
        public string Detallerendfinanc { get; set; }

        public Cliente SecuencialclienteNavigation { get; set; }
        public Cuentamaestro SecuencialcuentaNavigation { get; set; }
    }
}
