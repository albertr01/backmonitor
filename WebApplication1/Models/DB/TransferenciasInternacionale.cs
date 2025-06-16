using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class TransferenciasInternacionale
{
    public int Id { get; set; }

    public string IdCliente { get; set; } = null!;

    public string IdTransferencia { get; set; } = null!;

    public DateTime FechaHoraTransaccion { get; set; }

    public decimal MontoTransferencia { get; set; }

    public string MonedaTransferencia { get; set; } = null!;

    public string TipoTransferencia { get; set; } = null!;

    public string? SwiftInstitucionOrdenante { get; set; }

    public string? InstitucionOrdenante { get; set; }

    public string? PaisOrdenante { get; set; }

    public string? NombreOrdenante { get; set; }

    public string? IdentificacionOrdenante { get; set; }

    public string? SwiftInstitucionBeneficiario { get; set; }

    public string? InstitucionBeneficiario { get; set; }

    public string? PaisBeneficiario { get; set; }

    public string? NombreBeneficiario { get; set; }

    public string? IdentificacionBeneficiario { get; set; }

    public string? NumeroCuentaBeneficiario { get; set; }

    public string? MotivoTransferencia { get; set; }

    public string? IpOrdenanteTransaccion { get; set; }

    public string? IpBeneficiarioTransaccion { get; set; }
}
