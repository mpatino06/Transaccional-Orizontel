using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.trans.Interface;
using api.core.trans.Models;

namespace api.core.trans.Services
{
	public class TransaccionServices : ITransaccion
	{

		private FBS_SacPelileoContext context;


		public TransaccionServices()
		{
			context = new FBS_SacPelileoContext();
		}

		public Transaccion GetBySecuencial(int code)
		{
			try
			{
				return context.Transaccion.FirstOrDefault(a => a.Secuencial == code);
			}
			catch (Exception)
			{
				return null;
				throw;
			}
		}

		public List<Empresadenominacionfija> GetDenominacionMoneda(int empresa)
		{
			try
			{
				return context.Empresadenominacionfija.Where(a => a.Secuencialempresa == empresa && a.Estaactiva == true).OrderBy(a => a.Orden).ToList();
			}
			catch (Exception)
			{
				return null;
				throw;
			}
		}

		public List<Transacciontipomovimiento> GetTransacciontipomovimientos(int secuencial)
		{
			try
			{
				return context.Transacciontipomovimiento.Where(a => a.SecuencialTransaccion == secuencial).ToList();
			}
			catch (Exception ex)
			{
				return null;
				throw;
			}
		}
	}
}
