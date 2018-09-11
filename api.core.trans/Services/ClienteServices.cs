using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using api.core.trans.ExtendModels;
using api.core.trans.Interface;
using api.core.trans.Models;
using api.core.trans.Utility;

namespace api.core.trans.Services
{ 
	public class ClienteServices : ICliente
	{
		private FBS_SacPelileoContext context;

		public ClienteServices()
		{
			context = new FBS_SacPelileoContext();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="code"></param>
		/// <param name="seleccion">0. Por Secuencial 1. Por NumeroCliente </param>
		/// <returns></returns>
		public Cliente GetCliente(int code, int seleccion)
		{
			Cliente cliente = new Cliente();
			try
			{
				cliente = (seleccion == 0)? context.Cliente.FirstOrDefault(a => a.Secuencial == code) : context.Cliente.FirstOrDefault(a => a.Numerocliente == code);
			}
			catch (Exception ex)
			{
				return null;
				throw;
			}

			return cliente;
		}

		public ClienteExtend GetClienteBySecEmpresaYNumeroCliente(int secEmpresa, int numCliente)
		{
			ClienteExtend clienteExtend = new ClienteExtend();
			try
			{
				string qry = "";
				qry = "SELECT ";
				qry += "C.SECUENCIAL, ";
				qry += "C.SECUENCIALOFICINA, ";
				qry += "C.SECUENCIALPERSONA, ";
				qry += "C.NUMEROCLIENTE, ";
				qry += "C.FECHAINGRESO, ";
				qry += "C.CODIGOUSUARIOOFICIAL, ";
				qry += "C.CODIGOSECTORECONOMICO, ";
				qry += "C.CODIGOTIPOVINCULACION, ";
				qry += "C.CODIGOCALIFICACIONINTERNA, ";
				qry += "C.SECUENCIALDIVISIONMERCADO, ";
				qry += "C.CODIGOESTADOCLIENTE, ";
				qry += "C.NUMEROVERIFICADOR, ";
				qry += "TI.CODIGO, ";
				qry += "TI.NOMBRE, ";
				qry += "P.IDENTIFICACION, ";
				qry += "P.NOMBREUNIDO, ";
				qry += "P.DIRECCIONDOMICILIO, ";
				qry += "P.REFERENCIADOMICILIARIA, ";
				qry += "P.EMAIL, ";
				qry += "P.SECUENCIALTIPOIDENTIFICACION, ";
				qry += "P.SECUENCIALDIVPOLRESIDENCIA, ";
				qry += "P.CODIGOPAISORIGEN, ";
				qry += "NUMEROVERIFICADOR, ";
				qry += "P.SECUENCIALDIVACTIVIDADECON ";
				qry += "FROM FBS_CLIENTES.CLIENTE AS C INNER JOIN ";
				qry += "FBS_ORGANIZACIONES.OFICINA AS O ON C.SECUENCIALOFICINA = O.SECUENCIALDIVISION INNER JOIN ";
				qry += "FBS_PERSONAS.PERSONA AS P ON C.SECUENCIALPERSONA = p.SECUENCIAL INNER JOIN ";
				qry += "FBS_PERSONAS.TIPOIDENTIFICACION AS TI ON P.SECUENCIALTIPOIDENTIFICACION = TI.SECUENCIAL ";
				qry += "WHERE(O.SECUENCIALEMPRESA ='" + secEmpresa + "') AND(C.NUMEROCLIENTE ='" + numCliente + "')";

				using (SqlConnection conn = new SqlConnection(SQLHelper.ConnectionString))
				{
					SqlDataReader dr = SQLHelper.ExecuteReader(conn, System.Data.CommandType.Text, qry, null);
					if (dr.HasRows)
					{
						foreach (DbDataRecord c in dr.Cast<DbDataRecord>())
						{
							clienteExtend.SecuencialCliente = c.GetInt32(0);
							clienteExtend.SecuencialOficina = c.GetInt32(1);
							clienteExtend.SecuencialPersona = c.GetInt32(2);
							clienteExtend.NumeroCliente = c.GetInt32(3);
							clienteExtend.FechaIngreso = c.GetDateTime(4);
							clienteExtend.CodigoUsuarioOficial = c.GetString(5);
							clienteExtend.CodigoSectorEconomico = c.GetString(6);
							clienteExtend.CodigoTipoVinculacion = c.GetString(7);
							clienteExtend.CodigoCalificacionInterna = c.GetString(8);
							clienteExtend.SecuencialDivisionMercado = c.GetInt32(9);
							clienteExtend.CodigoEstadoCliente = c.GetString(10);
							clienteExtend.NumeroVerificadorCliente = c.GetInt32(11);
							clienteExtend.CodigoTipoIDentificacion = c.GetString(12);
							clienteExtend.NombreTipoIdentificacion = c.GetString(13);
							clienteExtend.Identificacion = c.GetInt32(14);
							clienteExtend.NombreUnido = c.GetString(15);
							clienteExtend.DireccionDomicilio = c.GetString(16);
							clienteExtend.ReferenciaDomiciliaria = c.GetString(17);
							clienteExtend.Email = c.GetString(18);
							clienteExtend.SecuencialTipoIdentificacion = c.GetInt32(19);
							clienteExtend.SecuencialDivPolResidencia = c.GetInt32(20);
							clienteExtend.CodigoPaisOrigen = c.GetString(21);
							clienteExtend.NumeroVerificador = c.GetInt32(22);
							clienteExtend.SecuencialDivActEcon = c.GetInt32(23);
						}
					}
				}

			}
			catch (Exception)
			{
				clienteExtend = null;
				throw;
			}
			return clienteExtend;
		}

		public List<Comentariocliente> GetComentarioCliente(int cliente, bool activo)
		{
			List<Comentariocliente> comentarioclientes = new List<Comentariocliente>();
			try
			{
				comentarioclientes = context.Comentariocliente.Where(a => a.Secuencialcliente == cliente && a.Estaactivo == activo).ToList();
			}
			catch (Exception)
			{
				comentarioclientes = null;
				throw;
			}
			return comentarioclientes;
		}

		public List<ClienteCuentas> GetCuentasCliente(int cliente, int transaccion)
		{
			List<ClienteCuentas> clienteCuentas = new List<ClienteCuentas>(); 
			try
			{
				string qry = string.Empty;

				qry = "SELECT CUENTAMAESTRO.SECUENCIAL, ";
				qry += "CUENTAMAESTRO.CODIGO, ";
				qry += "TIPOCUENTA.NOMBRE, ";
				qry += "CUENTAMAESTRO.CODIGOTIPOCUENTA, ";
				qry += "CUENTAMAESTRO.CODIGOPRODUCTOVISTA, ";
				qry += "PRODUCTO.NOMBRE, ";
				qry += "CUENTAMAESTRO.CODIGOESTADO, ";
				qry += "ESTADOCUENTA.NOMBRE, ";
				qry += "CUENTAMAESTRO.SECUENCIALMONEDA, ";
				qry += "CUENTAMAESTRO.SECUENCIALOFICINA,";
				qry += "DIVISION.NOMBRE, ";
				qry += "CUENTAMAESTRO.CODIGOUSUARIOOFICIAL, ";
				qry += "CUENTAMAESTRO.FECHASISTEMACREACION, ";
				qry += "CUENTAMAESTRO.FECHAMAQUINACREACION, ";
				qry += "CUENTAMAESTRO.NUMEROLIBRETA, ";
				qry += "CUENTAMAESTRO.NUMEROLINEAIMPRIMELIBRETA, ";
				qry += "CUENTAMAESTRO.ESANVERSO, ";
				qry += "CUENTAMAESTRO.TIENESEGUROACTIVO, ";
				qry += "CUENTAMAESTRO.FECHACORTE, ";
				qry += "CUENTAMAESTRO.BLOQUEADATRANSACCIONOPERATIVA, ";
				qry += "CUENTAMAESTRO.NUMEROVERIFICADOR ";
				qry += "FROM  FBS_CAPTACIONESVISTA.CUENTAMAESTRO , FBS_CAPTACIONESVISTA.CUENTACLIENTE , FBS_CAPTACIONESVISTA.TIPOCUENTA, FBS_CAPTACIONESVISTA.ESTADOCUENTA, FBS_NEGOCIOSFINANCIEROS.PRODUCTO, FBS_GENERALES.DIVISION ";
				qry += "WHERE CUENTACLIENTE.SECUENCIALCUENTA = CUENTAMAESTRO.SECUENCIAL AND ";
				qry += "CUENTACLIENTE.SECUENCIALCLIENTE ='" + cliente + "' AND ";
				qry += "CUENTACLIENTE.ESTAACTIVO = 1 AND ";
				qry += "TIPOCUENTA.CODIGO = CUENTAMAESTRO.CODIGOTIPOCUENTA AND ";
				qry += "CUENTAMAESTRO.BLOQUEADATRANSACCIONOPERATIVA = 0  AND ";
				qry += "CUENTAMAESTRO.CODIGOESTADO NOT IN('P', 'B', 'I', 'C') AND ";
				qry += "CUENTAMAESTRO.SECUENCIAL IN(SELECT CUENTACOMPONENTE_VISTA.SECUENCIALCUENTA FROM FBS_CAPTACIONESVISTA.CUENTACOMPONENTE_VISTA WHERE CUENTACOMPONENTE_VISTA.SECUENCIALCUENTA = CUENTAMAESTRO.SECUENCIAL AND CUENTACOMPONENTE_VISTA.SECUENCIALCOMPONENTEVISTA IN (SELECT TRANSACCIONCOMPONENTE.SECUENCIALCOMPONENTE ";
				qry += "FROM FBS_NEGOCIOSFINANCIEROS.TRANSACCIONCOMPONENTE ";
				qry += "WHERE TRANSACCIONCOMPONENTE.ESTAACTIVA = 1 AND ";
				qry += "TRANSACCIONCOMPONENTE.SECUENCIALTRANSACCION ='" + transaccion + "')) AND ";
				qry += "CUENTAMAESTRO.CODIGOESTADO = ESTADOCUENTA.CODIGO AND ";
				qry += "CUENTAMAESTRO.CODIGOPRODUCTOVISTA = PRODUCTO.CODIGO AND ";
				qry += " CUENTAMAESTRO.SECUENCIALOFICINA = DIVISION.SECUENCIAL ";
				qry += "ORDER BY TIPOCUENTA.NOMBRE";

				using (SqlConnection conn = new SqlConnection(SQLHelper.ConnectionString))
				{
					SqlDataReader dr = SQLHelper.ExecuteReader(conn, System.Data.CommandType.Text, qry, null);
					if (dr.HasRows)
					{
						foreach (DbDataRecord c in dr.Cast<DbDataRecord>())
						{

							clienteCuentas.Add(new ClienteCuentas
							{
								Secuencial = c.GetInt32(0),
								Codigo = c.GetString(1),
								NombreCuenta = c.GetString(2),
								CodigoTipoCuenta = c.GetInt32(3),
								CodigoProductoVista = c.GetInt32(4),
								NombreProducto = c.GetString(5),
								CodigoEstado = c.GetString(6),
								NombreEstado = c.GetString(7),
								SecuencialMoneda = c.GetInt32(8),
								SecuencialOficina = c.GetInt32(9),
								NombreDivision = c.GetString(10),
								CodigoUsuarioOficial = c.GetString(11),
								FechaSistemaCreacion = c.GetDateTime(12),
								FechaMaquinaCreacion = c.GetDateTime(13),
								NumeroLibreta = c.GetInt32(14),
								EsAnverso = c.GetBoolean(15),
								TieneSeguroActivo = c.GetBoolean(16),
								FechaCorte = c.GetDateTime(17),
								BloqeadaTransaccionOperativa = c.GetBoolean(18),
								NumeroVerificador = c.GetInt32(19)
							});
						}
					}
				}

			}
			catch (Exception)
			{

				throw;
			}
			return clienteCuentas;

		}

		public CuentacomponenteVista GetSaldoCuenta(int cuenta, int componente)
		{
			try
			{
				return context.CuentacomponenteVista.FirstOrDefault(a=> a.Secuencial == cuenta && a.Secuencialcomponentevista == componente);
			}
			catch (Exception)
			{
				return null;
				throw;
			}

		}


	}
}
