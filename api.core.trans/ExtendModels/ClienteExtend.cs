using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.ExtendModels
{
	public class ClienteExtend
	{

		public int SecuencialCliente { get; set; }
		public int SecuencialOficina { get; set; }
		public int SecuencialEmpresa { get; set; }
		public int SecuencialPersona { get; set; }
		public int NumeroCliente { get; set; }
		public DateTime FechaIngreso { get; set; }
		public string CodigoUsuarioOficial { get; set; }
		public string CodigoSectorEconomico { get; set; }
		public string CodigoTipoVinculacion { get; set; }
		public string CodigoCalificacionInterna { get; set; }
		public int SecuencialDivisionMercado { get; set; }
		public string CodigoEstadoCliente { get; set; }
		public int NumeroVerificadorCliente { get; set; }
		public string CodigoTipoIDentificacion { get; set; }
		public string NombreTipoIdentificacion { get; set; }
		public string Identificacion { get; set; }
		public string NombreUnido { get; set; }
		public string DireccionDomicilio { get; set; }
		public string ReferenciaDomiciliaria { get; set; }
		public string Email { get; set; }
		public int SecuencialTipoIdentificacion { get; set; }
		public int SecuencialDivPolResidencia { get; set; }
		public string CodigoPaisOrigen { get; set; }
		public int NumeroVerificador { get; set; } 
		public int SecuencialDivActEcon { get; set; }
	}
}
