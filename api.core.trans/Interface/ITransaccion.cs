using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.trans.ExtendModels;
using api.core.trans.Models;

namespace api.core.trans.Interface
{
	public interface ITransaccion
	{
		List<Transaccion> GetBySecuencialEmpresa(int Secuencialempresa);
		List<Empresadenominacionfija> GetDenominacionMoneda(int empresa);
		List<Transacciontipomovimiento> GetTransacciontipomovimientos(int secuencial);
		TransaccionMoneda GetTransaccionMonedas(int empresa, int secuencial);
		List<Banco> GetBancos();
		ResultTransaccion SaveTransaccion(RegistrarTransaccion model);
		List<TransaccionmobileExtend> GetTransaccionMobile(string codigoUsuario, string fecha);
	}
}
