using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace api.core.trans.Models
{
    public partial class FBS_SacPelileoContext : DbContext
    {
        public FBS_SacPelileoContext()
        {
        }

        public FBS_SacPelileoContext(DbContextOptions<FBS_SacPelileoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Comentariocliente> Comentariocliente { get; set; }
        public virtual DbSet<ComponenteCaja> ComponenteCaja { get; set; }
        public virtual DbSet<ComponenteVista> ComponenteVista { get; set; }
        public virtual DbSet<Cuentacliente> Cuentacliente { get; set; }
        public virtual DbSet<CuentacomponenteVista> CuentacomponenteVista { get; set; }
        public virtual DbSet<Cuentamaestro> Cuentamaestro { get; set; }
        public virtual DbSet<Division> Division { get; set; }
        public virtual DbSet<Empresadenominacionfija> Empresadenominacionfija { get; set; }
        public virtual DbSet<EmpresaDocumento> EmpresaDocumento { get; set; }
        public virtual DbSet<Estadocuenta> Estadocuenta { get; set; }
        public virtual DbSet<Movimiento> Movimiento { get; set; }
        public virtual DbSet<MovimientocomponenteCausal> MovimientocomponenteCausal { get; set; }
        public virtual DbSet<MovimientocuentacompVista> MovimientocuentacompVista { get; set; }
        public virtual DbSet<Movimientodetalle> Movimientodetalle { get; set; }
        public virtual DbSet<MovimientodetalleCuenta> MovimientodetalleCuenta { get; set; }
        public virtual DbSet<Movimientoimpresion> Movimientoimpresion { get; set; }
        public virtual DbSet<MovimientoventanillacompCaja> MovimientoventanillacompCaja { get; set; }
        public virtual DbSet<MovimientoventcompCajadet> MovimientoventcompCajadet { get; set; }
        public virtual DbSet<Oficina> Oficina { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Tipocuenta> Tipocuenta { get; set; }
        public virtual DbSet<Tipoidentificacion> Tipoidentificacion { get; set; }
        public virtual DbSet<Transaccion> Transaccion { get; set; }
        public virtual DbSet<Transaccioncomponente> Transaccioncomponente { get; set; }
        public virtual DbSet<Transaccionrangoaprobacion> Transaccionrangoaprobacion { get; set; }
        public virtual DbSet<TransaccionVista> TransaccionVista { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioComplemento> UsuarioComplemento { get; set; }
        public virtual DbSet<Ventanilla> Ventanilla { get; set; }
        public virtual DbSet<VentanillacomponenteCaja> VentanillacomponenteCaja { get; set; }
        public virtual DbSet<Ventanillacomponentedenomnefe> Ventanillacomponentedenomnefe { get; set; }
		public virtual DbSet<Transacciontipomovimiento> Transacciontipomovimiento { get; set; }
		public virtual DbSet<Registrocontable> Registrocontable { get; set; }
		public virtual DbSet<ComponenteCuentaContable> ComponenteCuentaContable { get; set; }
		public virtual DbSet<Banco> Banco { get; set; }
		public virtual DbSet<Cheque> Cheque { get; set; }
		public virtual DbSet<ChequeMovimientoDetalle> ChequeMovimientoDetalle { get; set; }
		public virtual DbSet<Transaccionmobile> Transaccionmobile { get; set; }
		public virtual DbSet<Empresafechacajero> Empresafechacajero { get; set; }
		public virtual DbSet<Chequeefectivizacion> Chequeefectivizacion { get; set; }
		public virtual DbSet<Ruta> Ruta { get; set; }
		public virtual DbSet<Calendario> Calendario { get; set; }
		public virtual DbSet<UsuarioHorarioIngreso> UsuarioHorarioIngreso { get; set; }
		public virtual DbSet<ComponenteVistaDisponible> ComponenteVistaDisponible { get; set; }
		public virtual DbSet<ComponenteVistaRetencion> ComponenteVistaRetencion { get; set; }
		public virtual DbSet<Componente> Componente { get; set; }
		public virtual DbSet<Cuentacontable> Cuentacontable { get; set; }
		public virtual DbSet<Movimientocontrol> Movimientocontrol { get; set; }
		public virtual DbSet<MovimientocontrolTransfInt> MovimientocontrolTransfInt { get; set; }
		public virtual DbSet<Usuariorol> Usuariorol { get; set; }


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			if (!optionsBuilder.IsConfigured)
			{
				var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json");

				var connectionStringConfig = builder.Build();
				
				optionsBuilder.UseSqlServer(connectionStringConfig.GetConnectionString("DefaultConnection"));
			}
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("CLIENTE", "FBS_CLIENTES");

                entity.HasIndex(e => e.Secuencialpersona)
                    .HasName("IX_CLIENTE")
                    .IsUnique();

                entity.HasIndex(e => new { e.Secuencial, e.Secuencialpersona })
                    .HasName("_dta_index_CLIENTE_14_757838012__K1_K3");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Codigocalificacioninterna)
                    .IsRequired()
                    .HasColumnName("CODIGOCALIFICACIONINTERNA")
                    .HasMaxLength(2);

                entity.Property(e => e.Codigoestadocliente)
                    .IsRequired()
                    .HasColumnName("CODIGOESTADOCLIENTE")
                    .HasMaxLength(2);

                entity.Property(e => e.Codigosectoreconomico)
                    .IsRequired()
                    .HasColumnName("CODIGOSECTORECONOMICO")
                    .HasMaxLength(8);

                entity.Property(e => e.Codigotipovinculacion)
                    .IsRequired()
                    .HasColumnName("CODIGOTIPOVINCULACION")
                    .HasMaxLength(6);

                entity.Property(e => e.Codigousuariooficial)
                    .IsRequired()
                    .HasColumnName("CODIGOUSUARIOOFICIAL")
                    .HasMaxLength(20);

                entity.Property(e => e.Fechaingreso)
                    .HasColumnName("FECHAINGRESO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Numerocliente).HasColumnName("NUMEROCLIENTE");

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Secuencialdivisionmercado).HasColumnName("SECUENCIALDIVISIONMERCADO");

                entity.Property(e => e.Secuencialoficina).HasColumnName("SECUENCIALOFICINA");

                entity.Property(e => e.Secuencialpersona).HasColumnName("SECUENCIALPERSONA");

                entity.HasOne(d => d.CodigousuariooficialNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.Codigousuariooficial)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CLIENTE_R03");

                entity.HasOne(d => d.SecuencialdivisionmercadoNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.Secuencialdivisionmercado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CLIENTE_R07");

                entity.HasOne(d => d.SecuencialoficinaNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.Secuencialoficina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CLIENTE_R01");

                entity.HasOne(d => d.SecuencialpersonaNavigation)
                    .WithOne(p => p.Cliente)
                    .HasForeignKey<Cliente>(d => d.Secuencialpersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CLIENTE_R02");
            });

            modelBuilder.Entity<Comentariocliente>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("COMENTARIOCLIENTE", "FBS_CLIENTES");

                entity.HasIndex(e => e.Secuencialcliente)
                    .HasName("IX_COMENTARIOCLIENTE");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Codigousuarioingreso)
                    .IsRequired()
                    .HasColumnName("CODIGOUSUARIOINGRESO")
                    .HasMaxLength(20);

                entity.Property(e => e.Comentario)
                    .IsRequired()
                    .HasColumnName("COMENTARIO")
                    .HasMaxLength(500);

                entity.Property(e => e.Estaactivo).HasColumnName("ESTAACTIVO");

                entity.Property(e => e.Fechaingreso)
                    .HasColumnName("FECHAINGRESO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fechamaquina)
                    .HasColumnName("FECHAMAQUINA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Secuencialcliente).HasColumnName("SECUENCIALCLIENTE");

                entity.HasOne(d => d.CodigousuarioingresoNavigation)
                    .WithMany(p => p.Comentariocliente)
                    .HasForeignKey(d => d.Codigousuarioingreso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("COMENTARIOCLIENTE_R02");

                entity.HasOne(d => d.SecuencialclienteNavigation)
                    .WithMany(p => p.Comentariocliente)
                    .HasForeignKey(d => d.Secuencialcliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("COMENTARIOCLIENTE_R01");
            });

            modelBuilder.Entity<ComponenteCaja>(entity =>
            {
                entity.HasKey(e => e.Secuencialcomponente);

                entity.ToTable("COMPONENTE_CAJA", "FBS_CAJAS");

                entity.Property(e => e.Secuencialcomponente)
                    .HasColumnName("SECUENCIALCOMPONENTE")
                    .ValueGeneratedNever();

                entity.Property(e => e.Codigotipocomponentecaja)
                    .IsRequired()
                    .HasColumnName("CODIGOTIPOCOMPONENTECAJA")
                    .HasMaxLength(44);
            });

            modelBuilder.Entity<ComponenteVista>(entity =>
            {
                entity.HasKey(e => e.Secuencialcomponente);

                entity.ToTable("COMPONENTE_VISTA", "FBS_CAPTACIONESVISTA");

                entity.Property(e => e.Secuencialcomponente)
                    .HasColumnName("SECUENCIALCOMPONENTE")
                    .ValueGeneratedNever();

                entity.Property(e => e.Admiteacreditacionprestamo).HasColumnName("ADMITEACREDITACIONPRESTAMO");

                entity.Property(e => e.Espartesaldo).HasColumnName("ESPARTESALDO");

                entity.Property(e => e.Secuencialtipocomponentevista).HasColumnName("SECUENCIALTIPOCOMPONENTEVISTA");
            });

            modelBuilder.Entity<Cuentacliente>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("CUENTACLIENTE", "FBS_CAPTACIONESVISTA");

                entity.HasIndex(e => e.Secuencialcliente)
                    .HasName("IX_CUENTACLIENTE");

                entity.HasIndex(e => e.Secuencialcuenta)
                    .HasName("IX_CUENTACLIENTE_1");

                entity.HasIndex(e => new { e.Esprincipal, e.Secuencialcliente, e.Secuencialcuenta })
                    .HasName("_dta_index_CUENTACLIENTE_14_674361717__K4_K3_K2");

                entity.HasIndex(e => new { e.Secuencialcuenta, e.Secuencial, e.Secuencialcliente })
                    .HasName("_dta_index_CUENTACLIENTE_14_674361717__K2_K1_K3");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Esprincipal).HasColumnName("ESPRINCIPAL");

                entity.Property(e => e.Estaactivo).HasColumnName("ESTAACTIVO");

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Secuencialcliente).HasColumnName("SECUENCIALCLIENTE");

                entity.Property(e => e.Secuencialcuenta).HasColumnName("SECUENCIALCUENTA");

                entity.HasOne(d => d.SecuencialclienteNavigation)
                    .WithMany(p => p.Cuentacliente)
                    .HasForeignKey(d => d.Secuencialcliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CUENTACLIENTE_R02");

                entity.HasOne(d => d.SecuencialcuentaNavigation)
                    .WithMany(p => p.Cuentacliente)
                    .HasForeignKey(d => d.Secuencialcuenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CUENTACLIENTE_CUENTAMAESTRO");
            });

            modelBuilder.Entity<CuentacomponenteVista>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("CUENTACOMPONENTE_VISTA", "FBS_CAPTACIONESVISTA");

                entity.HasIndex(e => e.Secuencialcomponentevista)
                    .HasName("IX_CUENTACOMPONENTE_VISTA_2");

                entity.HasIndex(e => e.Secuencialcuenta)
                    .HasName("IX_CUENTACOMPONENTE_VISTA_1");

                entity.HasIndex(e => new { e.Secuencialcuenta, e.Secuencialcomponentevista })
                    .HasName("IX_00037")
                    .IsUnique();

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Saldo)
                    .HasColumnName("SALDO")
                    .HasColumnType("decimal(18, 6)");

                entity.Property(e => e.Secuencialcomponentevista).HasColumnName("SECUENCIALCOMPONENTEVISTA");

                entity.Property(e => e.Secuencialcuenta).HasColumnName("SECUENCIALCUENTA");

                entity.HasOne(d => d.SecuencialcomponentevistaNavigation)
                    .WithMany(p => p.CuentacomponenteVista)
                    .HasForeignKey(d => d.Secuencialcomponentevista)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CUENTACOMPONENTE_VISTA_R02");

                entity.HasOne(d => d.SecuencialcuentaNavigation)
                    .WithMany(p => p.CuentacomponenteVista)
                    .HasForeignKey(d => d.Secuencialcuenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CUENTACOMPONENTE_VISTA_CUENTAMAESTRO");
            });

            modelBuilder.Entity<Cuentamaestro>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("CUENTAMAESTRO", "FBS_CAPTACIONESVISTA");

                entity.HasIndex(e => e.Codigo)
                    .HasName("IX_00038")
                    .IsUnique();

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Bloqueadatransaccionoperativa).HasColumnName("BLOQUEADATRANSACCIONOPERATIVA");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasColumnName("CODIGO")
                    .HasMaxLength(40);

                entity.Property(e => e.Codigoestado)
                    .IsRequired()
                    .HasColumnName("CODIGOESTADO")
                    .HasMaxLength(2);

                entity.Property(e => e.Codigoproductovista)
                    .IsRequired()
                    .HasColumnName("CODIGOPRODUCTOVISTA")
                    .HasMaxLength(40);

                entity.Property(e => e.Codigotipocuenta)
                    .IsRequired()
                    .HasColumnName("CODIGOTIPOCUENTA")
                    .HasMaxLength(40);

                entity.Property(e => e.Codigousuariooficial)
                    .IsRequired()
                    .HasColumnName("CODIGOUSUARIOOFICIAL")
                    .HasMaxLength(20);

                entity.Property(e => e.Esanverso).HasColumnName("ESANVERSO");

                entity.Property(e => e.Fechacorte)
                    .HasColumnName("FECHACORTE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fechamaquinacreacion)
                    .HasColumnName("FECHAMAQUINACREACION")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fechasistemacreacion)
                    .HasColumnName("FECHASISTEMACREACION")
                    .HasColumnType("datetime");

                entity.Property(e => e.Numerolibreta).HasColumnName("NUMEROLIBRETA");

                entity.Property(e => e.Numerolineaimprimelibreta).HasColumnName("NUMEROLINEAIMPRIMELIBRETA");

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Secuencialmoneda).HasColumnName("SECUENCIALMONEDA");

                entity.Property(e => e.Secuencialoficina).HasColumnName("SECUENCIALOFICINA");

                entity.Property(e => e.Tieneseguroactivo).HasColumnName("TIENESEGUROACTIVO");

                entity.HasOne(d => d.CodigoestadoNavigation)
                    .WithMany(p => p.Cuentamaestro)
                    .HasForeignKey(d => d.Codigoestado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CUENTAMAESTRO_R03");

                entity.HasOne(d => d.CodigotipocuentaNavigation)
                    .WithMany(p => p.Cuentamaestro)
                    .HasForeignKey(d => d.Codigotipocuenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CUENTAMAESTRO_R01");

                entity.HasOne(d => d.CodigousuariooficialNavigation)
                    .WithMany(p => p.Cuentamaestro)
                    .HasForeignKey(d => d.Codigousuariooficial)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CUENTAMAESTRO_R06");

                entity.HasOne(d => d.SecuencialoficinaNavigation)
                    .WithMany(p => p.Cuentamaestro)
                    .HasForeignKey(d => d.Secuencialoficina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CUENTAMAESTRO_R05");
            });

            modelBuilder.Entity<Division>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("DIVISION", "FBS_GENERALES");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasColumnName("CODIGO")
                    .HasMaxLength(40);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Secuencialnivel).HasColumnName("SECUENCIALNIVEL");
            });

            modelBuilder.Entity<Empresadenominacionfija>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("EMPRESADENOMINACIONFIJA", "FBS_CAJAS");

                entity.HasIndex(e => new { e.Secuencialempresa, e.Denominacion })
                    .HasName("IX_00013")
                    .IsUnique();

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Denominacion).HasColumnName("DENOMINACION");

                entity.Property(e => e.Estaactiva).HasColumnName("ESTAACTIVA");

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Orden).HasColumnName("ORDEN");

                entity.Property(e => e.Secuencialempresa).HasColumnName("SECUENCIALEMPRESA");
            });

            modelBuilder.Entity<EmpresaDocumento>(entity =>
            {
                entity.HasKey(e => e.Secuencialempresa);

                entity.ToTable("EMPRESA_DOCUMENTO", "FBS_GENERALES");

                entity.Property(e => e.Secuencialempresa)
                    .HasColumnName("SECUENCIALEMPRESA")
                    .ValueGeneratedNever();

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Ultimonumerodocumentomov).HasColumnName("ULTIMONUMERODOCUMENTOMOV");
            });

            modelBuilder.Entity<Estadocuenta>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("ESTADOCUENTA", "FBS_CAPTACIONESVISTA");

                entity.Property(e => e.Codigo)
                    .HasColumnName("CODIGO")
                    .HasMaxLength(2)
                    .ValueGeneratedNever();

                entity.Property(e => e.Estaactivo).HasColumnName("ESTAACTIVO");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(20);

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");
            });

            modelBuilder.Entity<Movimiento>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("MOVIMIENTO", "FBS_NEGOCIOSFINANCIEROS");

                entity.HasIndex(e => e.Documento)
                    .HasName("IX_000183")
                    .IsUnique();

                entity.HasIndex(e => new { e.Codigousuario, e.Fecha })
                    .HasName("IX_MOVIMIENTO_1");

                entity.HasIndex(e => new { e.Fecha, e.Codigousuario })
                    .HasName("IX_MOVIMIENTO");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Codigousuario)
                    .IsRequired()
                    .HasColumnName("CODIGOUSUARIO")
                    .HasMaxLength(20);

                entity.Property(e => e.Documento)
                    .IsRequired()
                    .HasColumnName("DOCUMENTO")
                    .HasMaxLength(100);

                entity.Property(e => e.Estaimpreso).HasColumnName("ESTAIMPRESO");

                entity.Property(e => e.Fecha)
                    .HasColumnName("FECHA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fechamaquina)
                    .HasColumnName("FECHAMAQUINA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Secuencialoficinausuario).HasColumnName("SECUENCIALOFICINAUSUARIO");

                entity.HasOne(d => d.CodigousuarioNavigation)
                    .WithMany(p => p.Movimiento)
                    .HasForeignKey(d => d.Codigousuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MOVIMIENTO_R01");

                entity.HasOne(d => d.SecuencialoficinausuarioNavigation)
                    .WithMany(p => p.Movimiento)
                    .HasForeignKey(d => d.Secuencialoficinausuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MOVIMIENTO_R02");
            });

            modelBuilder.Entity<MovimientocomponenteCausal>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("MOVIMIENTOCOMPONENTE_CAUSAL", "FBS_CAUSALES");

                entity.HasIndex(e => e.Secuencialmovimientodetalle)
                    .HasName("IX_MOVIMIENTOCOMPONENTE_CAUSAL");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Codigotipomovimiento)
                    .IsRequired()
                    .HasColumnName("CODIGOTIPOMOVIMIENTO")
                    .HasMaxLength(30);

                entity.Property(e => e.Concepto)
                    .IsRequired()
                    .HasColumnName("CONCEPTO")
                    .HasMaxLength(500);

                entity.Property(e => e.Documentoinstitucional)
                    .HasColumnName("DOCUMENTOINSTITUCIONAL")
                    .HasMaxLength(50);

                entity.Property(e => e.Secuencialcomponentecausal).HasColumnName("SECUENCIALCOMPONENTECAUSAL");

                entity.Property(e => e.Secuencialmovimientodetalle).HasColumnName("SECUENCIALMOVIMIENTODETALLE");

                entity.Property(e => e.Valor).HasColumnName("VALOR");
            });

            modelBuilder.Entity<MovimientocuentacompVista>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("MOVIMIENTOCUENTACOMP_VISTA", "FBS_CAPTACIONESVISTA");

                entity.HasIndex(e => e.Secuencialcuenta)
                    .HasName("IX_MOVIMIENTOCUENTACOMP_VISTA_3");

                entity.HasIndex(e => e.Secuencialmovimientodetalle)
                    .HasName("IX_MOVIMIENTOCUENTACOMP_VISTA_2");

                entity.HasIndex(e => new { e.Secuencialcuenta, e.Secuencialmovimientodetalle })
                    .HasName("IX_MOVIMIENTOCUENTACOMP_VISTA");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Codigotipomovimiento)
                    .IsRequired()
                    .HasColumnName("CODIGOTIPOMOVIMIENTO")
                    .HasMaxLength(30);

                entity.Property(e => e.Saldo).HasColumnName("SALDO");

                entity.Property(e => e.Saldocuenta).HasColumnName("SALDOCUENTA");

                entity.Property(e => e.Secuencialcomponentevista).HasColumnName("SECUENCIALCOMPONENTEVISTA");

                entity.Property(e => e.Secuencialcuenta).HasColumnName("SECUENCIALCUENTA");

                entity.Property(e => e.Secuencialmovimientodetalle).HasColumnName("SECUENCIALMOVIMIENTODETALLE");

                entity.Property(e => e.Valor).HasColumnName("VALOR");

                entity.HasOne(d => d.SecuencialcomponentevistaNavigation)
                    .WithMany(p => p.MovimientocuentacompVista)
                    .HasForeignKey(d => d.Secuencialcomponentevista)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MOVIMIENTOCUENTACOMP_VISTA_R03");

                entity.HasOne(d => d.SecuencialcuentaNavigation)
                    .WithMany(p => p.MovimientocuentacompVista)
                    .HasForeignKey(d => d.Secuencialcuenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MOVIMIENTOCUENTACOMP_VISTA_CUENTAMAESTRO");

                entity.HasOne(d => d.SecuencialmovimientodetalleNavigation)
                    .WithMany(p => p.MovimientocuentacompVista)
                    .HasForeignKey(d => d.Secuencialmovimientodetalle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MOVIMIENTOCUENTACOMP_VISTA_R01");
            });

            modelBuilder.Entity<Movimientodetalle>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("MOVIMIENTODETALLE", "FBS_NEGOCIOSFINANCIEROS");

                entity.HasIndex(e => e.Secuencialmovimiento)
                    .HasName("IX_MOVIMIENTODETALLE");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Secuencialmoneda).HasColumnName("SECUENCIALMONEDA");

                entity.Property(e => e.Secuencialmovimiento).HasColumnName("SECUENCIALMOVIMIENTO");

                entity.Property(e => e.Secuencialoficinaafectada).HasColumnName("SECUENCIALOFICINAAFECTADA");

                entity.Property(e => e.Secuencialtransaccion).HasColumnName("SECUENCIALTRANSACCION");

                entity.Property(e => e.Valor).HasColumnName("VALOR");

                entity.HasOne(d => d.SecuencialmovimientoNavigation)
                    .WithMany(p => p.Movimientodetalle)
                    .HasForeignKey(d => d.Secuencialmovimiento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MOVIMIENTODETALLE_R01");

                entity.HasOne(d => d.SecuencialoficinaafectadaNavigation)
                    .WithMany(p => p.Movimientodetalle)
                    .HasForeignKey(d => d.Secuencialoficinaafectada)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C0032900");

                entity.HasOne(d => d.SecuencialtransaccionNavigation)
                    .WithMany(p => p.Movimientodetalle)
                    .HasForeignKey(d => d.Secuencialtransaccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MOVIMIENTODETALLE_R02");
            });

            modelBuilder.Entity<MovimientodetalleCuenta>(entity =>
            {
                entity.HasKey(e => e.Secuencialmovimientodetalle);

                entity.ToTable("MOVIMIENTODETALLE_CUENTA", "FBS_CAPTACIONESVISTA");

                entity.HasIndex(e => e.Secuencialcuenta)
                    .HasName("IX_MOVIMIENTODETALLE_CUENTA");

                entity.Property(e => e.Secuencialmovimientodetalle)
                    .HasColumnName("SECUENCIALMOVIMIENTODETALLE")
                    .ValueGeneratedNever();

                entity.Property(e => e.Codigoestadocuenta)
                    .IsRequired()
                    .HasColumnName("CODIGOESTADOCUENTA")
                    .HasMaxLength(2);

                entity.Property(e => e.Saldocuenta).HasColumnName("SALDOCUENTA");

                entity.Property(e => e.Secuencialcuenta).HasColumnName("SECUENCIALCUENTA");

                entity.HasOne(d => d.CodigoestadocuentaNavigation)
                    .WithMany(p => p.MovimientodetalleCuenta)
                    .HasForeignKey(d => d.Codigoestadocuenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MOVIMIENTODETALLE_CUENTA_R03");

                entity.HasOne(d => d.SecuencialcuentaNavigation)
                    .WithMany(p => p.MovimientodetalleCuenta)
                    .HasForeignKey(d => d.Secuencialcuenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MOVIMIENTODETALLE_CUENTA_CUENTAMAESTRO");

                entity.HasOne(d => d.SecuencialmovimientodetalleNavigation)
                    .WithOne(p => p.MovimientodetalleCuenta)
                    .HasForeignKey<MovimientodetalleCuenta>(d => d.Secuencialmovimientodetalle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MOVIMIENTODETALLE_CUENTA_R02");
            });

            modelBuilder.Entity<Movimientoimpresion>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("MOVIMIENTOIMPRESION", "FBS_CAPTACIONESVISTA");

                entity.HasIndex(e => e.Secuencialcliente)
                    .HasName("IX_MOVIMIENTOIMPRESION");

                entity.HasIndex(e => e.Secuencialcuenta)
                    .HasName("IX_MOVIMIENTOIMPRESION_2");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Cheque)
                    .IsRequired()
                    .HasColumnName("CHEQUE")
                    .HasMaxLength(100);

                entity.Property(e => e.Depositos)
                    .IsRequired()
                    .HasColumnName("DEPOSITOS")
                    .HasMaxLength(100);

                entity.Property(e => e.Detallerendfinanc)
                    .IsRequired()
                    .HasColumnName("DETALLERENDFINANC")
                    .HasMaxLength(500);

                entity.Property(e => e.Efectivo)
                    .IsRequired()
                    .HasColumnName("EFECTIVO")
                    .HasMaxLength(100);

                entity.Property(e => e.Eslinearendfinanc).HasColumnName("ESLINEARENDFINANC");

                entity.Property(e => e.Estaimpresa).HasColumnName("ESTAIMPRESA");

                entity.Property(e => e.Fecha)
                    .HasColumnName("FECHA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Numeoverificador).HasColumnName("NUMEOVERIFICADOR");

                entity.Property(e => e.Operador)
                    .IsRequired()
                    .HasColumnName("OPERADOR")
                    .HasMaxLength(20);

                entity.Property(e => e.Retiros)
                    .IsRequired()
                    .HasColumnName("RETIROS")
                    .HasMaxLength(100);

                entity.Property(e => e.Saldo)
                    .IsRequired()
                    .HasColumnName("SALDO")
                    .HasMaxLength(100);

                entity.Property(e => e.Saldodisponible)
                    .IsRequired()
                    .HasColumnName("SALDODISPONIBLE")
                    .HasMaxLength(100);

                entity.Property(e => e.Saldoobligatorios)
                    .IsRequired()
                    .HasColumnName("SALDOOBLIGATORIOS")
                    .HasMaxLength(100);

                entity.Property(e => e.Secuencialcliente).HasColumnName("SECUENCIALCLIENTE");

                entity.Property(e => e.Secuencialcuenta).HasColumnName("SECUENCIALCUENTA");

                entity.Property(e => e.Transaccion)
                    .IsRequired()
                    .HasColumnName("TRANSACCION")
                    .HasMaxLength(100);

                entity.Property(e => e.Valortransaccion)
                    .IsRequired()
                    .HasColumnName("VALORTRANSACCION")
                    .HasMaxLength(100);

                entity.HasOne(d => d.SecuencialclienteNavigation)
                    .WithMany(p => p.Movimientoimpresion)
                    .HasForeignKey(d => d.Secuencialcliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MOVIMIENTOIMPRESION_R01");

                entity.HasOne(d => d.SecuencialcuentaNavigation)
                    .WithMany(p => p.Movimientoimpresion)
                    .HasForeignKey(d => d.Secuencialcuenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MOVIMIENTOIMPRESION_CUENTAMAESTRO");
            });

            modelBuilder.Entity<MovimientoventanillacompCaja>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("MOVIMIENTOVENTANILLACOMP_CAJA", "FBS_CAJAS");

                entity.HasIndex(e => e.Secuencialmovimientodetalle)
                    .HasName("IX_MOVIMIENTOVENTANILLACOMP_CAJA_2");

                entity.HasIndex(e => e.Secuencialventanillacompcaja)
                    .HasName("IX_MOVIMIENTOVENTANILLACOMP_CAJA_1");

                entity.HasIndex(e => new { e.Secuencialmovimientodetalle, e.Secuencialventanillacompcaja })
                    .HasName("IX_MOVIMIENTOVENTANILLACOMP_CAJA");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.Codigotipomovimientocaja)
                    .IsRequired()
                    .HasColumnName("CODIGOTIPOMOVIMIENTOCAJA")
                    .HasMaxLength(40);

                entity.Property(e => e.Saldo).HasColumnName("SALDO");

                entity.Property(e => e.Secuencialmovimientodetalle).HasColumnName("SECUENCIALMOVIMIENTODETALLE");

                entity.Property(e => e.Secuencialventanillacompcaja).HasColumnName("SECUENCIALVENTANILLACOMPCAJA");

                entity.Property(e => e.Valor).HasColumnName("VALOR");

                entity.HasOne(d => d.SecuencialmovimientodetalleNavigation)
                    .WithMany(p => p.MovimientoventanillacompCaja)
                    .HasForeignKey(d => d.Secuencialmovimientodetalle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MOVIMIENTOVENTANILLACOMP_C_R01");

                entity.HasOne(d => d.SecuencialventanillacompcajaNavigation)
                    .WithMany(p => p.MovimientoventanillacompCaja)
                    .HasForeignKey(d => d.Secuencialventanillacompcaja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MOVIMIENTOVENTANILLACOMP_C_R02");
            });

            modelBuilder.Entity<MovimientoventcompCajadet>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("MOVIMIENTOVENTCOMP_CAJADET", "FBS_CAJAS");

                entity.HasIndex(e => new { e.Secuencialmovventcompcaja, e.Secuencialventcompdenomefe })
                    .HasName("IX_MOVIMIENTOVENTCOMP_CAJADET");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.Saldo).HasColumnName("SALDO");

                entity.Property(e => e.Secuencialmovventcompcaja).HasColumnName("SECUENCIALMOVVENTCOMPCAJA");

                entity.Property(e => e.Secuencialventcompdenomefe).HasColumnName("SECUENCIALVENTCOMPDENOMEFE");

                entity.HasOne(d => d.SecuencialmovventcompcajaNavigation)
                    .WithMany(p => p.MovimientoventcompCajadet)
                    .HasForeignKey(d => d.Secuencialmovventcompcaja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MOVIMIENTOVENTCOMP_CAJADET_R01");

                entity.HasOne(d => d.SecuencialventcompdenomefeNavigation)
                    .WithMany(p => p.MovimientoventcompCajadet)
                    .HasForeignKey(d => d.Secuencialventcompdenomefe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MOVIMIENTOVENTCOMP_CAJADET_R02");
            });

            modelBuilder.Entity<Oficina>(entity =>
            {
                entity.HasKey(e => e.Secuencialdivision);

                entity.ToTable("OFICINA", "FBS_ORGANIZACIONES");

                entity.Property(e => e.Secuencialdivision)
                    .HasColumnName("SECUENCIALDIVISION")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cadenaconexionbasedatoslocal)
                    .IsRequired()
                    .HasColumnName("CADENACONEXIONBASEDATOSLOCAL")
                    .HasMaxLength(500);

                entity.Property(e => e.Ciudad)
                    .IsRequired()
                    .HasColumnName("CIUDAD")
                    .HasMaxLength(60);

                entity.Property(e => e.Codigoagenciacontrol)
                    .HasColumnName("CODIGOAGENCIACONTROL")
                    .HasMaxLength(8);

                entity.Property(e => e.Codigooficinacontrol)
                    .IsRequired()
                    .HasColumnName("CODIGOOFICINACONTROL")
                    .HasMaxLength(8);

                entity.Property(e => e.Codigooficinaseps)
                    .HasColumnName("CODIGOOFICINASEPS")
                    .HasMaxLength(8);

                entity.Property(e => e.Codigoregion)
                    .HasColumnName("CODIGOREGION")
                    .HasMaxLength(20);

                entity.Property(e => e.Esoperativa).HasColumnName("ESOPERATIVA");

                entity.Property(e => e.Estaactiva).HasColumnName("ESTAACTIVA");

                entity.Property(e => e.Fechacierrecontable)
                    .HasColumnName("FECHACIERRECONTABLE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Numerocontable).HasColumnName("NUMEROCONTABLE");

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Puertoswitch).HasColumnName("PUERTOSWITCH");

                entity.Property(e => e.Secuencialempresa).HasColumnName("SECUENCIALEMPRESA");

                entity.Property(e => e.Secuencialpersonaorganizacion).HasColumnName("SECUENCIALPERSONAORGANIZACION");

                entity.Property(e => e.Servidorimagenes)
                    .IsRequired()
                    .HasColumnName("SERVIDORIMAGENES")
                    .HasMaxLength(100);

                entity.Property(e => e.Servidorswitch)
                    .IsRequired()
                    .HasColumnName("SERVIDORSWITCH")
                    .HasMaxLength(500);

                entity.Property(e => e.Siglas)
                    .IsRequired()
                    .HasColumnName("SIGLAS")
                    .HasMaxLength(10);

                entity.HasOne(d => d.SecuencialdivisionNavigation)
                    .WithOne(p => p.Oficina)
                    .HasForeignKey<Oficina>(d => d.Secuencialdivision)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OFICINA_R01");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("PERSONA", "FBS_PERSONAS");

                entity.HasIndex(e => e.Identificacion)
                    .HasName("IX_PERSONA");

                entity.HasIndex(e => new { e.Secuencialtipoidentificacion, e.Identificacion })
                    .HasName("IX_01")
                    .IsUnique();

                entity.HasIndex(e => new { e.Identificacion, e.Nombreunido, e.Secuencial })
                    .HasName("_dta_index_PERSONA_14_1929318183__K1_2_3");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Codigopaisorigen)
                    .IsRequired()
                    .HasColumnName("CODIGOPAISORIGEN")
                    .HasMaxLength(20);

                entity.Property(e => e.Codigosociomigra)
                    .HasColumnName("CODIGOSOCIOMIGRA")
                    .HasMaxLength(255);

                entity.Property(e => e.Direcciondomicilio)
                    .IsRequired()
                    .HasColumnName("DIRECCIONDOMICILIO")
                    .HasMaxLength(500);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("EMAIL")
                    .HasMaxLength(70);

                entity.Property(e => e.Identificacion)
                    .IsRequired()
                    .HasColumnName("IDENTIFICACION")
                    .HasMaxLength(40);

                entity.Property(e => e.Identificacionmigra)
                    .HasColumnName("IDENTIFICACIONMIGRA")
                    .HasMaxLength(255);

                entity.Property(e => e.Nombreunido)
                    .IsRequired()
                    .HasColumnName("NOMBREUNIDO")
                    .HasMaxLength(200);

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Referenciadomiciliaria)
                    .IsRequired()
                    .HasColumnName("REFERENCIADOMICILIARIA")
                    .HasMaxLength(300);

                entity.Property(e => e.Secuencialdivactividadecon).HasColumnName("SECUENCIALDIVACTIVIDADECON");

                entity.Property(e => e.Secuencialdivpolresidencia).HasColumnName("SECUENCIALDIVPOLRESIDENCIA");

                entity.Property(e => e.Secuencialtipoidentificacion).HasColumnName("SECUENCIALTIPOIDENTIFICACION");

                entity.HasOne(d => d.SecuencialdivactividadeconNavigation)
                    .WithMany(p => p.PersonaSecuencialdivactividadeconNavigation)
                    .HasForeignKey(d => d.Secuencialdivactividadecon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PERSONA_R05");

                entity.HasOne(d => d.SecuencialdivpolresidenciaNavigation)
                    .WithMany(p => p.PersonaSecuencialdivpolresidenciaNavigation)
                    .HasForeignKey(d => d.Secuencialdivpolresidencia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PERSONA_R02");

                entity.HasOne(d => d.SecuencialtipoidentificacionNavigation)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.Secuencialtipoidentificacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PERSONA_R01");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("PRODUCTO", "FBS_NEGOCIOSFINANCIEROS");

                entity.Property(e => e.Codigo)
                    .HasColumnName("CODIGO")
                    .HasMaxLength(40)
                    .ValueGeneratedNever();

                entity.Property(e => e.Codigotipoproducto)
                    .IsRequired()
                    .HasColumnName("CODIGOTIPOPRODUCTO")
                    .HasMaxLength(30);

                entity.Property(e => e.Estaactivo).HasColumnName("ESTAACTIVO");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100);

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Secuencialmoneda).HasColumnName("SECUENCIALMONEDA");

                entity.Property(e => e.Siglas)
                    .IsRequired()
                    .HasColumnName("SIGLAS")
                    .HasMaxLength(6);
            });

            modelBuilder.Entity<Tipocuenta>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("TIPOCUENTA", "FBS_CAPTACIONESVISTA");

                entity.Property(e => e.Codigo)
                    .HasColumnName("CODIGO")
                    .HasMaxLength(40)
                    .ValueGeneratedNever();

                entity.Property(e => e.Aceptamulticuenta).HasColumnName("ACEPTAMULTICUENTA");

                entity.Property(e => e.Aceptasobregiroautomatico).HasColumnName("ACEPTASOBREGIROAUTOMATICO");

                entity.Property(e => e.Esoperativo).HasColumnName("ESOPERATIVO");

                entity.Property(e => e.Estaactivo).HasColumnName("ESTAACTIVO");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100);

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Provisionainteres).HasColumnName("PROVISIONAINTERES");

                entity.Property(e => e.Saldominimo).HasColumnName("SALDOMINIMO");

                entity.Property(e => e.Secuencialempresa).HasColumnName("SECUENCIALEMPRESA");

                entity.Property(e => e.Siglas)
                    .IsRequired()
                    .HasColumnName("SIGLAS")
                    .HasMaxLength(6);
            });

            modelBuilder.Entity<Tipoidentificacion>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("TIPOIDENTIFICACION", "FBS_PERSONAS");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasColumnName("CODIGO")
                    .HasMaxLength(20);

                entity.Property(e => e.Codigosbs)
                    .HasColumnName("CODIGOSBS")
                    .HasMaxLength(1);

                entity.Property(e => e.Estaactiva).HasColumnName("ESTAACTIVA");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100);

                entity.Property(e => e.Numerominimoreferenciasbancari).HasColumnName("NUMEROMINIMOREFERENCIASBANCARI");

                entity.Property(e => e.Numerominimoreferenciascomerci).HasColumnName("NUMEROMINIMOREFERENCIASCOMERCI");

                entity.Property(e => e.Numerominimoreferenciaspersona).HasColumnName("NUMEROMINIMOREFERENCIASPERSONA");

                entity.Property(e => e.Numerominimorepresentantes).HasColumnName("NUMEROMINIMOREPRESENTANTES");

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Parapersonanatural).HasColumnName("PARAPERSONANATURAL");
            });

            modelBuilder.Entity<Transaccion>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("TRANSACCION", "FBS_NEGOCIOSFINANCIEROS");

                entity.HasIndex(e => new { e.Codigo, e.Secuencialempresa })
                    .HasName("TRANSACCION_U01")
                    .IsUnique();

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasColumnName("CODIGO")
                    .HasMaxLength(20);

                entity.Property(e => e.Codigotipoproducto)
                    .IsRequired()
                    .HasColumnName("CODIGOTIPOPRODUCTO")
                    .HasMaxLength(30);

                entity.Property(e => e.Esdebito).HasColumnName("ESDEBITO");

                entity.Property(e => e.Esfacturable).HasColumnName("ESFACTURABLE");

                entity.Property(e => e.Esoperable).HasColumnName("ESOPERABLE");

                entity.Property(e => e.Estaactiva).HasColumnName("ESTAACTIVA");

                entity.Property(e => e.Esvisible).HasColumnName("ESVISIBLE");

                entity.Property(e => e.Facturarenlinea).HasColumnName("FACTURARENLINEA");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100);

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Requierealmacenarpapeleta).HasColumnName("REQUIEREALMACENARPAPELETA");

                entity.Property(e => e.Requiereconcepto).HasColumnName("REQUIERECONCEPTO");

                entity.Property(e => e.Secuencialempresa).HasColumnName("SECUENCIALEMPRESA");

                entity.Property(e => e.Siglas)
                    .IsRequired()
                    .HasColumnName("SIGLAS")
                    .HasMaxLength(6);

                entity.Property(e => e.Usuariodefineorigen).HasColumnName("USUARIODEFINEORIGEN");

                entity.Property(e => e.Verificahuella).HasColumnName("VERIFICAHUELLA");
            });

            modelBuilder.Entity<Transaccioncomponente>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("TRANSACCIONCOMPONENTE", "FBS_NEGOCIOSFINANCIEROS");

                entity.HasIndex(e => new { e.Secuencialtransaccion, e.Secuencialcomponente })
                    .HasName("IX_000186")
                    .IsUnique();

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Estaactiva).HasColumnName("ESTAACTIVA");

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Secuencialcomponente).HasColumnName("SECUENCIALCOMPONENTE");

                entity.Property(e => e.Secuencialtransaccion).HasColumnName("SECUENCIALTRANSACCION");

                entity.HasOne(d => d.SecuencialtransaccionNavigation)
                    .WithMany(p => p.Transaccioncomponente)
                    .HasForeignKey(d => d.Secuencialtransaccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TRANSACCIONCOMPONENTE_R01");
            });

            modelBuilder.Entity<Transaccionrangoaprobacion>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("TRANSACCIONRANGOAPROBACION", "FBS_NEGOCIOSFINANCIEROS");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Clientedefinecontrapartida).HasColumnName("CLIENTEDEFINECONTRAPARTIDA");

                entity.Property(e => e.Estaactivo).HasColumnName("ESTAACTIVO");

                entity.Property(e => e.Montofin).HasColumnName("MONTOFIN");

                entity.Property(e => e.Montoinicio).HasColumnName("MONTOINICIO");

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Secuencialtransaccion).HasColumnName("SECUENCIALTRANSACCION");

                entity.Property(e => e.Vistaimprimedocumento)
                    .IsRequired()
                    .HasColumnName("VISTAIMPRIMEDOCUMENTO")
                    .HasMaxLength(200);

                entity.HasOne(d => d.SecuencialtransaccionNavigation)
                    .WithMany(p => p.Transaccionrangoaprobacion)
                    .HasForeignKey(d => d.Secuencialtransaccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TRANSACCIONRANGOAPROBACION_R01");
            });

            modelBuilder.Entity<TransaccionVista>(entity =>
            {
                entity.HasKey(e => e.Secuencialtransaccion);

                entity.ToTable("TRANSACCION_VISTA", "FBS_NEGOCIOSFINANCIEROS");

                entity.Property(e => e.Secuencialtransaccion)
                    .HasColumnName("SECUENCIALTRANSACCION")
                    .ValueGeneratedNever();

                entity.Property(e => e.Enlote).HasColumnName("ENLOTE");

                entity.Property(e => e.Esaperturacuenta).HasColumnName("ESAPERTURACUENTA");

                entity.Property(e => e.Escierrecuenta).HasColumnName("ESCIERRECUENTA");

                entity.Property(e => e.Esconactivacion)
                    .IsRequired()
                    .HasColumnName("ESCONACTIVACION")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Esconinstitucionexterna).HasColumnName("ESCONINSTITUCIONEXTERNA");

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Paraacreditarprestamoautomatic).HasColumnName("PARAACREDITARPRESTAMOAUTOMATIC");

                entity.Property(e => e.Paraanexo2).HasColumnName("PARAANEXO2");

                entity.Property(e => e.Paraefectivizacioncheque).HasColumnName("PARAEFECTIVIZACIONCHEQUE");

                entity.Property(e => e.Paraprotestocheque).HasColumnName("PARAPROTESTOCHEQUE");

                entity.Property(e => e.Trabajaconcodigocuenta).HasColumnName("TRABAJACONCODIGOCUENTA");

                entity.HasOne(d => d.SecuencialtransaccionNavigation)
                    .WithOne(p => p.TransaccionVista)
                    .HasForeignKey<TransaccionVista>(d => d.Secuencialtransaccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TRANSACCION_VISTA_R01");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("USUARIO", "FBS_SEGURIDADES");

                entity.Property(e => e.Codigo)
                    .HasColumnName("CODIGO")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Estaactivo).HasColumnName("ESTAACTIVO");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100);

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Secuencialoficina).HasColumnName("SECUENCIALOFICINA");

                entity.HasOne(d => d.SecuencialoficinaNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.Secuencialoficina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("USUARIO_R01");
            });

            modelBuilder.Entity<UsuarioComplemento>(entity =>
            {
                entity.HasKey(e => e.Codigousuario);

                entity.ToTable("USUARIO_COMPLEMENTO", "FBS_SEGURIDADES");

                entity.Property(e => e.Codigousuario)
                    .HasColumnName("CODIGOUSUARIO")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Cambioclaveproximoingreso).HasColumnName("CAMBIOCLAVEPROXIMOINGRESO");

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasColumnName("CLAVE")
                    .HasMaxLength(1000);

                entity.Property(e => e.Emailinterno)
                    .IsRequired()
                    .HasColumnName("EMAILINTERNO")
                    .HasMaxLength(200);

                entity.Property(e => e.Esinterno).HasColumnName("ESINTERNO");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnName("FECHACREACION")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fechaultimocambioclave)
                    .HasColumnName("FECHAULTIMOCAMBIOCLAVE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Periodicidadcambioclave).HasColumnName("PERIODICIDADCAMBIOCLAVE");

                entity.Property(e => e.Puedeconsultarotrosusuarios).HasColumnName("PUEDECONSULTAROTROSUSUARIOS");

                entity.Property(e => e.Secuencialpersona).HasColumnName("SECUENCIALPERSONA");

                entity.HasOne(d => d.CodigousuarioNavigation)
                    .WithOne(p => p.UsuarioComplemento)
                    .HasForeignKey<UsuarioComplemento>(d => d.Codigousuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C0031868");

                entity.HasOne(d => d.SecuencialpersonaNavigation)
                    .WithMany(p => p.UsuarioComplemento)
                    .HasForeignKey(d => d.Secuencialpersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("USUARIO_COMPLEMENTO_R01");
            });

            modelBuilder.Entity<Ventanilla>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("VENTANILLA", "FBS_CAJAS");

                entity.HasIndex(e => new { e.Codigousuario, e.Secuencialoficina, e.Fecha })
                    .HasName("IX_00023")
                    .IsUnique();

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Abiertaautomaticamente).HasColumnName("ABIERTAAUTOMATICAMENTE");

                entity.Property(e => e.Codigousuario)
                    .IsRequired()
                    .HasColumnName("CODIGOUSUARIO")
                    .HasMaxLength(20);

                entity.Property(e => e.Estacerrada).HasColumnName("ESTACERRADA");

                entity.Property(e => e.Estacuadrada).HasColumnName("ESTACUADRADA");

                entity.Property(e => e.Fecha)
                    .HasColumnName("FECHA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Numerovecescuadrada).HasColumnName("NUMEROVECESCUADRADA");

                entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

                entity.Property(e => e.Secuencialoficina).HasColumnName("SECUENCIALOFICINA");

                entity.HasOne(d => d.CodigousuarioNavigation)
                    .WithMany(p => p.Ventanilla)
                    .HasForeignKey(d => d.Codigousuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VENTANILLA_R01");

                entity.HasOne(d => d.SecuencialoficinaNavigation)
                    .WithMany(p => p.Ventanilla)
                    .HasForeignKey(d => d.Secuencialoficina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VENTANILLA_R02");
            });

            modelBuilder.Entity<VentanillacomponenteCaja>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("VENTANILLACOMPONENTE_CAJA", "FBS_CAJAS");

                entity.HasIndex(e => e.Secuencialventanilla)
                    .HasName("IX_VENTANILLACOMPONENTE_CAJA");

                entity.HasIndex(e => new { e.Secuencialventanilla, e.Secuencialcomponentecaja, e.Secuencialmoneda })
                    .HasName("IX_00024")
                    .IsUnique();

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.Saldo).HasColumnName("SALDO");

                entity.Property(e => e.Secuencialcomponentecaja).HasColumnName("SECUENCIALCOMPONENTECAJA");

                entity.Property(e => e.Secuencialmoneda).HasColumnName("SECUENCIALMONEDA");

                entity.Property(e => e.Secuencialventanilla).HasColumnName("SECUENCIALVENTANILLA");

                entity.Property(e => e.Valorcuadre).HasColumnName("VALORCUADRE");

                entity.HasOne(d => d.SecuencialcomponentecajaNavigation)
                    .WithMany(p => p.VentanillacomponenteCaja)
                    .HasForeignKey(d => d.Secuencialcomponentecaja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VENTANILLACOMPONENTE_CAJA_R02");

                entity.HasOne(d => d.SecuencialventanillaNavigation)
                    .WithMany(p => p.VentanillacomponenteCaja)
                    .HasForeignKey(d => d.Secuencialventanilla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VENTANILLACOMPONENTE_CAJA_R01");
            });

            modelBuilder.Entity<Ventanillacomponentedenomnefe>(entity =>
            {
                entity.HasKey(e => e.Secuencial);

                entity.ToTable("VENTANILLACOMPONENTEDENOMNEFE", "FBS_CAJAS");

                entity.HasIndex(e => e.Secuencialventanillacompcaja)
                    .HasName("IX_VENTANILLACOMPONENTEDENOMNEFE");

                entity.Property(e => e.Secuencial).HasColumnName("SECUENCIAL");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.Denominacion).HasColumnName("DENOMINACION");

                entity.Property(e => e.Secuencialventanillacompcaja).HasColumnName("SECUENCIALVENTANILLACOMPCAJA");

                entity.HasOne(d => d.SecuencialventanillacompcajaNavigation)
                    .WithMany(p => p.Ventanillacomponentedenomnefe)
                    .HasForeignKey(d => d.Secuencialventanillacompcaja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VENTANILLACOMPONENTEDENOMN_R01");
            });

			modelBuilder.Entity<Transacciontipomovimiento>(entity =>
			{
				entity.HasKey(e => e.Secuencial);

				entity.ToTable("TRANSACCIONTIPOMOVIMIENTO", "FBS_NEGOCIOSFINANCIEROS");

				entity.Property(e => e.SecuencialTransaccion).HasColumnName("SECUENCIALTRANSACCION");
				entity.Property(e => e.Codigotipomovimiento).HasColumnName("CODIGOTIPOMOVIMIENTO");
				entity.Property(e => e.Estaactivo).HasColumnName("ESTAACTIVO");
				entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");

			});

			modelBuilder.Entity<Registrocontable>(entity =>
			{
				entity.HasKey(e => e.Secuencial);

				entity.ToTable("REGISTROCONTABLE", "FBS_REPOSITORIOCONTABLE");

				entity.Property(e => e.Valor).HasColumnName("VALOR");
				entity.Property(e => e.Esdebito).HasColumnName("ESDEBITO");
				entity.Property(e => e.Documento).HasColumnName("DOCUMENTO");
				entity.Property(e => e.Detalle).HasColumnName("DETALLE");
				entity.Property(e => e.Estacontabilizado).HasColumnName("ESTACONTABILIZADO");
				entity.Property(e => e.Secuencialcuentacontable).HasColumnName("SECUENCIALCUENTACONTABLE");
				entity.Property(e => e.Secuencialoficina).HasColumnName("SECUENCIALOFICINA");
				entity.Property(e => e.Secuencialperfilcontable).HasColumnName("SECUENCIALPERFILCONTABLE");
				entity.Property(e => e.Codigousuario).HasColumnName("CODIGOUSUARIO");
				entity.Property(e => e.Secuencialmoneda).HasColumnName("SECUENCIALMONEDA");
				entity.Property(e => e.Fechasistemaregistro).HasColumnName("FECHASISTEMAREGISTRO");
				entity.Property(e => e.Fechamaquinaregistro).HasColumnName("FECHAMAQUINAREGISTRO");
				entity.Property(e => e.Secuencialmovimientodetalle).HasColumnName("SECUENCIALMOVIMIENTODETALLE");
				entity.Property(e => e.Secuencialmovimientocontrol).HasColumnName("SECUENCIALMOVIMIENTOCONTROL");
				entity.Property(e => e.Estaactiva).HasColumnName("ESTAACTIVA");
				entity.Property(e => e.Generarcheque).HasColumnName("GENERARCHEQUE");
				entity.Property(e => e.Esreverso).HasColumnName("ESREVERSO");
				entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");
			});

			modelBuilder.Entity<ComponenteCuentaContable>(entity =>
			{
				entity.HasKey(e => e.Secuencialcomponente);
				entity.ToTable("COMPONENTE_CUENTACONTABLE", "FBS_NEGOCIOSFINANCIEROS");

				entity.Property(e => e.SecuencialCuentaContable).HasColumnName("SECUENCIALCUENTACONTABLE");
				entity.Property(e => e.EstaActivo).HasColumnName("ESTAACTIVO");
				entity.Property(e => e.NumeroVerificador).HasColumnName("NUMEROVERIFICADOR");
			});

			modelBuilder.Entity<Banco>(entity =>
			{
				entity.HasKey(e => e.Secuencial);
				entity.ToTable("BANCO", "FBS_GENERALES");

				entity.Property(e => e.Codigo).HasColumnName("CODIGO");
				entity.Property(e => e.Nombre).HasColumnName("NOMBRE");
				entity.Property(e => e.CodigoTipoBanco).HasColumnName("CODIGOTIPOBANCO");
				entity.Property(e => e.Estaactivo).HasColumnName("ESTAACTIVO");
				entity.Property(e => e.NumeroVerificador).HasColumnName("NUMEROVERIFICADOR");

			});

			modelBuilder.Entity<Cheque>(entity =>
			{
				entity.HasKey(e => e.Secuencial);
				entity.ToTable("CHEQUE", "FBS_REMESAS");

				entity.Property(e => e.CodigoCuentaCorriente).HasColumnName("CODIGOCUENTACORRIENTE");
				entity.Property(e => e.CodigoCheque).HasColumnName("CODIGOCHEQUE");
				entity.Property(e => e.SecuencialBancoEmisor).HasColumnName("SECUENCIALBANCOEMISOR");
				entity.Property(e => e.SecuencialMoneda).HasColumnName("SECUENCIALMONEDA");
				entity.Property(e => e.Valor).HasColumnName("VALOR");
				entity.Property(e => e.CodigoUsuario).HasColumnName("CODIGOUSUARIO");
				entity.Property(e => e.Estaenboveda).HasColumnName("ESTAENBOVEDA");
				entity.Property(e => e.FechaSistemaIngreso).HasColumnName("FECHASISTEMAINGRESO");
				entity.Property(e => e.FechaMaquinaIngreso).HasColumnName("FECHAMAQUINAINGRESO");
				entity.Property(e => e.CodigoEstadoCheque).HasColumnName("CODIGOESTADOCHEQUE");
				entity.Property(e => e.NumeroVerificador).HasColumnName("NUMEROVERIFICADOR");

			});
			//
			modelBuilder.Entity<ChequeMovimientoDetalle>(entity =>
			{
				entity.HasKey(e => e.Secuencial);
				entity.ToTable("CHEQUE_MOVIMIENTODETALLE", "FBS_REMESAS");

				entity.Property(e => e.SecuencialCheque).HasColumnName("SECUENCIALCHEQUE");
				entity.Property(e => e.SecuencialMovimientoDetalle).HasColumnName("SECUENCIALMOVIMIENTODETALLE");
			});

			modelBuilder.Entity<Transaccionmobile>(entity =>
			{
				entity.HasKey(e => e.Secuencial);
				entity.ToTable("TRANSACCIONMOBILE", "OR");

				entity.Property(e => e.CodigoUsuario).HasColumnName("CODIGOUSUARIO");
				entity.Property(e => e.NumeroCliente).HasColumnName("NUMEROCLIENTE");
				entity.Property(e => e.Fecha).HasColumnName("FECHA");
				entity.Property(e => e.Monto).HasColumnName("MONTO");
				entity.Property(e => e.Longitud).HasColumnName("LONGITUD");
				entity.Property(e => e.Latitud).HasColumnName("LATITUD");
			});

			//Empresafechacajero
			modelBuilder.Entity<Empresafechacajero>(entity =>
			{
				entity.HasKey(e => e.Secuencialempresa);
				entity.ToTable("EMPRESA_FECHACAJERO", "FBS_GENERALES");

				entity.Property(e => e.Fecha).HasColumnName("FECHA");
				entity.Property(e => e.Estaactiva).HasColumnName("ESTAACTIVA");
			});

			modelBuilder.Entity<Chequeefectivizacion>(entity =>
			{
				entity.HasKey(e => e.SecuencialCheque);
				entity.ToTable("CHEQUE_EFECTIVIZACION", "FBS_REMESAS");

				entity.Property(e => e.FechaSistema).HasColumnName("FECHASISTEMA");
				entity.Property(e => e.FechaMaquina).HasColumnName("FECHAMAQUINA");
				entity.Property(e => e.CodigoUsuario).HasColumnName("CODIGOUSUARIO");
				entity.Property(e => e.SecuencialOficina).HasColumnName("SECUENCIALOFICINA");
				entity.Property(e => e.Documento).HasColumnName("DOCUMENTO");
				entity.Property(e => e.EsManual).HasColumnName("ESMANUAL");
				entity.Property(e => e.EstuvoenTransito).HasColumnName("ESTUVOENTRANSITO");
			});

			modelBuilder.Entity<Ruta>(entity =>
			{
				entity.HasKey(e => e.Secuencial);
				entity.ToTable("RUTA", "FBS_REMESAS");

				entity.Property(e => e.Secuencialbancoemisor).HasColumnName("SECUENCIALBANCOEMISOR");
				entity.Property(e => e.Secuencialbancodeposito).HasColumnName("SECUENCIALBANCODEPOSITO");
				entity.Property(e => e.Diastransito).HasColumnName("DIASTRANSITO");
				entity.Property(e => e.Estaactivo).HasColumnName("ESTAACTIVO");
				entity.Property(e => e.Numeroverificador).HasColumnName("NUMEROVERIFICADOR");
			});

			modelBuilder.Entity<Calendario>(entity =>
			{
				entity.HasKey(e => e.Secuencial);
				entity.ToTable("CALENDARIO", "FBS_GENERALES");

				entity.Property(e => e.SecuencialEmpresa).HasColumnName("SECUENCIALEMPRESA");
				entity.Property(e => e.FechaSistema).HasColumnName("FECHASISTEMA");
				entity.Property(e => e.EstaCerrado).HasColumnName("ESTACERRADO");
				entity.Property(e => e.EsFeriado).HasColumnName("ESFERIADO");
				entity.Property(e => e.NumeroVerificador).HasColumnName("NUMEROVERIFICADOR");
				entity.Property(e => e.EsHabil).HasColumnName("ESHABIL");
			});

			modelBuilder.Entity<UsuarioHorarioIngreso>(entity =>
			{
				entity.HasKey(e => e.Secuencial);
				entity.ToTable("USUARIOHORARIOINGRESO", "FBS_SEGURIDADES");

				entity.Property(e => e.CodigoUsuario).HasColumnName("CODIGOUSUARIO");
				entity.Property(e => e.Dia).HasColumnName("DIA");
				entity.Property(e => e.HoraInicio).HasColumnName("HORAINICIO");
				entity.Property(e => e.MinutoInicio).HasColumnName("MINUTOINICIO");
				entity.Property(e => e.HorasValidez).HasColumnName("HORASVALIDEZ");
				entity.Property(e => e.MinutosValidez).HasColumnName("MINUTOSVALIDEZ");
				entity.Property(e => e.EstaActivo).HasColumnName("ESTAACTIVO");
				entity.Property(e => e.NumeroVerificador).HasColumnName("NUMEROVERIFICADOR");
			});

			
			modelBuilder.Entity<ComponenteVistaDisponible>(entity =>
			{
				entity.HasKey(e => e.SECUENCIALCOMPONENTEVISTA);
				entity.ToTable("COMPONENTE_VISTA_DISPONIBLE", "FBS_CAPTACIONESVISTA");

			});

			modelBuilder.Entity<ComponenteVistaRetencion>(entity =>
			{
				entity.HasKey(e => e.SECUENCIALCOMPONENTEVISTA);
				entity.ToTable("COMPONENTE_VISTA_RETENCION", "FBS_CAPTACIONESVISTA");

			});

			modelBuilder.Entity<Componente>(entity =>
			{
				entity.HasKey(e => e.Secuencial);
				entity.ToTable("COMPONENTE", "FBS_NEGOCIOSFINANCIEROS");

				entity.Property(e => e.Nombre).HasColumnName("NOMBRE");
				entity.Property(e => e.Siglas).HasColumnName("SIGLAS");
				entity.Property(e => e.SecuencialEmpresa).HasColumnName("SECUENCIALEMPRESA");
				entity.Property(e => e.EsOperativo).HasColumnName("ESOPERATIVO");
				entity.Property(e => e.MovimientoenBloque).HasColumnName("MOVIMIENTOENBLOQUE");
				entity.Property(e => e.CodigoTipoProducto).HasColumnName("CODIGOTIPOPRODUCTO");
				entity.Property(e => e.RequiereCuentaContable).HasColumnName("REQUIERECUENTACONTABLE");
				entity.Property(e => e.EstaActivo).HasColumnName("ESTAACTIVO");
				entity.Property(e => e.NumeroVerificador).HasColumnName("NUMEROVERIFICADOR");
			});

			modelBuilder.Entity<Cuentacontable>(entity =>
			{
				entity.HasKey(e => e.SecuecialDivision);
				entity.ToTable("CUENTACONTABLE", "FBS_CONTABILIDADES");

				entity.Property(e => e.EsDeudora).HasColumnName("ESDEUDORA");
				entity.Property(e => e.SecuencialEmpresa).HasColumnName("SECUENCIALEMPRESA");
				entity.Property(e => e.AfectaManualmente).HasColumnName("AFECTAMANUALMENTE");
				entity.Property(e => e.RequiereAuxiliar).HasColumnName("REQUIEREAUXILIAR");
				entity.Property(e => e.EsdeTotal).HasColumnName("ESDETOTAL");
				entity.Property(e => e.EstaActiva).HasColumnName("ESTAACTIVA");
				entity.Property(e => e.NumeroVerificador).HasColumnName("NUMEROVERIFICADOR");
				entity.Property(e => e.FechaCreacion).HasColumnName("fechaCreacion");
			});

			modelBuilder.Entity<Movimientocontrol>(entity =>
			{
				entity.HasKey(e => e.Secuencial);
				entity.ToTable("MOVIMIENTOCONTROL", "FBS_NEGOCIOSFINANCIEROS");

				entity.Property(e => e.EsDebito).HasColumnName("ESDEBITO");
				entity.Property(e => e.Valor).HasColumnName("VALOR");
				entity.Property(e => e.SecuencialOficina).HasColumnName("SECUENCIALOFICINA");
				entity.Property(e => e.SecuencialMoneda).HasColumnName("SECUENCIALMONEDA");
				entity.Property(e => e.Documento).HasColumnName("DOCUMENTO");
				entity.Property(e => e.Fecha).HasColumnName("FECHA");
				entity.Property(e => e.FechaMaquina).HasColumnName("FECHAMAQUINA");
				entity.Property(e => e.CodigoUsuario).HasColumnName("CODIGOUSUARIO");
				entity.Property(e => e.NumeroVerificador).HasColumnName("NUMEROVERIFICADOR");
			});

			//
			modelBuilder.Entity<MovimientocontrolTransfInt>(entity =>
			{
				entity.HasKey(e => e.SecuencialMovimientoControl);
				entity.ToTable("MOVIMIENTOCONTROL_TRANSFINT", "FBS_CONTABILIDADES");

				entity.Property(e => e.SecuencialCuentaContable).HasColumnName("SECUENCIALCUENTACONTABLE");
			});

			//Usuariorol

			modelBuilder.Entity<Usuariorol>(entity =>
			{
				entity.HasKey(e => e.Secuencial);
				entity.ToTable("USUARIOROL", "FBS_SEGURIDADES");

				entity.Property(e => e.CodigoUsuario).HasColumnName("CODIGOUSUARIO");
				entity.Property(e => e.CodigoRol).HasColumnName("CODIGOROL");
				entity.Property(e => e.EstaActivo).HasColumnName("ESTAACTIVO");
				entity.Property(e => e.NumeroVerificador).HasColumnName("NUMEROVERIFICADOR");
			});
		}
	}
}
