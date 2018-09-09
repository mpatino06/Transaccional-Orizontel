using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Cliente = new HashSet<Cliente>();
            Comentariocliente = new HashSet<Comentariocliente>();
            Cuentamaestro = new HashSet<Cuentamaestro>();
            Movimiento = new HashSet<Movimiento>();
            Ventanilla = new HashSet<Ventanilla>();
        }

        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Secuencialoficina { get; set; }
        public bool Estaactivo { get; set; }
        public int Numeroverificador { get; set; }

        public Oficina SecuencialoficinaNavigation { get; set; }
        public UsuarioComplemento UsuarioComplemento { get; set; }
        public ICollection<Cliente> Cliente { get; set; }
        public ICollection<Comentariocliente> Comentariocliente { get; set; }
        public ICollection<Cuentamaestro> Cuentamaestro { get; set; }
        public ICollection<Movimiento> Movimiento { get; set; }
        public ICollection<Ventanilla> Ventanilla { get; set; }
    }
}
