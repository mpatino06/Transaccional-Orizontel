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
	public class CienteServices : ICliente
	{
		private FBS_SacPelileoContext context;

		public CienteServices()
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
	}


}
