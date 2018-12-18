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
using log4net.Config;

namespace api.core.trans.Services
{
	public class TransaccionServices : ITransaccion
	{

		private FBS_SacPelileoContext context;

		private Log4 errorLog;

		public TransaccionServices()
		{
			context = new FBS_SacPelileoContext();
			errorLog = new Log4();
		}

		public List<Transaccion> GetBySecuencialEmpresa(int Secuencialempresa)
		{
			try
			{
				int[] secuencial = { 18, 19 };

				return context.Transaccion.Where(a => a.Secuencialempresa == Secuencialempresa && secuencial.Contains(a.Secuencial)).ToList();
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
			catch (Exception ex)
			{
				errorLog.MainLog("GetMoneda " + ex.Message.ToString());
				return null;
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

		public TransaccionMoneda GetTransaccionMonedas(int empresa, int secuencial)
		{
			var list = new TransaccionMoneda();
			try
			{
				list.DenominacionMoneda = GetDenominacionMoneda(empresa);
				list.TipoMovimiento = GetTransacciontipomovimientos(secuencial);
			}
			catch (Exception)
			{
				list = null;
				throw;
			}
			return list;
		}


		public ResultTransaccion SaveTransaccion(RegistrarTransaccion model)
		{
			ResultTransaccion result = new ResultTransaccion();
			result.Result = false;
			string tableName = "INCIO";
			using (var transaction = context.Database.BeginTransaction())
			{
				try
				{
					if (AccesUsarioRol(model.CodigoUsuario, "003"))
					{
						bool denominacionAcetapda = true;
						decimal saldoTotalCuentas = 0;
						decimal montoTransaccionTotal = model.Transacciones.TipoMovimiento.Sum(a => a.ValueInsert);
						decimal monttoTransaccionEfectivo = model.Transacciones.TipoMovimiento.FirstOrDefault(a => a.Codigotipomovimiento == "Efectivo").ValueInsert;
						decimal monttoTransaccionCheque = model.Transacciones.TipoMovimiento.FirstOrDefault(a => a.Codigotipomovimiento == "Cheque").ValueInsert;
						DateTime fechaCajero = context.Calendario.Where(a => a.EstaCerrado == false && a.EsFeriado == false).Min(a => a.FechaSistema);

						int contMenoraCero = model.Transacciones.DenominacionMoneda.Where(a => a.ValueInsert < 0).Count();

						if (contMenoraCero > 0)
						{
							string resultValidacion = ExixteDenominacionEfectivo(model, monttoTransaccionEfectivo, fechaCajero);
							if (!string.IsNullOrEmpty(resultValidacion))
							{
								denominacionAcetapda = false;

								result.Saldodeposito = 0;
								result.Result = false;
								result.MessageResult = resultValidacion;
								result.SecuencialDocumento = "0";
							}
						}

						if (denominacionAcetapda)
						{
							//CON 8
							tableName = "EmpresaDocumento";
							var _empresaDocumento = context.EmpresaDocumento.FirstOrDefault(a => a.Secuencialempresa == model.SecEmpresa);
							if (_empresaDocumento != null)
							{
								//CON 9
								_empresaDocumento.Ultimonumerodocumentomov += 1;
								_empresaDocumento.Numeroverificador += 1;
								context.Attach(_empresaDocumento);
								context.SaveChanges();

								//CON 10
								tableName = "CON 10 Movimiento";
								Movimiento movimiento = new Movimiento
								{
									Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
									Fecha = fechaCajero, // DateTime.Now.Date,
									Fechamaquina = DateTime.Now, //TODO esta fecha tiene que venir con horas minutos y segundos
									Codigousuario = model.CodigoUsuario,
									Secuencialoficinausuario = model.SecOficinaUsuario,
									Estaimpreso = false,
									Numeroverificador = 1
								};
								context.Movimiento.Add(movimiento);
								context.SaveChanges();

								result.SecuencialDocumento = movimiento.Secuencial.ToString();

								//12
								var _cuentaMaestro = context.Cuentamaestro.FirstOrDefault(a => a.Secuencial == model.SecuencialCuenta);
								if (_cuentaMaestro != null)
								{
									_cuentaMaestro.Numeroverificador += 1;

									context.Attach(_cuentaMaestro);
									context.SaveChanges();
								}

								//CON 13
								tableName = "CON13 Movimientodetalle";
								Movimientodetalle movimientodetalle = new Movimientodetalle
								{
									Secuencialmovimiento = movimiento.Secuencial,
									Secuencialtransaccion = model.SecuencialTransaccion,
									Secuencialmoneda = model.SecuencialMoneda,
									Valor = montoTransaccionTotal,
									Secuencialoficinaafectada = model.SecuencialOficinaCuenta
								};
								context.Movimientodetalle.Add(movimientodetalle);
								context.SaveChanges();

								TransaccionMoneda trs = model.Transacciones;
								//VALIDA SI EL DEPOSITO FUE EN EFECTIVO 
								if (trs.TipoMovimiento.FirstOrDefault(a => a.Codigotipomovimiento == "Efectivo").ValueInsert > 0)
								{

									//CON 14
									tableName = "CON14 CuentacomponenteVistaEfectivo";
									var cuentacomponenteVista = (from ccv in context.CuentacomponenteVista
																 join
						 cvd in context.ComponenteVistaDisponible on ccv.Secuencialcomponentevista equals cvd.SECUENCIALCOMPONENTEVISTA
																 where ccv.Secuencialcuenta == model.SecuencialCuenta
																 select ccv).FirstOrDefault();


									if (cuentacomponenteVista != null)
									{

										cuentacomponenteVista.Saldo += monttoTransaccionEfectivo;
										cuentacomponenteVista.Numeroverificador += 1;

										context.Attach(cuentacomponenteVista);
										context.SaveChanges();
									}

									saldoTotalCuentas += cuentacomponenteVista.Saldo; // cuentacomponenteVistaSaldoEfectivo.Saldo;

									//CON 15
									tableName = "CON15 movimientocuentacompVistaEfectivo";
									//obitene el secuencial componente
									int componenteSec = GetSecuenciaComponente(model.SecuencialCuenta);
									//decimal saldoTotalCuenta = context.MovimientocuentacompVista.LastOrDefault(a => a.Secuencialcuenta == model.SecuencialCuenta).Saldocuenta;

									decimal saldoTotalCuenta = context.MovimientocuentacompVista.Where(a => a.Secuencialcuenta == model.SecuencialCuenta).Take(1).OrderByDescending(a => a.Secuencial).First().Saldocuenta;

									errorLog.MainLog("ultimo saldo " + saldoTotalCuenta.ToString());

									saldoTotalCuenta += monttoTransaccionEfectivo;

									errorLog.MainLog("saldo mas monto transaccion " + saldoTotalCuenta.ToString());

									MovimientocuentacompVista movimientocuentacompVista = new MovimientocuentacompVista
									{
										Secuencialmovimientodetalle = movimientodetalle.Secuencial,
										Secuencialcuenta = model.SecuencialCuenta,
										Secuencialcomponentevista = componenteSec,
										Codigotipomovimiento = "Efectivo",
										Valor = monttoTransaccionEfectivo,
										Saldo = saldoTotalCuenta,
										Saldocuenta = saldoTotalCuenta
									};
									context.MovimientocuentacompVista.Add(movimientocuentacompVista);
									context.SaveChanges();

									//CON 18 TODO RELLENAR OTROS CAMPOS
									tableName = "CON18 registrocontable";
									var cuentaContable = context.ComponenteCuentaContable.FirstOrDefault(a => a.Secuencialcomponente == componenteSec);
									Registrocontable registrocontable = new Registrocontable
									{
										Valor = monttoTransaccionEfectivo,
										Esdebito = false,
										Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
										Detalle = model.NombreTransaccion + " " + model.CodigoCuenta.ToString(),
										Estacontabilizado = false,
										Secuencialcuentacontable = cuentaContable.SecuencialCuentaContable,
										Secuencialoficina = model.SecuencialOficinaCuenta,
										Secuencialperfilcontable = (model.SecOficinaUsuario != model.SecuencialOficinaCuenta) ? 5 : 1,
										Codigousuario = model.CodigoUsuario,
										Secuencialmoneda = model.SecuencialMoneda,
										Fechasistemaregistro = fechaCajero, // DateTime.Now.Date,
										Fechamaquinaregistro = DateTime.Now,
										Secuencialmovimientodetalle = movimientodetalle.Secuencial,
										Secuencialmovimientocontrol = 0,
										Estaactiva = true,
										Generarcheque = false,
										Esreverso = false,
										Numeroverificador = 1
									};
									context.Registrocontable.Add(registrocontable);
									context.SaveChanges();

									//CON19 
									tableName = "CON19 movimientoimpresion";
									Movimientoimpresion movimientoimpresion = new Movimientoimpresion
									{
										Fecha = DateTime.Now,
										Depositos = monttoTransaccionEfectivo.ToString(),
										Retiros = "0",
										Saldo = saldoTotalCuenta.ToString(), // cuentacomponenteVista.Saldo.ToString(),
										Transaccion = model.SiglasTransaccion,
										Secuencialcliente = model.secCliente,
										Secuencialcuenta = model.SecuencialCuenta,
										Operador = model.CodigoUsuario,
										Estaimpresa = false,
										Numeoverificador = 0,
										Efectivo = monttoTransaccionEfectivo.ToString(),
										Cheque = "0,00",
										Saldodisponible = saldoTotalCuenta.ToString(),  //cuentacomponenteVista.Saldo.ToString(),
										Saldoobligatorios = "0",
										Valortransaccion = monttoTransaccionEfectivo.ToString(),
										Eslinearendfinanc = false,
										Detallerendfinanc = "M"
									};
									context.Movimientoimpresion.Add(movimientoimpresion);
									context.SaveChanges();

									//CON 20 
									tableName = "CON20 MovimientodetalleCuenta";
									MovimientodetalleCuenta movimientodetalleCuenta = new MovimientodetalleCuenta
									{
										Secuencialmovimientodetalle = movimientodetalle.Secuencial,
										Secuencialcuenta = model.SecuencialCuenta,
										Saldocuenta = saldoTotalCuenta, // cuentacomponenteVista.Saldo,
										Codigoestadocuenta = "A"
									};
									context.MovimientodetalleCuenta.Add(movimientodetalleCuenta);
									context.SaveChanges();

									////CON 21
									tableName = "CON21 ventanilla - Efectivo";
									var getVentanillaEfectivo = context.Ventanilla.FirstOrDefault(a => a.Codigousuario == model.CodigoUsuario && a.Secuencialoficina == model.SecOficinaUsuario && a.Fecha == fechaCajero);
									int secuencialVentanillaEfectivo = 0;
									bool existeVentanillaefectivo;
									if (getVentanillaEfectivo == null)
									{
										existeVentanillaefectivo = false;
										Ventanilla ventanillaEfectivo = new Ventanilla
										{
											Codigousuario = model.CodigoUsuario,
											Secuencialoficina = (model.SecOficinaUsuario != model.SecuencialOficinaCuenta) ? model.SecOficinaUsuario : model.SecuencialOficinaCuenta, //model.SecuencialOficinaCuenta,
											Fecha = fechaCajero, // DateTime.Now,
											Abiertaautomaticamente = false,
											Estacerrada = false,
											Estacuadrada = false,
											Numerovecescuadrada = 0,
											Numeroverificador = 0
										};
										context.Ventanilla.Add(ventanillaEfectivo);
										context.SaveChanges();

										secuencialVentanillaEfectivo = ventanillaEfectivo.Secuencial;
									}
									else
									{
										existeVentanillaefectivo = true;
										secuencialVentanillaEfectivo = getVentanillaEfectivo.Secuencial;
										//getVentanillaCheque.Numeroverificador += 1;
										//context.Attach(getVentanillaCheque);
										//context.SaveChanges();
									}

									//CON 22 SE REPITE PORQUE GUARDA LA TRANSACCION EN CAJA QUE ES CODIGO 3
									tableName = "CON22 Movimientodetalle";
									Movimientodetalle movimientodetalle2 = new Movimientodetalle
									{
										Secuencialmovimiento = movimiento.Secuencial,
										Secuencialtransaccion = 3,
										Secuencialmoneda = model.SecuencialMoneda,
										Valor = monttoTransaccionEfectivo,
										Secuencialoficinaafectada = (model.SecOficinaUsuario != model.SecuencialOficinaCuenta) ? model.SecOficinaUsuario : model.SecuencialOficinaCuenta, //model.SecuencialOficinaCuenta
									};
									context.Movimientodetalle.Add(movimientodetalle2);
									context.SaveChanges();

									//CON 26  Secuencialcomponentecaja = 26 => Efectivo ...... 27 => ChequeIngreso
									tableName = "CON26 ventanillacomponente - Efectivo";
									int secuencialVentanillaComponenteCaja = 0;
									if (existeVentanillaefectivo)
									{
										var existeVentanilla = context.VentanillacomponenteCaja.FirstOrDefault(a => a.Secuencialventanilla == secuencialVentanillaEfectivo && a.Secuencialcomponentecaja == 26);

										if (existeVentanilla != null)
										{
											int valorPositivo = model.Transacciones.DenominacionMoneda.Where(a => a.ValueInsert > 0).Sum(a => a.ValueInsert);
											int valorNegativo = model.Transacciones.DenominacionMoneda.Where(a => a.ValueInsert < 0).Sum(a => a.ValueInsert);
											int totalPositivo = existeVentanilla.Cantidad + valorPositivo;

											existeVentanilla.Cantidad = totalPositivo + valorNegativo;
											existeVentanilla.Saldo += monttoTransaccionEfectivo;

											context.Attach(existeVentanilla);
											context.SaveChanges();

											secuencialVentanillaComponenteCaja = existeVentanilla.Secuencial;
										}
										else
										{
											VentanillacomponenteCaja ventanillacomponente = new VentanillacomponenteCaja
											{
												Secuencialventanilla = secuencialVentanillaEfectivo,
												Secuencialcomponentecaja = 26,
												Secuencialmoneda = model.SecuencialMoneda,
												Cantidad = model.Transacciones.DenominacionMoneda.Where(a => a.ValueInsert > 0).Sum(a => a.ValueInsert),
												Saldo = monttoTransaccionEfectivo,
												Valorcuadre = 0
											};
											context.VentanillacomponenteCaja.Add(ventanillacomponente);
											context.SaveChanges();

											secuencialVentanillaComponenteCaja = ventanillacomponente.Secuencial;

										}
									}
									else
									{
										VentanillacomponenteCaja ventanillacomponente = new VentanillacomponenteCaja
										{
											Secuencialventanilla = secuencialVentanillaEfectivo,
											Secuencialcomponentecaja = 26,
											Secuencialmoneda = model.SecuencialMoneda,
											Cantidad = model.Transacciones.DenominacionMoneda.Where(a => a.ValueInsert > 0).Sum(a => a.ValueInsert),
											Saldo = monttoTransaccionEfectivo,
											Valorcuadre = 0
										};
										context.VentanillacomponenteCaja.Add(ventanillacomponente);
										context.SaveChanges();

										secuencialVentanillaComponenteCaja = ventanillacomponente.Secuencial;
									}


									//27 VALIDAR ESTA CONSULTA TIENE QUE VER CON LOS BILLETES Y TRANSACCIONES DEL MONTO
									tableName = "CON27 MovimientoventanillacompCaja - efectivo";
									MovimientoventanillacompCaja movimientoventanillacompCaja = new MovimientoventanillacompCaja
									{
										Secuencialmovimientodetalle = movimientodetalle2.Secuencial,
										Secuencialventanillacompcaja = secuencialVentanillaComponenteCaja, // ventanillacomponente.Secuencial,
										Codigotipomovimientocaja = "Efectivo",
										Cantidad = 1,
										Valor = monttoTransaccionEfectivo,
										Saldo = monttoTransaccionEfectivo
									};
									context.MovimientoventanillacompCaja.Add(movimientoventanillacompCaja);
									context.SaveChanges();

									tableName = "CON28 Registrocontable";
									var codigoCuentaContable = context.ComponenteCuentaContable.FirstOrDefault(a => a.Secuencialcomponente == 26);
									Registrocontable registrocontable2 = new Registrocontable
									{
										Valor = monttoTransaccionEfectivo,
										Esdebito = true,
										Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
										Detalle = model.CodigoUsuario + " " + "Efectivo",
										Estacontabilizado = false,
										Secuencialcuentacontable = codigoCuentaContable.SecuencialCuentaContable,
										Secuencialoficina = (model.SecOficinaUsuario != model.SecuencialOficinaCuenta) ? model.SecOficinaUsuario : model.SecuencialOficinaCuenta,
										Secuencialperfilcontable = 1,
										Codigousuario = model.CodigoUsuario,
										Secuencialmoneda = model.SecuencialMoneda,
										Fechasistemaregistro = fechaCajero, // DateTime.Now.Date,
										Fechamaquinaregistro = DateTime.Now,
										Secuencialmovimientodetalle = movimientodetalle2.Secuencial,
										Secuencialmovimientocontrol = 0,
										Estaactiva = true,
										Generarcheque = false,
										Esreverso = false,
										Numeroverificador = 1
									};
									context.Registrocontable.Add(registrocontable2);
									context.SaveChanges();

									//Valida si son cuentas diferentes
									if (model.SecOficinaUsuario != model.SecuencialOficinaCuenta)
									{
										var rowItems = GetCuentaContableDiferenteOficina(model.SecOficinaUsuario, model.SecuencialOficinaCuenta);

										Movimientocontrol movControl1 = new Movimientocontrol
										{
											EsDebito = false,
											Valor = monttoTransaccionEfectivo,
											SecuencialOficina = model.SecOficinaUsuario,
											SecuencialMoneda = model.SecuencialMoneda,
											Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
											Fecha = fechaCajero,
											FechaMaquina = DateTime.Now,
											CodigoUsuario = model.CodigoUsuario,
											NumeroVerificador = 1
										};
										context.Movimientocontrol.Add(movControl1);
										context.SaveChanges();

										MovimientocontrolTransfInt movTransfInt1 = new MovimientocontrolTransfInt
										{
											SecuencialMovimientoControl = movControl1.Secuencial,
											SecuencialCuentaContable = rowItems.First()
										};
										context.MovimientocontrolTransfInt.Add(movTransfInt1);
										context.SaveChanges();

										Registrocontable regControl1 = new Registrocontable
										{
											Valor = monttoTransaccionEfectivo,
											Esdebito = false,
											Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
											Detalle = "",
											Estacontabilizado = false,
											Secuencialcuentacontable = rowItems.First(),
											Secuencialoficina = model.SecOficinaUsuario,
											Secuencialperfilcontable = 1,
											Codigousuario = model.CodigoUsuario,
											Secuencialmoneda = model.SecuencialMoneda,
											Fechasistemaregistro = fechaCajero,
											Fechamaquinaregistro = DateTime.Now,
											Secuencialmovimientodetalle = 0,
											Secuencialmovimientocontrol = movControl1.Secuencial,
											Estaactiva = true,
											Generarcheque = false,
											Esreverso = false,
											Numeroverificador = 1
										};
										context.Registrocontable.Add(regControl1);
										context.SaveChanges();

										Movimientocontrol movControl2 = new Movimientocontrol
										{
											EsDebito = true,
											Valor = monttoTransaccionEfectivo,
											SecuencialOficina = model.SecuencialOficinaCuenta,
											SecuencialMoneda = model.SecuencialMoneda,
											Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
											Fecha = fechaCajero,
											FechaMaquina = DateTime.Now,
											CodigoUsuario = model.CodigoUsuario,
											NumeroVerificador = 1

										};
										context.Movimientocontrol.Add(movControl2);
										context.SaveChanges();

										MovimientocontrolTransfInt movTransfInt2 = new MovimientocontrolTransfInt
										{
											SecuencialMovimientoControl = movControl2.Secuencial,
											SecuencialCuentaContable = rowItems.Last()
										};
										context.MovimientocontrolTransfInt.Add(movTransfInt2);
										context.SaveChanges();

										Registrocontable regControl2 = new Registrocontable
										{
											Valor = monttoTransaccionEfectivo,
											Esdebito = true,
											Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
											Detalle = "",
											Estacontabilizado = false,
											Secuencialcuentacontable = rowItems.Last(),
											Secuencialoficina = model.SecuencialOficinaCuenta,
											Secuencialperfilcontable = 5,
											Codigousuario = model.CodigoUsuario,
											Secuencialmoneda = model.SecuencialMoneda,
											Fechasistemaregistro = fechaCajero,
											Fechamaquinaregistro = DateTime.Now,
											Secuencialmovimientodetalle = 0,
											Secuencialmovimientocontrol = movControl1.Secuencial,
											Estaactiva = true,
											Generarcheque = false,
											Esreverso = false,
											Numeroverificador = 1
										};
										context.Registrocontable.Add(regControl2);
										context.SaveChanges();

									}


									//INGRESA DENOMINACIONES MONEDA
									foreach (var item in model.Transacciones.DenominacionMoneda.Where(a => a.ValueInsert != 0))
									{
										int secEfectivoDenominacion = 0;
										var ventanillaDenominacion = context.Ventanillacomponentedenomnefe.FirstOrDefault(a => a.Secuencialventanillacompcaja == secuencialVentanillaComponenteCaja && a.Denominacion == item.Denominacion);
										if (ventanillaDenominacion == null)
										{
											Ventanillacomponentedenomnefe _vent = new Ventanillacomponentedenomnefe
											{
												Secuencialventanillacompcaja = secuencialVentanillaComponenteCaja, // ventanillacomponente.Secuencial,
												Denominacion = item.Denominacion,
												Cantidad = item.ValueInsert
											};
											context.Ventanillacomponentedenomnefe.Add(_vent);
											context.SaveChanges();
											secEfectivoDenominacion = _vent.Secuencial;
										}
										else
										{
											ventanillaDenominacion.Cantidad += item.ValueInsert;
											context.Attach(ventanillaDenominacion);
											context.SaveChanges();
											secEfectivoDenominacion = ventanillaDenominacion.Secuencial;
										}


										MovimientoventcompCajadet movimientoventcomp = new MovimientoventcompCajadet
										{
											Secuencialmovventcompcaja = movimientoventanillacompCaja.Secuencial,
											Secuencialventcompdenomefe = secEfectivoDenominacion,
											Cantidad = item.ValueInsert,
											Saldo = item.ValueInsert * item.Denominacion,
										};
										context.MovimientoventcompCajadet.Add(movimientoventcomp);
										context.SaveChanges();

									}

									//29 (DENOMINACION DE BILLETES) //AQUI DEBE IR EL BUCLE DE LAS MONEDAS
									//Ventanillacomponentedenomnefe ventanillacomponentedenomnefe = new Ventanillacomponentedenomnefe
									//{
									//	Secuencialventanillacompcaja = ventanillacomponente.Secuencial,
									//	Denominacion = 10,
									//	Cantidad = 1, // moneda selecciona 
									//};
								}
								//else
								//{
								//	var cuentacomponenteVista = (from ccv in context.CuentacomponenteVista join
								//                               cvd in context.ComponenteVistaDisponible on ccv.Secuencialcomponentevista equals cvd.SECUENCIALCOMPONENTEVISTA
								//								 where ccv.Secuencialcuenta == model.SecuencialCuenta
								//								 select ccv).FirstOrDefault();

								//	//var cuentacomponenteVista = context.CuentacomponenteVista.Where(a => a.Secuencialcuenta == model.SecuencialCuenta).ToList();
								//	//var cuentacomponenteVistaSaldoEfectivo = cuentacomponenteVista.FirstOrDefault(a => a.Secuencialcomponentevista == 1);
								//	saldoTotalCuentas += cuentacomponenteVista.Saldo; // cuentacomponenteVistaSaldoEfectivo.Saldo;
								//}

								//VALIDA SI EL DEPOSITO FUE EN CHEQUE
								if (trs.TipoMovimiento.FirstOrDefault(a => a.Codigotipomovimiento == "Cheque").ValueInsert > 0)
								{
									//CON 14
									tableName = "CON14 CuentacomponenteVistaCheque";
									//var cuentacomponenteVista = context.CuentacomponenteVista.Where(a => a.Secuencialcuenta == model.SecuencialCuenta).ToList();
									//var cuentacomponenteVistaSaldoCheque = cuentacomponenteVista.FirstOrDefault(a => a.Secuencialcomponentevista == 2);

									var cuentacomponenteVista = (from ccv in context.CuentacomponenteVista
																 join
					   cvd in context.ComponenteVistaRetencion on ccv.Secuencialcomponentevista equals cvd.SECUENCIALCOMPONENTEVISTA
																 where ccv.Secuencialcuenta == model.SecuencialCuenta
																 select ccv).FirstOrDefault();

									if (cuentacomponenteVista != null)
									{

										cuentacomponenteVista.Saldo += monttoTransaccionCheque;
										cuentacomponenteVista.Numeroverificador += 1;

										context.Attach(cuentacomponenteVista);
										context.SaveChanges();
									}

									//saldoTotalCuentas += cuentacomponenteVista.Saldo;

									//CON 15
									tableName = "CON15 movimientocuentacompVistaCheque";
									//var saldoTotalCuenta = context.CuentacomponenteVista.LastOrDefault(a => a.Secuencialcuenta == model.SecuencialCuenta).Saldo + monttoTransaccionCheque;
									//var saldoTotalCuenta = cuentacomponenteVista.Saldo; // + monttoTransaccionCheque;  //cuentacomponenteVista.Sum(a => a.Saldo) + monttoTransaccionCheque;

									var saldoTotalCuenta = context.MovimientocuentacompVista.LastOrDefault(a => a.Secuencialcuenta == model.SecuencialCuenta).Saldo + monttoTransaccionCheque;

									MovimientocuentacompVista movimientocuentacompVista = new MovimientocuentacompVista
									{
										Secuencialmovimientodetalle = movimientodetalle.Secuencial,
										Secuencialcuenta = model.SecuencialCuenta,
										Secuencialcomponentevista = 2,
										Codigotipomovimiento = "Cheque",
										Valor = monttoTransaccionCheque,
										Saldo = saldoTotalCuenta, // cuentacomponenteVista.Saldo, //montoTransaccion,
										Saldocuenta = saldoTotalCuenta // cuentacomponenteVista.Saldo
									};
									context.MovimientocuentacompVista.Add(movimientocuentacompVista);
									context.SaveChanges();

									//CON 18 TODO RELLENAR OTROS CAMPOS
									tableName = "CON18 registrocontableCheque";
									//Validar el secuencial componente si siempre es 1 efectivo , 2 Cheque
									var cuentaContable = context.ComponenteCuentaContable.FirstOrDefault(a => a.Secuencialcomponente == 2);
									Registrocontable registrocontable = new Registrocontable
									{
										Valor = monttoTransaccionCheque,
										Esdebito = false,
										Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
										Detalle = model.CodigoUsuario + " " + "ChequeIngreso", // model.NombreTransaccion + " " + model.CodigoCuenta.ToString(),
										Estacontabilizado = false,
										Secuencialcuentacontable = cuentaContable.SecuencialCuentaContable,
										Secuencialoficina = model.SecuencialOficinaCuenta,
										Secuencialperfilcontable = 1,
										Codigousuario = model.CodigoUsuario,
										Secuencialmoneda = model.SecuencialMoneda,
										Fechasistemaregistro = fechaCajero, // DateTime.Now.Date,
										Fechamaquinaregistro = DateTime.Now,
										Secuencialmovimientodetalle = movimientodetalle.Secuencial,
										Secuencialmovimientocontrol = 0,
										Estaactiva = true,
										Generarcheque = false,
										Esreverso = false,
										Numeroverificador = 1
									};
									context.Registrocontable.Add(registrocontable);
									context.SaveChanges();

									//CON19 PREGUNTAR SI ESTA TABLA LA VAMOS A UTILIZAR 
									tableName = "CON19 movimientoimpresionCheque";
									Movimientoimpresion movimientoimpresion = new Movimientoimpresion
									{
										Fecha = DateTime.Now,
										Depositos = monttoTransaccionCheque.ToString(),
										Retiros = "0",
										Saldo = saldoTotalCuenta.ToString(), // cuentacomponenteVista.Saldo.ToString(),
										Transaccion = model.SiglasTransaccion,
										Secuencialcliente = model.secCliente,
										Secuencialcuenta = model.SecuencialCuenta,
										Operador = model.CodigoUsuario,
										Estaimpresa = false,
										Numeoverificador = 0,
										Efectivo = "0,00",
										Cheque = monttoTransaccionCheque.ToString(),
										Saldodisponible = saldoTotalCuenta.ToString(),  //cuentacomponenteVista.Saldo.ToString(),
										Saldoobligatorios = "0",
										Valortransaccion = monttoTransaccionCheque.ToString(),
										Eslinearendfinanc = false,
										Detallerendfinanc = ""
									};
									context.Movimientoimpresion.Add(movimientoimpresion);
									context.SaveChanges();

									//CON 20 
									tableName = "CON20 MovimientodetalleCuentaCheque";
									var existMovimientoDetalleCuenta = context.MovimientodetalleCuenta.FirstOrDefault(a => a.Secuencialmovimientodetalle == movimientodetalle.Secuencial);

									if (existMovimientoDetalleCuenta == null)
									{
										MovimientodetalleCuenta movimientodetalleCuentaCheque = new MovimientodetalleCuenta
										{
											Secuencialmovimientodetalle = movimientodetalle.Secuencial,
											Secuencialcuenta = model.SecuencialCuenta,
											Saldocuenta = saldoTotalCuenta, //saldoTotalCuenta, // cuentacomponenteVista.Saldo,
											Codigoestadocuenta = "A"
										};
										context.MovimientodetalleCuenta.Add(movimientodetalleCuentaCheque);
										context.SaveChanges();
									}
									else
									{
										existMovimientoDetalleCuenta.Saldocuenta = saldoTotalCuenta; // saldoTotalCuenta;
										context.Attach(existMovimientoDetalleCuenta);
										context.SaveChanges();
									}


									////CON 21
									tableName = "CON21 ventanilla - Cheque";
									var getVentanillaCheque = context.Ventanilla.FirstOrDefault(a => a.Codigousuario == model.CodigoUsuario && a.Secuencialoficina == model.SecuencialOficinaCuenta && a.Fecha == fechaCajero);
									int secuencialVentanillaCheque = 0;
									bool existeVentanillaeCheque;
									if (getVentanillaCheque == null)
									{
										existeVentanillaeCheque = false;
										Ventanilla ventanillaCheque = new Ventanilla
										{
											Codigousuario = model.CodigoUsuario,
											Secuencialoficina = model.SecuencialOficinaCuenta,
											Fecha = fechaCajero, // DateTime.Now,
											Abiertaautomaticamente = false,
											Estacerrada = false,
											Estacuadrada = false,
											Numerovecescuadrada = 0,
											Numeroverificador = 0
										};
										context.Ventanilla.Add(ventanillaCheque);
										context.SaveChanges();
										secuencialVentanillaCheque = ventanillaCheque.Secuencial;
									}
									else
									{
										existeVentanillaeCheque = true;
										secuencialVentanillaCheque = getVentanillaCheque.Secuencial;
									}


									//CON 22 SE REPITE PORQUE GUARDA LA TRANSACCION EN CAJA QUE ES CODIGO 3
									tableName = "CON22 MovimientodetalleCheque";
									Movimientodetalle movimientodetalleCheque2 = new Movimientodetalle
									{
										Secuencialmovimiento = movimiento.Secuencial,
										Secuencialtransaccion = 3,
										Secuencialmoneda = model.SecuencialMoneda,
										Valor = monttoTransaccionCheque,
										Secuencialoficinaafectada = model.SecuencialOficinaCuenta
									};
									context.Movimientodetalle.Add(movimientodetalleCheque2);
									context.SaveChanges();

									//CON 26  Secuencialcomponentecaja = 26 => Efectivo ...... 27 => ChequeIngreso
									tableName = "CON26 ventanillacomponenteCheque";
									int secuencialVentanillaComponenteCajaCheque = 0;
									if (existeVentanillaeCheque)
									{
										var existeVentanillaCheque = context.VentanillacomponenteCaja.FirstOrDefault(a => a.Secuencialventanilla == secuencialVentanillaCheque && a.Secuencialcomponentecaja == 27);

										if (existeVentanillaCheque != null)
										{
											existeVentanillaCheque.Cantidad += model.Cheques.Count;
											existeVentanillaCheque.Saldo += monttoTransaccionCheque;

											context.Attach(existeVentanillaCheque);
											context.SaveChanges();

											secuencialVentanillaComponenteCajaCheque = existeVentanillaCheque.Secuencial;
										}
										else
										{
											VentanillacomponenteCaja ventanillacomponente = new VentanillacomponenteCaja
											{
												Secuencialventanilla = secuencialVentanillaCheque, // ventanillaCheque.Secuencial,
												Secuencialcomponentecaja = 27,
												Secuencialmoneda = model.SecuencialMoneda,
												Cantidad = model.Cheques.Count, // 1,
												Saldo = monttoTransaccionCheque,
												Valorcuadre = 0
											};
											context.VentanillacomponenteCaja.Add(ventanillacomponente);
											context.SaveChanges();
											secuencialVentanillaComponenteCajaCheque = ventanillacomponente.Secuencial;
										}

									}
									else
									{
										VentanillacomponenteCaja ventanillacomponente = new VentanillacomponenteCaja
										{
											Secuencialventanilla = secuencialVentanillaCheque, // ventanillaCheque.Secuencial,
											Secuencialcomponentecaja = 27,
											Secuencialmoneda = model.SecuencialMoneda,
											Cantidad = model.Cheques.Count, // 1,
											Saldo = monttoTransaccionCheque,
											Valorcuadre = 0
										};
										context.VentanillacomponenteCaja.Add(ventanillacomponente);
										context.SaveChanges();
										secuencialVentanillaComponenteCajaCheque = ventanillacomponente.Secuencial;
									}


									tableName = "CON27 MovimientoventanillacompCajaCheque";
									MovimientoventanillacompCaja movimientoventanillacompCaja = new MovimientoventanillacompCaja
									{
										Secuencialmovimientodetalle = movimientodetalleCheque2.Secuencial,
										Secuencialventanillacompcaja = secuencialVentanillaComponenteCajaCheque, // ventanillacomponente.Secuencial,
										Codigotipomovimientocaja = "ChequeIngreso",
										Cantidad = model.Cheques.Count(), //1,
										Valor = monttoTransaccionCheque,
										Saldo = monttoTransaccionCheque
									};
									context.MovimientoventanillacompCaja.Add(movimientoventanillacompCaja);
									context.SaveChanges();

									//28   
									tableName = "CON28 RegistrocontableCheque";
									var codigoCuentaContable = context.ComponenteCuentaContable.FirstOrDefault(a => a.Secuencialcomponente == 2);
									Registrocontable registrocontable2 = new Registrocontable
									{
										Valor = monttoTransaccionCheque,
										Esdebito = true,
										Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
										Detalle = model.CodigoUsuario + " " + "ChequeIngreso",
										Estacontabilizado = false,
										Secuencialcuentacontable = codigoCuentaContable.SecuencialCuentaContable,
										Secuencialoficina = model.SecuencialOficinaCuenta,
										Secuencialperfilcontable = 1,
										Codigousuario = model.CodigoUsuario,
										Secuencialmoneda = model.SecuencialMoneda,
										Fechasistemaregistro = fechaCajero, // DateTime.Now.Date,
										Fechamaquinaregistro = DateTime.Now,
										Secuencialmovimientodetalle = movimientodetalleCheque2.Secuencial,
										Secuencialmovimientocontrol = 0,
										Estaactiva = true,
										Generarcheque = false,
										Esreverso = false,
										Numeroverificador = 1
									};
									context.Registrocontable.Add(registrocontable2);
									context.SaveChanges();

									foreach (var item in model.Cheques)
									{
										tableName = "CON29 Cheque";
										//int diasEfectivizacion = context.Ruta.FirstOrDefault(a => a.Secuencialbancoemisor == item.SecuencialBancoEmisor).Diastransito;
										var cheque = new Cheque
										{
											CodigoCuentaCorriente = item.CodigoCuentaCorriente,
											CodigoCheque = item.CodigoCheque,
											SecuencialBancoEmisor = item.SecuencialBancoEmisor,
											SecuencialMoneda = model.SecuencialMoneda,
											Valor = item.Valor,
											CodigoUsuario = model.CodigoUsuario,
											Estaenboveda = false,
											FechaSistemaIngreso = fechaCajero, // DateTime.Now.Date,
											FechaMaquinaIngreso = DateTime.Now,
											CodigoEstadoCheque = "Ingresado",
											NumeroVerificador = 1,
										};
										context.Cheque.Add(cheque);
										context.SaveChanges();

										tableName = "CON30 ChequeMovimientoDetalle";
										ChequeMovimientoDetalle chequeMovimientoDetalle = new ChequeMovimientoDetalle
										{
											SecuencialCheque = cheque.Secuencial,
											SecuencialMovimientoDetalle = movimientodetalle.Secuencial
										};

										context.ChequeMovimientoDetalle.Add(chequeMovimientoDetalle);
										context.SaveChanges();

										ChequeMovimientoDetalle chequeMovimientoDetalle2 = new ChequeMovimientoDetalle
										{
											SecuencialCheque = cheque.Secuencial,
											SecuencialMovimientoDetalle = movimientodetalleCheque2.Secuencial
										};

										context.ChequeMovimientoDetalle.Add(chequeMovimientoDetalle2);
										context.SaveChanges();

										//Chequeefectivizacion chequeEfectivizacion = new Chequeefectivizacion
										//{
										//	SecuencialCheque = cheque.Secuencial,
										//	FechaMaquina = DateTime.Now,
										//	FechaSistema = DateTime.Now, // fechaCajero.AddDays(diasEfectivizacion), //TODO AGREGA CAMPOS DIAS DE EFECTIVACION DE CHEQUE SEGUN BANCO SELECCIONADO
										//	CodigoUsuario = model.CodigoUsuario,
										//	SecuencialOficina = model.SecuencialOficinaCuenta,
										//	Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
										//	EsManual = false,
										//	EstuvoenTransito = false
										//};
										//context.Chequeefectivizacion.Add(chequeEfectivizacion);
										//context.SaveChanges();
									}
								}
								else
								{
									var cuentacomponenteVista = (from ccv in context.CuentacomponenteVista
																 join
					   cvd in context.ComponenteVistaRetencion on ccv.Secuencialcomponentevista equals cvd.SECUENCIALCOMPONENTEVISTA
																 where ccv.Secuencialcuenta == model.SecuencialCuenta
																 select ccv).FirstOrDefault();

									//var cuentacomponenteVista = context.CuentacomponenteVista.Where(a => a.Secuencialcuenta == model.SecuencialCuenta).ToList();
									//var cuentacomponenteVistaSaldoCheque = cuentacomponenteVista.FirstOrDefault(a => a.Secuencialcomponentevista == 2);

									saldoTotalCuentas += (cuentacomponenteVista == null) ? 0 : cuentacomponenteVista.Saldo; // cuentacomponenteVistaSaldoCheque.Saldo;
								}

								tableName = "Transaccionmobile";
								Transaccionmobile tmobile = new Transaccionmobile
								{
									CodigoUsuario = model.CodigoUsuario,
									NumeroCliente = model.numCliente,
									Fecha = DateTime.Now,
									Monto = montoTransaccionTotal,
									Longitud = 0,
									Latitud = 0
								};
								context.Transaccionmobile.Add(tmobile);
								context.SaveChanges();

							}

							result.Saldodeposito = saldoTotalCuentas;
							result.Result = true;
							transaction.Commit();
						}
					}
					else
					{
						result.MessageResult = "El Usuario NO tiene permisos para hacer esta OPERACION";
						result.Result = false;
					}
				}
				catch (Exception ex)
				{
					result.MessageResult = "Se Presento un Error al momento de Guardar la Transaccion, Consulte Administradror";
					errorLog.MainLog("SaveTransaccion " + tableName + " Error: " +  ex.Message.ToString());
					result.Result = false;
					transaction.Rollback();
				}

			}
			return result;
		}

		private string ExixteDenominacionEfectivo(RegistrarTransaccion model, decimal montoEfectivo, DateTime fechaCajero)
		{
			string messageResult = string.Empty;
			int resultCantidadDisponible = 0;
			try
			{

				var getVentanillaEfectivo = context.Ventanilla.FirstOrDefault(a => a.Codigousuario == model.CodigoUsuario && a.Secuencialoficina == model.SecuencialOficinaCuenta && a.Fecha == fechaCajero);
				if (getVentanillaEfectivo != null)
				{
					int secuencialVentanillaEfectivo = getVentanillaEfectivo.Secuencial;
					var existeVentanilla = context.VentanillacomponenteCaja.FirstOrDefault(a => a.Secuencialventanilla == secuencialVentanillaEfectivo && a.Secuencialcomponentecaja == 26);

					int secuencialVentanillaComponenteCaja = existeVentanilla.Secuencial;

					foreach (var item in model.Transacciones.DenominacionMoneda.Where(a => a.ValueInsert < 0))
					{
						var ventanillaDenominacion = context.Ventanillacomponentedenomnefe.FirstOrDefault(a => a.Secuencialventanillacompcaja == secuencialVentanillaComponenteCaja && a.Denominacion == item.Denominacion);
						if (ventanillaDenominacion == null)
						{
							messageResult = "Denominacion NO creada para Moneda : " + item.Denominacion;
							return messageResult;
						}
						else
						{
							int CantidadDisponibleDenominacion = ventanillaDenominacion.Cantidad;
							resultCantidadDisponible = CantidadDisponibleDenominacion + item.ValueInsert;
							if (resultCantidadDisponible < 0)
							{
								messageResult = "No existe CantidadDisponible para la Denominación : " + item.Denominacion;
								return messageResult;
							}
						}
					}
				}
				else
				{
					messageResult = "Ventanilla no creada para el usuario / oficina / fecha";
					return messageResult;
				}
			}
			catch (Exception ex)
			{
				messageResult = ex.Message.ToString();
			}
			return messageResult;

		}


		private int GetSecuenciaComponente(int secuencialCuenta)
		{
			int componente = 0;
			try
			{
				string qry = "";
				qry += "SELECT CUENTACOMPONENTE_VISTA.SECUENCIAL, ";
				qry += "CUENTACOMPONENTE_VISTA.SECUENCIALCUENTA, ";
				qry += "CUENTACOMPONENTE_VISTA.SECUENCIALCOMPONENTEVISTA, ";
				qry += "CUENTACOMPONENTE_VISTA.SALDO, ";
				qry += "CUENTACOMPONENTE_VISTA.NUMEROVERIFICADOR ";
				qry += "FROM  FBS_CAPTACIONESVISTA.CUENTACOMPONENTE_VISTA , FBS_CAPTACIONESVISTA.COMPONENTE_VISTA ";
				qry += "WHERE CUENTACOMPONENTE_VISTA.SECUENCIALCOMPONENTEVISTA = COMPONENTE_VISTA.SECUENCIALCOMPONENTE AND ";
				qry += "CUENTACOMPONENTE_VISTA.SECUENCIALCUENTA = '" + secuencialCuenta + "' AND ";
				qry += "COMPONENTE_VISTA.SECUENCIALCOMPONENTE IN ";
				qry += "(SELECT COMPONENTE_VISTA_DISPONIBLE.SECUENCIALCOMPONENTEVISTA FROM FBS_CAPTACIONESVISTA.COMPONENTE_VISTA_DISPONIBLE )";


				using (SqlConnection conn = new SqlConnection(SQLHelper.ConnectionString))
				{
					SqlDataReader dr = SQLHelper.ExecuteReader(conn, System.Data.CommandType.Text, qry, null);
					if (dr.HasRows)
					{
						while (dr.Read())
						{
							componente = dr.GetInt32(2);
						}
					}
				}
			}
			catch (Exception)
			{

				throw;
			}
			return componente;
		}

		private List<int> GetCuentaContableDiferenteOficina(int oficinaUsuario, int oficinaCuenta)
		{
			List<int> result = new List<int>();
			try
			{
				string qry = "";
				qry += "SELECT CONDICIONTRANSFERENCIAINTERNA.SECUENCIAL, ";
				qry += "CONDICIONTRANSFERENCIAINTERNA.SECUENCIALOFICINAORIGEN, ";
				qry += "CONDICIONTRANSFERENCIAINTERNA.SECUENCIALOFICINADESTINO, ";
				qry += "CONDICIONTRANSFERENCIAINTERNA.ESDEBITO, ";
				qry += "CONDICIONTRANSFERENCIAINTERNA.SECUENCIALCUENTACONTABLE, ";
				qry += "CONDICIONTRANSFERENCIAINTERNA.SECUENCIALOFICINAAFECTA, ";
				qry += "CONDICIONTRANSFERENCIAINTERNA.ESDEUDOR, ";
				qry += "CONDICIONTRANSFERENCIAINTERNA.ESTAACTIVA, ";
				qry += "CONDICIONTRANSFERENCIAINTERNA.NUMEROVERIFICADOR ";
				qry += "FROM FBS_CONTABILIDADES.CONDICIONTRANSFERENCIAINTERNA ";
				qry += "WHERE CONDICIONTRANSFERENCIAINTERNA.SECUENCIALOFICINAORIGEN = " + oficinaUsuario + " ";
				qry += "AND CONDICIONTRANSFERENCIAINTERNA.SECUENCIALOFICINADESTINO = " + oficinaCuenta + " ";
				qry += "AND CONDICIONTRANSFERENCIAINTERNa.ESDEBITO = 0 ";
				qry += "AND CONDICIONTRANSFERENCIAINTERNA.ESTAACTIVA = 1";

				using (SqlConnection conn = new SqlConnection(SQLHelper.ConnectionString))
				{
					SqlDataReader dr = SQLHelper.ExecuteReader(conn, System.Data.CommandType.Text, qry, null);
					if (dr.HasRows)
					{
						foreach (DbDataRecord c in dr.Cast<DbDataRecord>())
						{
							result.Add(dr.GetInt32(4));
						}
					}
				}

			}
			catch (Exception ex)
			{
				result = null;
			}
			return result;
		}


		public List<Banco> GetBancos()
		{
			List<Banco> list = new List<Banco>();
			try
			{
				list = context.Banco.Where(a => a.Estaactivo == true).ToList();
			}
			catch ( Exception ex)
			{
				errorLog.MainLog("GetBancos, ERROR " + ex.Message.ToString());
			}
			return list;
		}

		public List<TransaccionmobileExtend> GetTransaccionMobile(string codigoUsuario, string fecha)
		{
			List<TransaccionmobileExtend> list = new List<TransaccionmobileExtend>();
			try
			{
				list = (from tm in context.Transaccionmobile join
                        cl in context.Cliente on tm.NumeroCliente equals cl.Numerocliente join
                        p in context.Persona on cl.Secuencialpersona equals p.Secuencial
						where tm.CodigoUsuario == codigoUsuario && tm.Fecha.ToString("dd-MM-yyyy") ==  fecha
						select new TransaccionmobileExtend
						{
							Secuencial = tm.Secuencial,
							CodigoUsuario = tm.CodigoUsuario,
							NumeroCliente = tm.NumeroCliente,
							NombreCliente = p.Nombreunido,
							Fecha = tm.Fecha,
							Latitud = tm.Latitud,
							Longitud = tm.Longitud,
							Monto = tm.Monto
						}).ToList();
			}
			catch (Exception ex)
			{
				list = null;
				errorLog.MainLog("SaveTransaccionMobile, Error " + ex.Message.ToString());
			}
			return list;
		}

		public bool AccesUsarioRol(string usuario, string rol)
		{
			return context.Usuariorol.Any(a => a.CodigoUsuario == usuario && a.CodigoRol == rol && a.EstaActivo == true);
		}
	}
}
