using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models.DB;

public partial class MonitoreopyaContext : DbContext
{
    public MonitoreopyaContext()
    {
    }

    public MonitoreopyaContext(DbContextOptions<MonitoreopyaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accione> Acciones { get; set; }

    public virtual DbSet<Agencium> Agencia { get; set; }

    public virtual DbSet<AlertaMonto> AlertaMontos { get; set; }

    public virtual DbSet<AlertaTransaccione> AlertaTransacciones { get; set; }

    public virtual DbSet<Alertum> Alerta { get; set; }

    public virtual DbSet<Autorizacione> Autorizaciones { get; set; }

    public virtual DbSet<ClienteBase> ClienteBases { get; set; }

    public virtual DbSet<ClienteBeneficiariosFrecuente> ClienteBeneficiariosFrecuentes { get; set; }

    public virtual DbSet<ClienteConyugue> ClienteConyugues { get; set; }

    public virtual DbSet<ClienteEconomiaOtrosIngreso> ClienteEconomiaOtrosIngresos { get; set; }

    public virtual DbSet<ClienteProducto> ClienteProductos { get; set; }

    public virtual DbSet<ClienteReferencia> ClienteReferencias { get; set; }

    public virtual DbSet<ClientesAgencium> ClientesAgencia { get; set; }

    public virtual DbSet<ClientesContato> ClientesContatos { get; set; }

    public virtual DbSet<ClientesEcoNegocioPropio> ClientesEcoNegocioPropios { get; set; }

    public virtual DbSet<ClientesEconomiaGeneral> ClientesEconomiaGenerals { get; set; }

    public virtual DbSet<ClientesEconomiaRelacionDependencium> ClientesEconomiaRelacionDependencia { get; set; }

    public virtual DbSet<DebidaDiligencium> DebidaDiligencia { get; set; }

    public virtual DbSet<ListaInterna> ListaInternas { get; set; }

    public virtual DbSet<ListaPep> ListaPeps { get; set; }

    public virtual DbSet<LogsAuditorium> LogsAuditoria { get; set; }

    public virtual DbSet<Menu> Menu { get; set; }

    public virtual DbSet<MstActividadEconomica> MstActividadEconomicas { get; set; }

    public virtual DbSet<MstAgencia> MstAgencias { get; set; }

    public virtual DbSet<MstCanalesDistribucion> MstCanalesDistribucions { get; set; }

    public virtual DbSet<MstNacionalidad> MstNacionalidads { get; set; }

    public virtual DbSet<MstOcupacion> MstOcupacions { get; set; }

    public virtual DbSet<MstPaise> MstPaises { get; set; }

    public virtual DbSet<MstParametrosRiesgo> MstParametrosRiesgos { get; set; }

    public virtual DbSet<MstProductosServicio> MstProductosServicios { get; set; }

    public virtual DbSet<MstRiesgoMonto> MstRiesgoMontos { get; set; }

    public virtual DbSet<MstTablasParametro> MstTablasParametros { get; set; }

    public virtual DbSet<MstZonaGeografica> MstZonaGeograficas { get; set; }

    public virtual DbSet<OperacionesBancaEnLinea> OperacionesBancaEnLineas { get; set; }

    public virtual DbSet<OperacionesDiario> OperacionesDiarios { get; set; }

    public virtual DbSet<Periodo> Periodos { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<RevokedToken> RevokedTokens { get; set; }

    public virtual DbSet<Riesgo> Riesgos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SubcripcionAlertum> SubcripcionAlerta { get; set; }

    public virtual DbSet<TempAcumuladosMe> TempAcumuladosMes { get; set; }

    public virtual DbSet<TempClienteProducto> TempClienteProductos { get; set; }

    public virtual DbSet<TipoMonedum> TipoMoneda { get; set; }

    public virtual DbSet<TipoOperacion> TipoOperacions { get; set; }

    public virtual DbSet<TransaccionesPosDiario> TransaccionesPosDiarios { get; set; }

    public virtual DbSet<TransferenciasInternacionale> TransferenciasInternacionales { get; set; }

    public virtual DbSet<UsuariosAutorizado> UsuariosAutorizados { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=159.65.98.130; DataBase=monitoreopya;User=sa_sysadmin20; Password=VpsDesa123$;  Trusted_Connection=False; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Acciones__3214EC07C9FB8DCB");

            entity.Property(e => e.Endpoint)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Menu).WithMany(p => p.Acciones)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Acciones_Menu");
        });

