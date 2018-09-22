using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.trans.Models;

namespace api.core.trans.ExtendModels
{
	public class TransaccionMoneda
	{
		public List<Empresadenominacionfija> DenominacionMoneda { get; set; }
		public List<Transacciontipomovimiento> TipoMovimiento { get; set; }

	}
}
