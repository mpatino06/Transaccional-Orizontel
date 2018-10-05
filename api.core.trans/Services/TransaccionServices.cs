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


		public bool SaveTransaccion(RegistrarTransaccion model)
		{
			bool result = true;
			string tableName = string.Empty;
			try
			{
				using(var transaction = context.Database.BeginTransaction())
				{
					int secEmpresa = 0, secCliente=0, secOficina = 0, numCliente, secCuenta= 0, codigoCuenta=0, secTransaccion = 18, secMoneda=0, secCuentaSeleccionada =0;
					decimal montoTransaccion = 0;
					string codUsuario = "", codTipoMovimiento = "";

					//CON 8
					tableName = "EmpresaDocumento";
					var _empresaDocumento = context.EmpresaDocumento.FirstOrDefault(a => a.Secuencialempresa == secEmpresa);
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
							Fecha = DateTime.Now.Date,
							Fechamaquina = DateTime.Now, //TODO esta fecha tiene que venir con horas minutos y segundos
							Codigousuario = codUsuario,
							Secuencialoficinausuario = secOficina,
							Estaimpreso = false,
							Numeroverificador = 1
						};
						context.Movimiento.Add(movimiento);
						context.SaveChanges();



						//12
						var _cuentaMaestro = context.Cuentamaestro.FirstOrDefault(a => a.Secuencial == secCuentaSeleccionada);
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
							Secuencialtransaccion = secTransaccion,
							Secuencialmoneda = secMoneda,
							Valor = montoTransaccion,
							Secuencialoficinaafectada = secOficina
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
							var cuentacomponenteVista = context.CuentacomponenteVista.Where(a => a.Secuencialcuenta == secCuenta).ToList();
							var cuentacomponenteVistaSaldoEfectivo = cuentacomponenteVista.FirstOrDefault(a => a.Secuencialcomponentevista == 1);
							if (cuentacomponenteVistaSaldoEfectivo != null)
							{

								cuentacomponenteVistaSaldoEfectivo.Saldo += montoTransaccion;
								cuentacomponenteVistaSaldoEfectivo.Numeroverificador += 1;

								context.Attach(cuentacomponenteVistaSaldoEfectivo);
								context.SaveChanges();
							}

							//CON 15
							tableName = "CON15 movimientocuentacompVistaEfectivo";
							var saldoTotalCuenta = cuentacomponenteVista.Sum(a => a.Saldo) + montoTransaccion;
							MovimientocuentacompVista movimientocuentacompVista = new MovimientocuentacompVista
							{
								Secuencialmovimientodetalle = movimientodetalle.Secuencial,
								Secuencialcuenta = secCuenta,
								Secuencialcomponentevista = 1,
								Codigotipomovimiento = codTipoMovimiento,
								Valor = montoTransaccion,
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
								Valor = montoTransaccion,
								Esdebito = false,
								Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
								Detalle = model.NombreTransaccion + " " + codigoCuenta.ToString(),
								Estacontabilizado = false,
								Secuencialcuentacontable = cuentaContable.SecuencialCuentaContable,
								Secuencialoficina = secOficina,
								Secuencialperfilcontable = 1,
								Codigousuario = codUsuario,
								Secuencialmoneda = secMoneda,
								Fechasistemaregistro = DateTime.Now.Date,
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
								Depositos = montoTransaccion.ToString(),
								Retiros = "0",
								Saldo = saldoTotalCuenta.ToString(), // cuentacomponenteVista.Saldo.ToString(),
								Transaccion = model.SiglasTransaccion,
								Secuencialcliente = secCliente,
								Secuencialcuenta = secCuenta,
								Operador = codUsuario,
								Estaimpresa = false,
								Numeoverificador = 0,
								Efectivo = montoTransaccion.ToString(),
								Saldodisponible = saldoTotalCuenta.ToString(),  //cuentacomponenteVista.Saldo.ToString(),
								Saldoobligatorios = "0",
								Valortransaccion = montoTransaccion.ToString(),
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
								Secuencialcuenta = secCuenta,
								Saldocuenta = saldoTotalCuenta, // cuentacomponenteVista.Saldo,
								Codigoestadocuenta = "A"
							};
							context.MovimientodetalleCuenta.Add(movimientodetalleCuenta);
							context.SaveChanges();


							////CON 21
							tableName = "CON21 ventanilla - Efecivo";
							var getVentanillaCheque = context.Ventanilla.FirstOrDefault(a => a.Secuencial == secCuenta);
							int secuencialVentanillaEfectivo = 0;
							if (getVentanillaCheque == null)
							{
								Ventanilla ventanillaEfectivo = new Ventanilla
								{
									Codigousuario = codUsuario,
									Secuencialoficina = secOficina,
									Fecha = DateTime.Now,
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
								secuencialVentanillaEfectivo = getVentanillaCheque.Secuencial;
								getVentanillaCheque.Numeroverificador += 1;
								context.Attach(getVentanillaCheque);
								context.SaveChanges();
							}

	


							//CON 22 SE REPITE PORQUE GUARDA LA TRANSACCION EN CAJA QUE ES CODIGO 3
							tableName = "CON22 Movimientodetalle";
							Movimientodetalle movimientodetalle2 = new Movimientodetalle
							{
								Secuencialmovimiento = movimiento.Secuencial,
								Secuencialtransaccion = 3,
								Secuencialmoneda = secMoneda,
								Valor = montoTransaccion,
								Secuencialoficinaafectada = secOficina
							};
							context.Movimientodetalle.Add(movimientodetalle2);
							context.SaveChanges();

							//CON 26  Secuencialcomponentecaja = 26 => Efectivo ...... 27 => ChequeIngreso
							tableName = "CON26 ventanillacomponente";
							VentanillacomponenteCaja ventanillacomponente = new VentanillacomponenteCaja
							{
								Secuencialventanilla = secuencialVentanillaEfectivo,
								Secuencialcomponentecaja = 26,
								Secuencialmoneda = secMoneda,
								Cantidad = 1, //CREO q ES LA CANTIDA DE billete  
								Saldo = montoTransaccion, //creo q es la cantidad de la moneda
								Valorcuadre = 0
							};
							context.VentanillacomponenteCaja.Add(ventanillacomponente);
							context.SaveChanges();

							//27 VALIDAR ESTA CONSULTA TIENE QUE VER CON LOS BILLETES Y TRANSACCIONES DEL MONTO
							tableName = "CON27 MovimientoventanillacompCaja";
							MovimientoventanillacompCaja movimientoventanillacompCaja = new MovimientoventanillacompCaja
							{
								Secuencialmovimientodetalle = movimientodetalle2.Secuencial, //Revisar se hace insercion en dos movimientos detalle revisar cual
								Secuencialventanillacompcaja = ventanillacomponente.Secuencial,
								Codigotipomovimientocaja = "Efectivo",
								Cantidad = 1,
								Valor = montoTransaccion,
								Saldo = montoTransaccion
							};
							context.MovimientoventanillacompCaja.Add(movimientoventanillacompCaja);
							context.SaveChanges();

							//28
							tableName = "CON28 Registrocontable";
							var codigoCuentaContable = context.ComponenteCuentaContable.FirstOrDefault(a => a.Secuencialcomponente == 26);
							Registrocontable registrocontable2 = new Registrocontable
							{
								Valor = montoTransaccion,
								Esdebito = true,
								Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
								Detalle = codUsuario + " " + "Efectivo",
								Estacontabilizado = false,
								Secuencialcuentacontable = codigoCuentaContable.SecuencialCuentaContable,
								Secuencialoficina = secOficina,
								Secuencialperfilcontable = 1,
								Codigousuario = codUsuario,
								Secuencialmoneda = secMoneda,
								Fechasistemaregistro = DateTime.Now.Date,
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


							//29 (DENOMINACION DE BILLETES) //AQUI DEBE IR EL BUCLE DE LAS MONEDAS
							Ventanillacomponentedenomnefe ventanillacomponentedenomnefe = new Ventanillacomponentedenomnefe
							{
								Secuencialventanillacompcaja = ventanillacomponente.Secuencial,
								Denominacion = 10,
								Cantidad = 1, // moneda selecciona 
							};
						}

						//VALIDA SI EL DEPOSITO FUE EN CHEQUE
						if (trs.TipoMovimiento.FirstOrDefault(a => a.Codigotipomovimiento == "Cheque").ValueInsert > 0)
						{
							//CON 14
							tableName = "CON14 CuentacomponenteVistaCheque";
							//var cuentacomponenteVista = context.CuentacomponenteVista.FirstOrDefault(a => a.Secuencialcuenta == secCuenta && a.Secuencialcomponentevista == 1);
							var cuentacomponenteVista = context.CuentacomponenteVista.Where(a => a.Secuencialcuenta == secCuenta).ToList();
							var cuentacomponenteVistaSaldoEfectivo = cuentacomponenteVista.FirstOrDefault(a => a.Secuencialcomponentevista == 2);
							if (cuentacomponenteVistaSaldoEfectivo != null)
							{

								cuentacomponenteVistaSaldoEfectivo.Saldo += montoTransaccion;
								cuentacomponenteVistaSaldoEfectivo.Numeroverificador += 1;

								context.Attach(cuentacomponenteVistaSaldoEfectivo);
								context.SaveChanges();
							}

							//CON 15
							tableName = "CON15 movimientocuentacompVistaCheque";
							var saldoTotalCuenta = cuentacomponenteVista.Sum(a => a.Saldo) + montoTransaccion;
							MovimientocuentacompVista movimientocuentacompVista = new MovimientocuentacompVista
							{
								Secuencialmovimientodetalle = movimientodetalle.Secuencial,
								Secuencialcuenta = secCuenta,
								Secuencialcomponentevista = 2,
								Codigotipomovimiento = codTipoMovimiento,
								Valor = montoTransaccion,
								Saldo = cuentacomponenteVistaSaldoEfectivo.Saldo, //montoTransaccion,
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
								Valor = montoTransaccion,
								Esdebito = false,
								Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
								Detalle = model.NombreTransaccion + " " + codigoCuenta.ToString(),
								Estacontabilizado = false,
								Secuencialcuentacontable = cuentaContable.SecuencialCuentaContable,
								Secuencialoficina = secOficina,
								Secuencialperfilcontable = 1,
								Codigousuario = codUsuario,
								Secuencialmoneda = secMoneda,
								Fechasistemaregistro = DateTime.Now.Date,
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
								Depositos = montoTransaccion.ToString(),
								Retiros = "0",
								Saldo = saldoTotalCuenta.ToString(), // cuentacomponenteVista.Saldo.ToString(),
								Transaccion = model.SiglasTransaccion,
								Secuencialcliente = secCliente,
								Secuencialcuenta = secCuenta,
								Operador = codUsuario,
								Estaimpresa = false,
								Numeoverificador = 0,
								Efectivo = "0,00",
								Cheque = montoTransaccion.ToString(),
								Saldodisponible = saldoTotalCuenta.ToString(),  //cuentacomponenteVista.Saldo.ToString(),
								Saldoobligatorios = "0",
								Valortransaccion = montoTransaccion.ToString(),
								Eslinearendfinanc = false,
								Detallerendfinanc = ""
							};
							context.Movimientoimpresion.Add(movimientoimpresion);
							context.SaveChanges();

							//CON 20 
							tableName = "CON20 MovimientodetalleCuentaCheque";
							MovimientodetalleCuenta movimientodetalleCuenta = new MovimientodetalleCuenta
							{
								Secuencialmovimientodetalle = movimientodetalle.Secuencial,
								Secuencialcuenta = secCuenta,
								Saldocuenta = saldoTotalCuenta, // cuentacomponenteVista.Saldo,
								Codigoestadocuenta = "A"
							};
							context.MovimientodetalleCuenta.Add(movimientodetalleCuenta);
							context.SaveChanges();


							////CON 21
							//tableName = "CON21 ventanilla";
							Ventanilla ventanilla = new Ventanilla
							{
								Codigousuario = codUsuario,
								Secuencialoficina = secOficina,
								Fecha = DateTime.Now,
								Abiertaautomaticamente = false,
								Estacerrada = false,
								Estacuadrada = false,
								Numerovecescuadrada = 0,
								Numeroverificador = 0
							};
							context.Ventanilla.Add(ventanilla);
							context.SaveChanges();


							//CON 22 SE REPITE PORQUE GUARDA LA TRANSACCION EN CAJA QUE ES CODIGO 3
							tableName = "CON22 MovimientodetalleCheque";
							Movimientodetalle movimientodetalleCheque2 = new Movimientodetalle
							{
								Secuencialmovimiento = movimiento.Secuencial,
								Secuencialtransaccion = 3,
								Secuencialmoneda = secMoneda,
								Valor = montoTransaccion,
								Secuencialoficinaafectada = secOficina
							};
							context.Movimientodetalle.Add(movimientodetalleCheque2);
							context.SaveChanges();

							//CON 26  Secuencialcomponentecaja = 26 => Efectivo ...... 27 => ChequeIngreso
							tableName = "CON26 ventanillacomponenteCheque";
							VentanillacomponenteCaja ventanillacomponente = new VentanillacomponenteCaja
							{
								Secuencialventanilla = ventanilla.Secuencial,
								Secuencialcomponentecaja = 27,
								Secuencialmoneda = secMoneda,
								Cantidad = 1, //CREO q ES LA CANTIDA DE billete  
								Saldo = montoTransaccion, //creo q es la cantidad de la moneda
								Valorcuadre = 0
							};
							context.VentanillacomponenteCaja.Add(ventanillacomponente);
							context.SaveChanges();

							//27 VALIDAR ESTA CONSULTA TIENE QUE VER CON LOS BILLETES Y TRANSACCIONES DEL MONTO
							tableName = "CON27 MovimientoventanillacompCajaCheque";
							MovimientoventanillacompCaja movimientoventanillacompCaja = new MovimientoventanillacompCaja
							{
								Secuencialmovimientodetalle = movimientodetalleCheque2.Secuencial, //Revisar se hace insercion en dos movimientos detalle revisar cual
								Secuencialventanillacompcaja = ventanillacomponente.Secuencial,
								Codigotipomovimientocaja = "ChequeIngreso",
								Cantidad = 1,
								Valor = montoTransaccion,
								Saldo = montoTransaccion
							};
							context.MovimientoventanillacompCaja.Add(movimientoventanillacompCaja);
							context.SaveChanges();

							//28   
							tableName = "CON28 RegistrocontableCheque";
							var codigoCuentaContable = context.ComponenteCuentaContable.FirstOrDefault(a => a.Secuencialcomponente == 26);
							Registrocontable registrocontable2 = new Registrocontable
							{
								Valor = montoTransaccion,
								Esdebito = true,
								Documento = _empresaDocumento.Ultimonumerodocumentomov.ToString(),
								Detalle = codUsuario + " " + "Efectivo",
								Estacontabilizado = false,
								Secuencialcuentacontable = codigoCuentaContable.SecuencialCuentaContable,
								Secuencialoficina = secOficina,
								Secuencialperfilcontable = 1,
								Codigousuario = codUsuario,
								Secuencialmoneda = secMoneda,
								Fechasistemaregistro = DateTime.Now.Date,
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
								var cheque = new Cheque
								{
									CodigoCuentaCorriente = item.CodigoCuentaCorriente,
									CodigoCheque = item.CodigoCheque,
									SecuencialBancoEmisor= item.SecuencialBancoEmisor,
									SecuencialMoneda = secMoneda,
									Valor = item.Valor,
									CodigoUsuario = codUsuario,
									Estaenboveda = false,
									FechaSistemaIngreso = DateTime.Now.Date,
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

							}
						}


					}
				}
			}
			catch (Exception ex)
			{
				errorLog.MainLog("SaveTransaccon " + tableName + " MESSAGE " + ex.Message.ToString());
				result = false;
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
	}
}