        modelBuilder.Entity<Agencium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Agencia_PK");

            entity.Property(e => e.Nombre)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AlertaMonto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AlertaMonto_PK");

            entity.ToTable("AlertaMonto");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(38, 0)");
            entity.Property(e => e.Codigo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Comentario)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.FechaConfiguracion).HasPrecision(0);
            entity.Property(e => e.IdTipoMoneda).HasColumnType("numeric(38, 0)");
            entity.Property(e => e.IdTipoOperacion).HasColumnType("numeric(38, 0)");
            entity.Property(e => e.Umbral).HasColumnType("numeric(38, 2)");

            entity.HasOne(d => d.IdAgenciaNavigation).WithMany(p => p.AlertaMontos)
                .HasForeignKey(d => d.IdAgencia)
                .HasConstraintName("AlertaMonto_Agencia_FK");

            entity.HasOne(d => d.IdTipoMonedaNavigation).WithMany(p => p.AlertaMontos)
                .HasForeignKey(d => d.IdTipoMoneda)
                .HasConstraintName("AlertaMonto_TipoMoneda_FK");

            entity.HasOne(d => d.IdTipoOperacionNavigation).WithMany(p => p.AlertaMontos)
                .HasForeignKey(d => d.IdTipoOperacion)
                .HasConstraintName("AlertaMonto_TipoOperacion_FK_1");
        });

        modelBuilder.Entity<AlertaTransaccione>(entity =>
        {
            entity.HasKey(e => e.IdTransaccion).HasName("PK__AlertaTr__334B1F7743F07A4D");

            entity.Property(e => e.Acciones).HasMaxLength(255);
            entity.Property(e => e.Agencia).HasMaxLength(255);
            entity.Property(e => e.AnalistaAsignado).HasMaxLength(255);
            entity.Property(e => e.ArchivoAdjunto).HasMaxLength(500);
            entity.Property(e => e.Comentarios).HasMaxLength(255);
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.DocumentoIdentificacion).HasMaxLength(50);
            entity.Property(e => e.EstatusAlerta).HasMaxLength(100);
            entity.Property(e => e.FechaAnalisis).HasColumnType("datetime");
            entity.Property(e => e.FechaAsignacion).HasColumnType("datetime");
            entity.Property(e => e.FechaAtencion).HasColumnType("datetime");
            entity.Property(e => e.FechaGeneracion).HasColumnType("datetime");
            entity.Property(e => e.FechaSeguimiento).HasColumnType("datetime");
            entity.Property(e => e.MontoOperacion).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NombreAlerta).HasMaxLength(255);
            entity.Property(e => e.NombreCliente).HasMaxLength(255);
            entity.Property(e => e.PersonaReporta).HasMaxLength(255);
            entity.Property(e => e.TipoAlerta).HasMaxLength(100);
            entity.Property(e => e.TipoMoneda).HasMaxLength(50);
            entity.Property(e => e.TipoOperacion).HasMaxLength(100);
        });

        modelBuilder.Entity<Alertum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Alerta_PK");

            entity.Property(e => e.Codigo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Autorizacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Autoriza__3214EC07B1C3C824");

            entity.HasOne(d => d.Accion).WithMany(p => p.Autorizaciones)
                .HasForeignKey(d => d.AccionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Autorizac__Accio__52593CB8");

            entity.HasOne(d => d.Rol).WithMany(p => p.Autorizaciones)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Autorizac__RolId__5165187F");
        });

        modelBuilder.Entity<ClienteBase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClienteB__3213E83F62A7C9E7");

            entity.ToTable("ClienteBase");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActividadEconomica)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("actividadEconomica");
            entity.Property(e => e.CargaFamiliar).HasColumnName("cargaFamiliar");
            entity.Property(e => e.CategoriaEspecial)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("categoriaEspecial");
            entity.Property(e => e.EsPep).HasColumnName("esPEP");
            entity.Property(e => e.FechaConstitucion).HasColumnName("fechaConstitucion");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fechaNacimiento");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
            entity.Property(e => e.Nacionalidad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nacionalidad");
            entity.Property(e => e.NombreCompletoPrimerApellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreCompleto_primerApellido");
            entity.Property(e => e.NombreCompletoPrimerNombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreCompleto_primerNombre");
            entity.Property(e => e.NombreCompletoSegundoApellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreCompleto_segundoApellido");
            entity.Property(e => e.NombreCompletoSegundoNombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreCompleto_segundoNombre");
            entity.Property(e => e.Oficio)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("oficio");
            entity.Property(e => e.Profesion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("profesion");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("razonSocial");
            entity.Property(e => e.RelacionPep)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("relacionPEP");
            entity.Property(e => e.Rif)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("rif");
            entity.Property(e => e.TipoPersona)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipoPersona");
            entity.Property(e => e.Vivienda)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("vivienda");
        });

        modelBuilder.Entity<ClienteBeneficiariosFrecuente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClienteB__3213E83F2C71A6DA");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BancoBeneficiario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("bancoBeneficiario");
            entity.Property(e => e.CedulaBeneficiario)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("cedulaBeneficiario");
            entity.Property(e => e.FechaUltimaTransaccion)
                .HasColumnType("datetime")
                .HasColumnName("fechaUltimaTransaccion");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
            entity.Property(e => e.IdentificadorBeneficiario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("identificadorBeneficiario");
            entity.Property(e => e.NombreBeneficiario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreBeneficiario");
            entity.Property(e => e.NumeroCuentaBeneficiario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numeroCuentaBeneficiario");
            entity.Property(e => e.RifBeneficiario)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("rifBeneficiario");
            entity.Property(e => e.TipoTransaccion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoTransaccion");
        });

        modelBuilder.Entity<ClienteConyugue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClienteC__3213E83F5005A2C9");

            entity.ToTable("ClienteConyugue");

            entity.HasIndex(e => e.IdCliente, "UQ__ClienteC__885457EF942426FB").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConyugeIngresos)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("conyuge_ingresos");
            entity.Property(e => e.ConyugeNombreCompleto)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("conyuge_nombreCompleto");
            entity.Property(e => e.ConyugeOcupacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("conyuge_ocupacion");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
        });

        modelBuilder.Entity<ClienteEconomiaOtrosIngreso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClienteE__3213E83F4D1EABDB");

            entity.HasIndex(e => new { e.IdCliente, e.OtroIngresoFuente }, "UQ_ClienteEconomiaOtrosIngresos").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
            entity.Property(e => e.OtroIngresoFrecuencia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("otroIngreso_frecuencia");
            entity.Property(e => e.OtroIngresoFuente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("otroIngreso_fuente");
            entity.Property(e => e.OtroIngresoMonto)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("otroIngreso_monto");
        });

        modelBuilder.Entity<ClienteProducto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClienteP__3213E83F164E43F8");

            entity.HasIndex(e => new { e.IdCliente, e.NumeroCuenta }, "UQ_ClienteCuenta").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaApertura).HasColumnName("fechaApertura");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
            entity.Property(e => e.MontoUmbral)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("montoUmbral");
            entity.Property(e => e.NumeroCuenta)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numeroCuenta");
            entity.Property(e => e.OficinaApertura)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("oficinaApertura");
            entity.Property(e => e.ProductoDestinoFondos)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("producto_destinoFondos");
            entity.Property(e => e.ProductoMoneda)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("producto_moneda");
            entity.Property(e => e.ProductoMontoPromedio)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("producto_montoPromedio");
            entity.Property(e => e.ProductoNombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("producto_nombre");
            entity.Property(e => e.ProductoOrigenFondos)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("producto_origenFondos");
            entity.Property(e => e.ProductoTransaccionesPromedio).HasColumnName("producto_transaccionesPromedio");
            entity.Property(e => e.ProductoUso)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("producto_uso");
        });

        modelBuilder.Entity<ClienteReferencia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClienteR__3213E83FD91035CA");

            entity.HasIndex(e => new { e.IdCliente, e.TipoReferencia, e.NombreReferencia }, "UQ_ClienteReferencias").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContactoReferencia)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("contactoReferencia");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
            entity.Property(e => e.NombreReferencia)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreReferencia");
            entity.Property(e => e.TipoReferencia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoReferencia");
        });

        modelBuilder.Entity<ClientesAgencium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clientes__3213E83F6C2425D8");

            entity.HasIndex(e => e.IdCliente, "UQ__Clientes__885457EF0D19ABF0").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AgenciaCodigo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("agencia_codigo");
            entity.Property(e => e.AgenciaNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("agencia_nombre");
            entity.Property(e => e.EjecutivoNegociosNombreCompleto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ejecutivoNegocios_nombreCompleto");
            entity.Property(e => e.GerenteNombreCompleto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("gerente_nombreCompleto");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
        });

        modelBuilder.Entity<ClientesContato>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clientes__3213E83F6C1FD015");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("correoElectronico");
            entity.Property(e => e.DireccionApartamento)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("direccion_apartamento");
            entity.Property(e => e.DireccionCalle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("direccion_calle");
            entity.Property(e => e.DireccionCodigoPostal)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("direccion_codigoPostal");
            entity.Property(e => e.DireccionEstado)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("direccion_estado");
            entity.Property(e => e.DireccionMunicipio)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("direccion_municipio");
            entity.Property(e => e.DireccionNumero)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("direccion_numero");
            entity.Property(e => e.DireccionPais)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("direccion_pais");
            entity.Property(e => e.DireccionUrbanizacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("direccion_urbanizacion");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
            entity.Property(e => e.TelefonoCodigoPais)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("telefono_codigoPais");
            entity.Property(e => e.TelefonoNumero)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono_numero");
            entity.Property(e => e.TipoContacto)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoContacto");
            entity.Property(e => e.TipoDireccion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoDireccion");
            entity.Property(e => e.TipoTelefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipoTelefono");
        });

        modelBuilder.Entity<ClientesEcoNegocioPropio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clientes__3213E83F2647EC5F");

            entity.ToTable("ClientesEcoNegocioPropio");

            entity.HasIndex(e => new { e.IdCliente, e.NegocioNombreEmpresa }, "UQ_clientes_eco_negocio_propio").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
            entity.Property(e => e.NegocioCargo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("negocio_cargo");
            entity.Property(e => e.NegocioContactoCorreo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("negocio_contacto_correo");
            entity.Property(e => e.NegocioContactoTelefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("negocio_contacto_telefono");
            entity.Property(e => e.NegocioIngreso)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("negocio_ingreso");
            entity.Property(e => e.NegocioNombreEmpresa)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("negocio_nombreEmpresa");
            entity.Property(e => e.NegocioPrincipalCliente)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("negocio_principalCliente");
            entity.Property(e => e.NegocioPrincipalProveedor)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("negocio_principalProveedor");
        });

        modelBuilder.Entity<ClientesEconomiaGeneral>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clientes__3213E83FCD59C5AE");

            entity.ToTable("ClientesEconomiaGeneral");

            entity.HasIndex(e => e.IdCliente, "UQ__Clientes__885457EF6DCE1B28").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActividadEconomicaEspecifica)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("actividadEconomica_especifica");
            entity.Property(e => e.ActividadEconomicaGeneral)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("actividadEconomica_general");
            entity.Property(e => e.FuenteIngresos)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("fuenteIngresos");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
        });

        modelBuilder.Entity<ClientesEconomiaRelacionDependencium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clientes__3213E83F10CF57BD");

            entity.HasIndex(e => e.IdCliente, "UQ__Clientes__885457EF98AD4E42").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmpleoContactoCorreo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("empleo_contacto_correo");
            entity.Property(e => e.EmpleoContactoTelefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("empleo_contacto_telefono");
            entity.Property(e => e.EmpleoEmpresa)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("empleo_empresa");
            entity.Property(e => e.EmpleoFechaIngreso).HasColumnName("empleo_fechaIngreso");
            entity.Property(e => e.EmpleoSalario)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("empleo_salario");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
        });

        modelBuilder.Entity<DebidaDiligencium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DebidaDiligencia_PK");

            entity.Property(e => e.FechaDebidaDiligencia).HasPrecision(0);
            entity.Property(e => e.IdentificacionCliente)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Observaciones)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.TipoDebidaDiligencia)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ListaInterna>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ListaInt__3214EC0729815153");

            entity.ToTable("ListaInterna");

            entity.Property(e => e.Motivo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NombRazonSocial)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Nomb_Razon_Social");
            entity.Property(e => e.NumDocumento).HasColumnName("Num_Documento");
        });

        modelBuilder.Entity<ListaPep>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ListaPEP__3214EC07B638E8AE");

            entity.ToTable("ListaPEP");

            entity.Property(e => e.Cargo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EnteAdscripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Ente_Adscripcion");
            entity.Property(e => e.NombApellido)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Nomb_Apellido");
            entity.Property(e => e.NumDocumento).HasColumnName("Num_Documento");
            entity.Property(e => e.Pais)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TipPersonaExpuesta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tip_Persona_Expuesta");
        });

        modelBuilder.Entity<LogsAuditorium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LogsAudi__3214EC07B5D2D9A0");

            entity.Property(e => e.BdTabla)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("BD_tabla");
            entity.Property(e => e.DetalleRegistro).HasColumnName("Detalle_registro");
            entity.Property(e => e.FechaConexion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_conexion");
            entity.Property(e => e.FechaDesconexion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_desconexion");
            entity.Property(e => e.FechaIngreso)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_ingreso");
            entity.Property(e => e.IdOperacion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Id_operacion");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Id_usuario");
            entity.Property(e => e.IntentosExitosos)
                .HasDefaultValue(0)
                .HasColumnName("Intentos_exitosos");
            entity.Property(e => e.IntentosFallidos)
                .HasDefaultValue(0)
                .HasColumnName("Intentos_fallidos");
            entity.Property(e => e.IpDispositivo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Ip_dispositivo");
            entity.Property(e => e.Menu)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Modulo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Opciones)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Submenu)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TpEvento)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Tp_Evento");
            entity.Property(e => e.TpUsuario)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Tp_Usuario");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3213E83F30AFC6CB");

            entity.ToTable("Menu", tb =>
                {
                    tb.HasTrigger("TR_Menu_UpdatedAt");
                    tb.HasTrigger("trg_Menu_CascadeDelete");
                });

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("icon");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsSeparator).HasColumnName("is_separator");
            entity.Property(e => e.Label)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("label");
            entity.Property(e => e.OrderIndex).HasColumnName("order_index");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.RouteLink)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("route_link");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__Menu__parent_id__318258D2");
        });

        modelBuilder.Entity<MstActividadEconomica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MstActiv__3214EC0705A4C667");

            entity.ToTable("MstActividadEconomica");

            entity.Property(e => e.ActividadEconomicaEspecifica)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Apnfd).HasColumnName("APNFD");
            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NivelRiesgo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MstAgencia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MstAgenc__3214EC07E9B8847B");

            entity.Property(e => e.Agencia)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Municipio)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NombreGerente)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OfcFronteriza).HasColumnName("ofcFronteriza");
        });

        modelBuilder.Entity<MstCanalesDistribucion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MstCanal__3214EC077AF34157");

            entity.ToTable("MstCanalesDistribucion");

            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Denominacion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NivelRiesgo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MstNacionalidad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MstNacio__3214EC072370272A");

            entity.ToTable("MstNacionalidad");

            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nacionalidad)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NivelRiesgo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MstOcupacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MstOcupa__3214EC0756298AAE");

            entity.ToTable("MstOcupacion");

            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NivelRiesgo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProfesionOcupacion)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MstPaise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MstPaise__3214EC07A9EF236E");

            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NivelRiesgo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pais)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MstParametrosRiesgo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MstParam__3214EC0782216893");

            entity.ToTable("MstParametrosRiesgo");

            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TipoParametroRiesgo)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdMtsParametrosRiegosNavigation).WithMany(p => p.InverseIdMtsParametrosRiegosNavigation)
                .HasForeignKey(d => d.IdMtsParametrosRiegos)
                .HasConstraintName("MstParametrosRiesgo_MstParametrosRiesgo_FK");
        });

        modelBuilder.Entity<MstProductosServicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MstProdu__3214EC076E2E6341");

            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Denominacion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaAutorizacion).HasColumnType("datetime");
            entity.Property(e => e.NivelRiesgo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductosServicios)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MstRiesgoMonto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MstRiesg__3214EC0782E9990F");

            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Desde).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Hasta).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NivelRiesgo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoMoneda)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MstTablasParametro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MstTabla__3214EC079233758A");

            entity.Property(e => e.NombreParametro)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TablaParametro)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MstZonaGeografica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MstZonaG__3214EC07F759BC59");

            entity.ToTable("MstZonaGeografica");

            entity.Property(e => e.Capital)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NivelRiesgo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RiesgoInherente)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OperacionesBancaEnLinea>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Operacio__3213E83F96BC9C74");

            entity.ToTable("OperacionesBancaEnLinea");

            entity.HasIndex(e => e.IdOperacion, "UQ__Operacio__E7EB6989370CA28A").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BancoDestino)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("bancoDestino");
            entity.Property(e => e.BancoOrigen)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("bancoOrigen");
            entity.Property(e => e.CanalOperacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("canalOperacion");
            entity.Property(e => e.CodOperacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codOperacion");
            entity.Property(e => e.CuentaDestino)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cuentaDestino");
            entity.Property(e => e.CuentaOrigen)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cuentaOrigen");
            entity.Property(e => e.FechaHoraOperacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaHoraOperacion");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
            entity.Property(e => e.IdOperacion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idOperacion");
            entity.Property(e => e.IdentificacionBeneficiario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("identificacionBeneficiario");
            entity.Property(e => e.IdentificacionOrdenante)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("identificacionOrdenante");
            entity.Property(e => e.IpOrdenante)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("ipOrdenante");
            entity.Property(e => e.MonedaOperacion)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("monedaOperacion");
            entity.Property(e => e.MontoOperacion)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("montoOperacion");
            entity.Property(e => e.MotivoOperacion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("motivoOperacion");
            entity.Property(e => e.NombreBeneficiario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreBeneficiario");
            entity.Property(e => e.NombreOrdenante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreOrdenante");
            entity.Property(e => e.TipoOperacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoOperacion");
        });

        modelBuilder.Entity<OperacionesDiario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Operacio__3213E83F898050BD");

            entity.ToTable("OperacionesDiario");

            entity.HasIndex(e => e.IdTransaccion, "UQ__Operacio__5B8761F1A239DA01").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodigoOficina)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codigoOficina");
            entity.Property(e => e.DireccionOficina)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("direccionOficina");
            entity.Property(e => e.FechaHoraTransaccion)
                .HasColumnType("datetime")
                .HasColumnName("fechaHoraTransaccion");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
            entity.Property(e => e.IdTransaccion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idTransaccion");
            entity.Property(e => e.LatitudOficina)
                .HasColumnType("decimal(18, 7)")
                .HasColumnName("latitudOficina");
            entity.Property(e => e.LongitudOficina)
                .HasColumnType("decimal(18, 7)")
                .HasColumnName("longitudOficina");
            entity.Property(e => e.MonedaTransaccion)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("monedaTransaccion");
            entity.Property(e => e.MontoTransaccion)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("montoTransaccion");
            entity.Property(e => e.NombreOficina)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreOficina");
            entity.Property(e => e.TipoOperacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoOperacion");
        });

        modelBuilder.Entity<Periodo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Periodos_PK");

            entity.Property(e => e.Estatus)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaCierre).HasPrecision(0);
            entity.Property(e => e.FechaCreacion).HasPrecision(0);
            entity.Property(e => e.FechaFin).HasPrecision(0);
            entity.Property(e => e.FechaInicio).HasPrecision(0);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Periodo1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Periodo");
            entity.Property(e => e.TipoPeriodo)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC0711DFAE00");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Expiration).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(500);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_RefreshTokens_Usuarios");
        });

        modelBuilder.Entity<RevokedToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RevokedT__3214EC078D2A0221");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.RevokedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.RevokedTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_RevokedTokens_Usuarios");
        });

        modelBuilder.Entity<Riesgo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Riesgo_PK");

            entity.ToTable("Riesgo");

            entity.Property(e => e.Accion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Amenaza)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Causa)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Consecuencia)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion).HasPrecision(0);
            entity.Property(e => e.FkAutomatizacion).HasColumnName("FK_Automatizacion");
            entity.Property(e => e.FkFactorRiesgo).HasColumnName("FK_FactorRiesgo");
            entity.Property(e => e.FkFrecuencia).HasColumnName("FK_Frecuencia");
            entity.Property(e => e.FkImpacto).HasColumnName("FK_Impacto");
            entity.Property(e => e.FkPeriodo).HasColumnName("FK_Periodo");
            entity.Property(e => e.FkProbabilidad).HasColumnName("FK_Probabilidad");
            entity.Property(e => e.FkSubFactorRiesgo).HasColumnName("FK_SubFactorRiesgo");
            entity.Property(e => e.FkTiempoEjecucion).HasColumnName("FK_TiempoEjecucion");
            entity.Property(e => e.FkTipoRiesgo).HasColumnName("FK_TipoRiesgo");
            entity.Property(e => e.FkTratamientoRiesgo).HasColumnName("FK_TratamientoRiesgo");
            entity.Property(e => e.FktipoRiesgoR).HasColumnName("FKTIpoRiesgoR");
            entity.Property(e => e.ImpactoValor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProbabilidadValor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Responsable)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RiesgoInherente)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RiesgoResidualValor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Severidad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SeveridadValor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ValoresControl)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Vulnerabilidad)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.FkAutomatizacionNavigation).WithMany(p => p.RiesgoFkAutomatizacionNavigations)
                .HasForeignKey(d => d.FkAutomatizacion)
                .HasConstraintName("Riesgo_MstParametrosRiesgo_FK_3");

            entity.HasOne(d => d.FkFactorRiesgoNavigation).WithMany(p => p.RiesgoFkFactorRiesgoNavigations)
                .HasForeignKey(d => d.FkFactorRiesgo)
                .HasConstraintName("Riesgo_MstParametrosRiesgo_FK_7");

            entity.HasOne(d => d.FkFrecuenciaNavigation).WithMany(p => p.RiesgoFkFrecuenciaNavigations)
                .HasForeignKey(d => d.FkFrecuencia)
                .HasConstraintName("Riesgo_MstParametrosRiesgo_FK_4");

            entity.HasOne(d => d.FkImpactoNavigation).WithMany(p => p.RiesgoFkImpactoNavigations)
                .HasForeignKey(d => d.FkImpacto)
                .HasConstraintName("Riesgo_MstParametrosRiesgo_FK_1");

            entity.HasOne(d => d.FkPeriodoNavigation).WithMany(p => p.Riesgos)
                .HasForeignKey(d => d.FkPeriodo)
                .HasConstraintName("Riesgo_Periodos_FK");

            entity.HasOne(d => d.FkProbabilidadNavigation).WithMany(p => p.RiesgoFkProbabilidadNavigations)
                .HasForeignKey(d => d.FkProbabilidad)
                .HasConstraintName("Riesgo_MstParametrosRiesgo_FK");

            entity.HasOne(d => d.FkSubFactorRiesgoNavigation).WithMany(p => p.RiesgoFkSubFactorRiesgoNavigations)
                .HasForeignKey(d => d.FkSubFactorRiesgo)
                .HasConstraintName("Riesgo_MstParametrosRiesgo_FK_8");

            entity.HasOne(d => d.FkTipoRiesgoNavigation).WithMany(p => p.RiesgoFkTipoRiesgoNavigations)
                .HasForeignKey(d => d.FkTipoRiesgo)
                .HasConstraintName("Riesgo_MstParametrosRiesgo_FK_2");

            entity.HasOne(d => d.FktipoRiesgoRNavigation).WithMany(p => p.RiesgoFktipoRiesgoRNavigations)
                .HasForeignKey(d => d.FktipoRiesgoR)
                .HasConstraintName("Riesgo_MstParametrosRiesgo_FK_9");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC075452682C");

            entity.HasIndex(e => e.Nombre, "UQ__Roles__75E3EFCF50D33F07").IsUnique();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Estatus).HasDefaultValue((byte)1);
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SubcripcionAlertum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SubcripcionAlerta_PK");

            entity.Property(e => e.Comentario).IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.FechaActualizacion).HasPrecision(0);
            entity.Property(e => e.FechaCreacion).HasPrecision(0);
            entity.Property(e => e.Nombre)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.FkAlertaNavigation).WithMany(p => p.SubcripcionAlerta)
                .HasForeignKey(d => d.FkAlerta)
                .HasConstraintName("SubcripcionAlerta_Alerta_FK");
        });

        modelBuilder.Entity<TempAcumuladosMe>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
            entity.Property(e => e.Mes).HasMaxLength(5);
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.NumeroCuenta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numeroCuenta");
            entity.Property(e => e.TransaccionesNoFrecuentes).HasColumnName("transacciones_no_frecuentes");
        });

        modelBuilder.Entity<TempClienteProducto>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TempClienteProducto");

            entity.Property(e => e.Codigo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("codigo");
            entity.Property(e => e.FechaApertura).HasColumnName("fecha_apertura");
            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
            entity.Property(e => e.MontoPromedio)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto_promedio");
            entity.Property(e => e.NumeroCuenta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numeroCuenta");
            entity.Property(e => e.TipoProducto)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo_producto");
            entity.Property(e => e.TransaccionesPromedio)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("transacciones_promedio");
            entity.Property(e => e.UltimoMov)
                .HasColumnType("datetime")
                .HasColumnName("ultimo_mov");
            entity.Property(e => e.Umbral)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("umbral");
        });

        modelBuilder.Entity<TipoMonedum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TipoMoneda_PK");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(38, 0)");
            entity.Property(e => e.Codigo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoOperacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TipoOperacion_PK");

            entity.ToTable("TipoOperacion");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(38, 0)");
            entity.Property(e => e.Codigo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TransaccionesPosDiario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transacc__3213E83F7EDCCC5B");

            entity.ToTable("TransaccionesPosDiario");

            entity.HasIndex(e => e.IdTransaccionPos, "UQ__Transacc__46643E76482565F5").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaComercio)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("categoriaComercio");
            entity.Property(e => e.CodigoComercio)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codigoComercio");
            entity.Property(e => e.DireccionComercio)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("direccionComercio");
            entity.Property(e => e.FechaHoraTransaccion)
                .HasColumnType("datetime")
                .HasColumnName("fechaHoraTransaccion");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
            entity.Property(e => e.IdTransaccionPos)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idTransaccionPOS");
            entity.Property(e => e.LatitudComercio)
                .HasColumnType("decimal(10, 7)")
                .HasColumnName("latitudComercio");
            entity.Property(e => e.LongitudComercio)
                .HasColumnType("decimal(10, 7)")
                .HasColumnName("longitudComercio");
            entity.Property(e => e.MontoTransaccion)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("montoTransaccion");
            entity.Property(e => e.NombreComercio)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreComercio");
        });

        modelBuilder.Entity<TransferenciasInternacionale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transfer__3213E83F2AC6FD19");

            entity.HasIndex(e => e.IdTransferencia, "UQ__Transfer__296B0E2A8A3400CD").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaHoraTransaccion)
                .HasColumnType("datetime")
                .HasColumnName("fechaHoraTransaccion");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idCliente");
            entity.Property(e => e.IdTransferencia)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("idTransferencia");
            entity.Property(e => e.IdentificacionBeneficiario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("identificacionBeneficiario");
            entity.Property(e => e.IdentificacionOrdenante)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("identificacionOrdenante");
            entity.Property(e => e.InstitucionBeneficiario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("institucionBeneficiario");
            entity.Property(e => e.InstitucionOrdenante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("institucionOrdenante");
            entity.Property(e => e.IpBeneficiarioTransaccion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("ipBeneficiarioTransaccion");
            entity.Property(e => e.IpOrdenanteTransaccion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("ipOrdenanteTransaccion");
            entity.Property(e => e.MonedaTransferencia)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("monedaTransferencia");
            entity.Property(e => e.MontoTransferencia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("montoTransferencia");
            entity.Property(e => e.MotivoTransferencia)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("motivoTransferencia");
            entity.Property(e => e.NombreBeneficiario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreBeneficiario");
            entity.Property(e => e.NombreOrdenante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreOrdenante");
            entity.Property(e => e.NumeroCuentaBeneficiario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numeroCuentaBeneficiario");
            entity.Property(e => e.PaisBeneficiario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("paisBeneficiario");
            entity.Property(e => e.PaisOrdenante)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("paisOrdenante");
            entity.Property(e => e.SwiftInstitucionBeneficiario)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("swiftInstitucionBeneficiario");
            entity.Property(e => e.SwiftInstitucionOrdenante)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("swiftInstitucionOrdenante");
            entity.Property(e => e.TipoTransferencia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoTransferencia");
        });

        modelBuilder.Entity<UsuariosAutorizado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC07D29DC03B");

            entity.HasIndex(e => new { e.UsuarioAd, e.RolId }, "UQ__Usuarios__24AE0A88F2DD6657").IsUnique();

            entity.Property(e => e.Estatus).HasDefaultValue((byte)1);
            entity.Property(e => e.RolId).HasDefaultValue(1);
            entity.Property(e => e.UsuarioAd)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("UsuarioAD");

            entity.HasOne(d => d.Rol).WithMany(p => p.UsuariosAutorizados)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuariosA__RolId__4AB81AF0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
