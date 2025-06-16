using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class ClientesEcoNegocioPropio
{
    public int Id { get; set; }

    public string IdCliente { get; set; } = null!;

    public string NegocioNombreEmpresa { get; set; } = null!;

    public string? NegocioContactoTelefono { get; set; }

    public string? NegocioContactoCorreo { get; set; }

    public decimal? NegocioIngreso { get; set; }

    public string? NegocioCargo { get; set; }

    public string? NegocioPrincipalCliente { get; set; }

    public string? NegocioPrincipalProveedor { get; set; }
}
