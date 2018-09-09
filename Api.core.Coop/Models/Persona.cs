using System;
using System.Collections.Generic;

namespace Api.core.Coop.Models
{
    public partial class Persona
    {
        public Persona()
        {
            UsuarioComplemento = new HashSet<UsuarioComplemento>();
        }

        public int Secuencial { get; set; }
        public string Identificacion { get; set; }
        public string Nombreunido { get; set; }
        public string Direcciondomicilio { get; set; }
        public string Referenciadomiciliaria { get; set; }
        public string Email { get; set; }
        public int Secuencialtipoidentificacion { get; set; }
        public int Secuencialdivpolresidencia { get; set; }
        public string Codigopaisorigen { get; set; }
        public int Numeroverificador { get; set; }
        public int Secuencialdivactividadecon { get; set; }
        public string Codigosociomigra { get; set; }
        public string Identificacionmigra { get; set; }

        public Division SecuencialdivactividadeconNavigation { get; set; }
        public Division SecuencialdivpolresidenciaNavigation { get; set; }
        public Tipoidentificacion SecuencialtipoidentificacionNavigation { get; set; }
        public Cliente Cliente { get; set; }
        public ICollection<UsuarioComplemento> UsuarioComplemento { get; set; }
    }
}
