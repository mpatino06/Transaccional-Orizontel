using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.ExtendModels
{
	public class RegistrarTransaccion
	{
		public string CodigoUsuario { get; set; }		
		public int SecuencialTransaccion { get; set; }
		public string NombreTransaccion { get; set; }
		public string SiglasTransaccion { get; set; }
		//CLIENTE
		public int secCliente { get; set; }
		public int numCliente { get; set; }
		public int SecEmpresa { get; set; }
		public int SecOficina { get; set; }
		//CUENTA SELECCIONADA
		public int SecuencialCuenta { get; set; }
		public string CodigoCuenta { get; set; }
		public int SecuencialMoneda { get; set; }
		//MONTO TRANSACCION
		public TransaccionMoneda Transacciones { get; set; } //Efectivo - Cheque
		//CHEQUES
		public List<Models.Cheque> Cheques { get; set; }
	}
}
