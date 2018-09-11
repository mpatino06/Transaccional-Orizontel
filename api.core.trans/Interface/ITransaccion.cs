using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.trans.Models;

namespace api.core.trans.Interface
{
	public interface ITransaccion
	{
		Transaccion GetBySecuencial(int code);
		List<Empresadenominacionfija> GetDenominacionMoneda(int empresa);
	}
}
