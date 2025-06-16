using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class AlertaTransaccione
{
    public int IdTransaccion { get; set; }

    public string? NombreCliente { get; set; }

    public string? DocumentoIdentificacion { get; set; }

    public string? TipoOperacion { get; set; }

    public string? TipoMoneda { get; set; }

    public decimal? MontoOperacion { get; set; }

    public string? NombreAlerta { get; set; }

    public string? TipoAlerta { get; set; }

    public string? PersonaReporta { get; set; }

    public string? Agencia { get; set; }

    public string? AnalistaAsignado { get; set; }

    public DateTime? FechaGeneracion { get; set; }

    public DateTime? FechaAsignacion { get; set; }

    public DateTime? FechaAnalisis { get; set; }

    public DateTime? FechaAtencion { get; set; }

    public DateTime? FechaSeguimiento { get; set; }

    public string? EstatusAlerta { get; set; }

    public string? ArchivoAdjunto { get; set; }

    public int? EstatusEjecucion { get; set; }

    public string? Descripcion { get; set; }

    public string? Acciones { get; set; }

    public string? Comentarios { get; set; }
}
