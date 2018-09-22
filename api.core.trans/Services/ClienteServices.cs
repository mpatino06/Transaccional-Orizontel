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
				qry += "P.NUMEROVERIFICADOR, ";
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
						while (dr.Read())
						{
							clienteExtend.SecuencialCliente = dr.GetInt32(0);
							clienteExtend.SecuencialOficina = dr.GetInt32(1);
							clienteExtend.SecuencialPersona = dr.GetInt32(2);
							clienteExtend.NumeroCliente = dr.GetInt32(3);
							clienteExtend.FechaIngreso = dr.GetDateTime(4);
							clienteExtend.CodigoUsuarioOficial = dr.GetString(5);
							clienteExtend.CodigoSectorEconomico = dr.GetString(6);
							clienteExtend.CodigoTipoVinculacion = dr.GetString(7);
							clienteExtend.CodigoCalificacionInterna = dr.GetString(8);
							clienteExtend.SecuencialDivisionMercado = dr.GetInt32(9);
							clienteExtend.CodigoEstadoCliente = dr.GetString(10);
							clienteExtend.NumeroVerificadorCliente = dr.GetInt32(11);
							clienteExtend.CodigoTipoIDentificacion = dr.GetString(12);
							clienteExtend.NombreTipoIdentificacion = dr.GetString(13);
							clienteExtend.Identificacion = dr.GetString(14);
							clienteExtend.NombreUnido = dr.GetString(15);
							clienteExtend.DireccionDomicilio = dr.GetString(16);
							clienteExtend.ReferenciaDomiciliaria = dr.GetString(17);
							clienteExtend.Email = dr.GetString(18);
							clienteExtend.SecuencialTipoIdentificacion = dr.GetInt32(19);
							clienteExtend.SecuencialDivPolResidencia = dr.GetInt32(20);
							clienteExtend.CodigoPaisOrigen = dr.GetString(21);
							clienteExtend.NumeroVerificador = dr.GetInt32(22);
							clienteExtend.SecuencialDivActEcon = dr.GetInt32(23);
						}
					}
				}
			}
			catch (Exception ex)
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
				qry += "MONEDA.NOMBRE AS NOMBREMONEDA, ";
				qry += "CUENTAMAESTRO.SECUENCIALOFICINA,";
				qry += "OFICINA.SECUENCIALEMPRESA, ";
				qry += "DIVISION.NOMBRE, ";
				qry += "CUENTAMAESTRO.CODIGOUSUARIOOFICIAL, ";
				qry += "USUARIO.NOMBRE AS NOMBREUSUARIO, ";
				qry += "CUENTAMAESTRO.FECHASISTEMACREACION, ";
				qry += "CUENTAMAESTRO.FECHAMAQUINACREACION, ";
				qry += "CUENTAMAESTRO.NUMEROLIBRETA, ";
				qry += "CUENTAMAESTRO.NUMEROLINEAIMPRIMELIBRETA, ";
				qry += "CUENTAMAESTRO.ESANVERSO, ";
				qry += "CUENTAMAESTRO.TIENESEGUROACTIVO, ";
				qry += "CUENTAMAESTRO.FECHACORTE, ";
				qry += "CUENTAMAESTRO.BLOQUEADATRANSACCIONOPERATIVA, ";
				qry += "CUENTAMAESTRO.NUMEROVERIFICADOR ";
				qry += "FROM  FBS_CAPTACIONESVISTA.CUENTAMAESTRO , FBS_CAPTACIONESVISTA.CUENTACLIENTE , FBS_CAPTACIONESVISTA.TIPOCUENTA, FBS_CAPTACIONESVISTA.ESTADOCUENTA, FBS_NEGOCIOSFINANCIEROS.PRODUCTO, FBS_GENERALES.DIVISION, FBS_GENERALES.MONEDA, FBS_SEGURIDADES.USUARIO, FBS_ORGANIZACIONES.OFICINA ";
				qry += "WHERE CUENTACLIENTE.SECUENCIALCUENTA = CUENTAMAESTRO.SECUENCIAL AND ";
				qry += "CUENTACLIENTE.SECUENCIALCLIENTE ='" + cliente + "' AND ";
				qry += "CUENTACLIENTE.ESTAACTIVO = 1 AND ";
				qry += "TIPOCUENTA.CODIGO = CUENTAMAESTRO.CODIGOTIPOCUENTA AND ";
				qry += "CUENTAMAESTRO.BLOQUEADATRANSACCIONOPERATIVA = 0  AND ";
				qry += "CUENTAMAESTRO.CODIGOESTADO NOT IN('P', 'B', 'I', 'C') AND ";
				qry += "MONEDA.SECUENCIAL = CUENTAMAESTRO.SECUENCIALMONEDA AND ";
				qry += "USUARIO.CODIGO = CUENTAMAESTRO.CODIGOUSUARIOOFICIAL AND ";
				qry += "CUENTAMAESTRO.SECUENCIALOFICINA = OFICINA.SECUENCIALDIVISION AND ";
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
								CodigoTipoCuenta = c.GetString(3),
								CodigoProductoVista = c.GetString(4),
								NombreProducto = c.GetString(5),
								CodigoEstado = c.GetString(6),
								NombreEstado = c.GetString(7).Trim(),
								SecuencialMoneda = c.GetInt32(8),
								NombreMondea = c.GetString(9),
								SecuencialOficina = c.GetInt32(10),
								SecuencialEmpresa = c.GetInt32(11),
								NombreDivision = c.GetString(12),
								CodigoUsuarioOficial = c.GetString(13),
								NombreUsuario = c.GetString(14),
								FechaSistemaCreacion = c.GetDateTime(15),
								FechaMaquinaCreacion = c.GetDateTime(16),
								NumeroLibreta = c.GetInt32(17),
								NumeroLineaImprimeLibreta = c.GetInt32(18),
								EsAnverso = c.GetBoolean(19),
								TieneSeguroActivo = c.GetBoolean(20),
								FechaCorte = c.GetDateTime(21),
								BloqeadaTransaccionOperativa = c.GetBoolean(22),
								NumeroVerificador = c.GetInt32(23)
							});
						}
					}
				}

			}
			catch (Exception ex)
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
