using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class OperacionesBancaEnLinea
{
    public int Id { get; set; }

    public string IdOperacion { get; set; } = null!;

    public string IdCliente { get; set; } = null!;

    public DateTime FechaHoraOperacion { get; set; }

    public string CodOperacion { get; set; } = null!;

    public string TipoOperacion { get; set; } = null!;

    public decimal MontoOperacion { get; set; }

    public string MonedaOperacion { get; set; } = null!;

    public string? CanalOperacion { get; set; }

    public string? CuentaOrigen { get; set; }

    public string? BancoOrigen { get; set; }

    public string? NombreOrdenante { get; set; }

    public string? IdentificacionOrdenante { get; set; }

    public string? CuentaDestino { get; set; }

    public string? BancoDestino { get; set; }

    public string? NombreBeneficiario { get; set; }

    public string? IdentificacionBeneficiario { get; set; }

    public string? MotivoOperacion { get; set; }

    public string? IpOrdenante { get; set; }
}
