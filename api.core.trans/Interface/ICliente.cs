using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.trans.ExtendModels;
using api.core.trans.Models;

namespace api.core.trans.Interface
{
	public interface ICliente
	{
		Cliente GetCliente(int code, int seleccion);
		ClienteExtend GetClienteBySecEmpresaYNumeroCliente(int secEmpresa, int numCliente);
		List<Comentariocliente> GetComentarioCliente(int cliente, bool activo);
		List<ClienteCuentas> GetCuentasCliente(int cliente, int transaccion);
		CuentacomponenteVista GetSaldoCuenta(int cuenta, int componente);
	}
}
