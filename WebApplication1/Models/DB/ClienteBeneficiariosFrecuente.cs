using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class ClienteBeneficiariosFrecuente
{
    public int Id { get; set; }

    public string IdCliente { get; set; } = null!;

    public string TipoTransaccion { get; set; } = null!;

    public string IdentificadorBeneficiario { get; set; } = null!;

    public string? NombreBeneficiario { get; set; }

    public string? CedulaBeneficiario { get; set; }

    public string? RifBeneficiario { get; set; }

    public string? BancoBeneficiario { get; set; }

    public string? NumeroCuentaBeneficiario { get; set; }

    public DateTime? FechaUltimaTransaccion { get; set; }
}
