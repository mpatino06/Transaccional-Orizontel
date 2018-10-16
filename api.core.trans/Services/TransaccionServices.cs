using System;
using System.Collections.Generic;
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
					decimal saldoTotalCuentas = 0;
					decimal montoTransaccionTotal = model.Transacciones.TipoMovimiento.Sum(a=> a.ValueInsert);
					decimal monttoTransaccionEfectivo = model.Transacciones.TipoMovimiento.FirstOrDefault(a => a.Codigotipomovimiento =="Efectivo").ValueInsert;
					decimal monttoTransaccionCheque = model.Transacciones.TipoMovimiento.FirstOrDefault(a => a.Codigotipomovimiento == "Cheque").ValueInsert;
					//DateTime fechaCajero = context.Empresafechacajero.FirstOrDefault(a => a.Secuencialempresa == model.SecEmpresa).Fecha;
					DateTime fechaCajero = context.Calendario.Where(a => a.EstaCerrado ==  false && a.EsFeriado == false).Min(a=> a.FechaSistema);

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
							Secuencialoficinausuario = model.SecOficina,
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
							Secuencialoficinaafectada = model.SecOficina
						};
						context.Movimientodetalle.Add(movimientodetalle);
						context.SaveChanges();

						TransaccionMoneda trs = model.Transacciones;
						//VALIDA SI EL DEPOSITO FUE EN EFECTIVO 
						if (trs.TipoMovimiento.FirstOrDefault(a => a.Codigotipomovimiento == "Efectivo").ValueInsert > 0)
						{

							//CON 14
							tableName = "CON14 CuentacomponenteVistaEfectivo";
							//var cuentacomponenteVista = context.CuentacomponenteVista.FirstOrDefault(a => a.Secuencialcuenta == secCuenta && a.Secuencialcomponentevista == 1);
							var cuentacomponenteVista = context.CuentacomponenteVista.Where(a => a.Secuencialcuenta == model.SecuencialCuenta).ToList();
							var cuentacomponenteVistaSaldoEfectivo = cuentacomponenteVista.FirstOrDefault(a => a.Secuencialcomponentevista == 1);
							if (cuentacomponenteVistaSaldoEfectivo != null)
							{

								cuentacomponenteVistaSaldoEfectivo.Saldo += monttoTransaccionEfectivo;
								cuentacomponenteVistaSaldoEfectivo.Numeroverificador += 1;

								context.Attach(cuentacomponenteVistaSaldoEfectivo);
								context.SaveChanges();
							}

							saldoTotalCuentas += cuentacomponenteVistaSaldoEfectivo.Saldo;

							//CON 15
							tableName = "CON15 movimientocuentacompVistaEfectivo";
							var saldoTotalCuenta = cuentacomponenteVista.Sum(a => a.Saldo) + monttoTransaccionEfectivo;
							MovimientocuentacompVista movimientocuentacompVista = new MovimientocuentacompVista
							{
								Secuencialmovimientodetalle = movimientodetalle.Secuencial,
								Secuencialcuenta = model.SecuencialCuenta,
								Secuencialcomponentevista = 1,
								Codigotipomovimiento = "Efectivo",
								Valor = monttoTransaccionEfectivo,
								Saldo = cuentacomponenteVistaSaldoEfectivo.Saldo, //montoTransaccion,
								Saldocuenta = saldoTotalCuenta // cuentacomponenteVista.Saldo
							};
							context.MovimientocuentacompVista.Add(movimientocuentacompVista);
							context.SaveChanges();

							//CON 18 TODO RELLENAR OTROS CAMPOS
							tableName = "CON18 registrocontable";
							//Validar el secuencial componente si siempre es 1 efectivo , 2 Cheque
							var cuentaContable = context.ComponenteCuentaContable.FirstOrDefault(a => a.Secuencialcomponente == 1);
							Registrocontable registrocontable = new Registrocontable
							{
								Valor = monttoTransaccionEfectivo,
								Esdebito = false,
								Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
								Detalle = model.NombreTransaccion + " " + model.CodigoCuenta.ToString(),
								Estacontabilizado = false,
								Secuencialcuentacontable = cuentaContable.SecuencialCuentaContable,
								Secuencialoficina = model.SecOficina,
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
							tableName = "CON21 ventanilla - Efecivo";
							var getVentanillaEfectivo = context.Ventanilla.FirstOrDefault(a => a.Codigousuario == model.CodigoUsuario && a.Secuencialoficina == model.SecOficina && a.Fecha == fechaCajero);
							int secuencialVentanillaEfectivo = 0;
							bool existeVentanillaefectivo;
							if (getVentanillaEfectivo == null)
							{
								existeVentanillaefectivo = false;
								Ventanilla ventanillaEfectivo = new Ventanilla
								{
									Codigousuario = model.CodigoUsuario,
									Secuencialoficina = model.SecOficina,
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
								Secuencialoficinaafectada = model.SecOficina
							};
							context.Movimientodetalle.Add(movimientodetalle2);
							context.SaveChanges();

							//CON 26  Secuencialcomponentecaja = 26 => Efectivo ...... 27 => ChequeIngreso
							tableName = "CON26 ventanillacomponente - Efectivo";
							int secuencialVentanillaComponenteCaja = 0;
							if (existeVentanillaefectivo)
							{
								var existeVentanilla = context.VentanillacomponenteCaja.FirstOrDefault(a => a.Secuencialventanilla == secuencialVentanillaEfectivo);

								if(existeVentanilla != null)
								{
									existeVentanilla.Cantidad += model.Transacciones.DenominacionMoneda.Where(a => a.ValueInsert > 0).Sum(a => a.ValueInsert);
									existeVentanilla.Saldo += monttoTransaccionEfectivo;

									context.Attach(existeVentanilla);
									context.SaveChanges();

									secuencialVentanillaComponenteCaja = existeVentanilla.Secuencial;
								}
							}
							else
							{
								VentanillacomponenteCaja ventanillacomponente = new VentanillacomponenteCaja
								{
									Secuencialventanilla = secuencialVentanillaEfectivo,
									Secuencialcomponentecaja = 26,
									Secuencialmoneda = model.SecuencialMoneda,
									Cantidad = model.Transacciones.DenominacionMoneda.Where(a => a.ValueInsert > 0).Sum(a => a.ValueInsert), //   
									Saldo = monttoTransaccionEfectivo,
									Valorcuadre = 0
								};
								context.VentanillacomponenteCaja.Add(ventanillacomponente);
								context.SaveChanges();

								secuencialVentanillaComponenteCaja = ventanillacomponente.Secuencial;
							}


							//27 VALIDAR ESTA CONSULTA TIENE QUE VER CON LOS BILLETES Y TRANSACCIONES DEL MONTO
							tableName = "CON27 MovimientoventanillacompCaja";
							MovimientoventanillacompCaja movimientoventanillacompCaja = new MovimientoventanillacompCaja
							{
								Secuencialmovimientodetalle = movimientodetalle2.Secuencial, //Revisar se hace insercion en dos movimientos detalle revisar cual
								Secuencialventanillacompcaja = secuencialVentanillaComponenteCaja, // ventanillacomponente.Secuencial,
								Codigotipomovimientocaja = "Efectivo",
								Cantidad = 1,
								Valor = monttoTransaccionEfectivo,
								Saldo = monttoTransaccionEfectivo
							};
							context.MovimientoventanillacompCaja.Add(movimientoventanillacompCaja);
							context.SaveChanges();

							//28
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
								Secuencialoficina = model.SecOficina,
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



							foreach (var item in model.Transacciones.DenominacionMoneda.Where(a=> a.ValueInsert > 0))
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
						else
						{
							var cuentacomponenteVista = context.CuentacomponenteVista.Where(a => a.Secuencialcuenta == model.SecuencialCuenta).ToList();
							var cuentacomponenteVistaSaldoEfectivo = cuentacomponenteVista.FirstOrDefault(a => a.Secuencialcomponentevista == 1);
							saldoTotalCuentas += cuentacomponenteVistaSaldoEfectivo.Saldo;
						}

						//VALIDA SI EL DEPOSITO FUE EN CHEQUE
						if (trs.TipoMovimiento.FirstOrDefault(a => a.Codigotipomovimiento == "Cheque").ValueInsert > 0)
						{
							//CON 14
							tableName = "CON14 CuentacomponenteVistaCheque";
							//var cuentacomponenteVista = context.CuentacomponenteVista.FirstOrDefault(a => a.Secuencialcuenta == secCuenta && a.Secuencialcomponentevista == 1);
							var cuentacomponenteVista = context.CuentacomponenteVista.Where(a => a.Secuencialcuenta == model.SecuencialCuenta).ToList();
							var cuentacomponenteVistaSaldoCheque = cuentacomponenteVista.FirstOrDefault(a => a.Secuencialcomponentevista == 2);
							if (cuentacomponenteVistaSaldoCheque != null)
							{

								cuentacomponenteVistaSaldoCheque.Saldo += monttoTransaccionCheque;
								cuentacomponenteVistaSaldoCheque.Numeroverificador += 1;

								context.Attach(cuentacomponenteVistaSaldoCheque);
								context.SaveChanges();
							}

							saldoTotalCuentas += cuentacomponenteVistaSaldoCheque.Saldo;

							//CON 15
							tableName = "CON15 movimientocuentacompVistaCheque";
							var saldoTotalCuenta = cuentacomponenteVista.Sum(a => a.Saldo) + monttoTransaccionCheque;
							MovimientocuentacompVista movimientocuentacompVista = new MovimientocuentacompVista
							{
								Secuencialmovimientodetalle = movimientodetalle.Secuencial,
								Secuencialcuenta = model.SecuencialCuenta,
								Secuencialcomponentevista = 2,
								Codigotipomovimiento = "Cheque",
								Valor = monttoTransaccionCheque,
								Saldo = cuentacomponenteVistaSaldoCheque.Saldo, //montoTransaccion,
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
								Detalle = model.NombreTransaccion + " " + model.CodigoCuenta.ToString(),
								Estacontabilizado = false,
								Secuencialcuentacontable = cuentaContable.SecuencialCuentaContable,
								Secuencialoficina = model.SecOficina,
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
									Saldocuenta = saldoTotalCuenta, // cuentacomponenteVista.Saldo,
									Codigoestadocuenta = "A"
								};
								context.MovimientodetalleCuenta.Add(movimientodetalleCuentaCheque);
								context.SaveChanges();
							}
							else
							{
								existMovimientoDetalleCuenta.Saldocuenta = saldoTotalCuenta;
								context.Attach(existMovimientoDetalleCuenta);
								context.SaveChanges();
							}


							////CON 21
							tableName = "CON21 ventanilla - Cheque";
							var getVentanillaCheque = context.Ventanilla.FirstOrDefault(a => a.Codigousuario == model.CodigoUsuario && a.Secuencialoficina == model.SecOficina && a.Fecha == fechaCajero);
							int secuencialVentanillaCheque = 0;
							bool existeVentanillaeCheque;
							if (getVentanillaCheque == null)
							{
								existeVentanillaeCheque = false;
								Ventanilla ventanillaCheque = new Ventanilla
								{
									Codigousuario = model.CodigoUsuario,
									Secuencialoficina = model.SecOficina,
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
								Secuencialoficinaafectada = model.SecOficina
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
										Cantidad = 1,
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
									Cantidad = 1,
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
								Cantidad = 1,
								Valor = monttoTransaccionCheque,
								Saldo = monttoTransaccionCheque
							};
							context.MovimientoventanillacompCaja.Add(movimientoventanillacompCaja);
							context.SaveChanges();

							//28   
							tableName = "CON28 RegistrocontableCheque";
							var codigoCuentaContable = context.ComponenteCuentaContable.FirstOrDefault(a => a.Secuencialcomponente == 26);
							Registrocontable registrocontable2 = new Registrocontable
							{
								Valor = monttoTransaccionCheque,
								Esdebito = true,
								Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
								Detalle = model.CodigoUsuario + " " + "Efectivo",
								Estacontabilizado = false,
								Secuencialcuentacontable = codigoCuentaContable.SecuencialCuentaContable,
								Secuencialoficina = model.SecOficina,
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

								int diasEfectivizacion = context.Ruta.FirstOrDefault(a => a.Secuencialbancoemisor == item.SecuencialBancoEmisor).Diastransito;
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

								Chequeefectivizacion chequeEfectivizacion = new Chequeefectivizacion
								{
									SecuencialCheque = cheque.Secuencial,
									FechaMaquina = DateTime.Now,
									FechaSistema = fechaCajero.AddDays(diasEfectivizacion), //TODO AGREGA CAMPOS DIAS DE EFECTIVACION DE CHEQUE SEGUN BANCO SELECCIONADO
									CodigoUsuario = model.CodigoUsuario,
									SecuencialOficina = model.SecOficina,
									Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
									EsManual = false,
									EstuvoenTransito = false
								};

								context.Chequeefectivizacion.Add(chequeEfectivizacion);
								context.SaveChanges();
							}
						}
						else
						{
							var cuentacomponenteVista = context.CuentacomponenteVista.Where(a => a.Secuencialcuenta == model.SecuencialCuenta).ToList();
							var cuentacomponenteVistaSaldoCheque = cuentacomponenteVista.FirstOrDefault(a => a.Secuencialcomponentevista == 2);

							saldoTotalCuentas += cuentacomponenteVistaSaldoCheque.Saldo;
						}

						tableName = "Transaccionmobile";
						Transaccionmobile tmobile = new Transaccionmobile
						{
							CodigoUsuario = model.CodigoUsuario,
							NumeroCliente = model.numCliente,
							Fecha = DateTime.Now,
							Monto = montoTransaccionTotal,
							Longitud = 0,
							Latitud=0
						};
						context.Transaccionmobile.Add(tmobile);
						context.SaveChanges();

					}

					result.Saldodeposito = saldoTotalCuentas;
					result.Result = true;
					transaction.Commit();
				}
				catch (Exception ex)
				{
					errorLog.MainLog("SaveTransaccion " + tableName + " Error: " +  ex.Message.ToString());
					result.Result = false;
					transaction.Rollback();
				}

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
	}
}
