using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class ClientesAgencium
{
    public int Id { get; set; }

    public string IdCliente { get; set; } = null!;

    public string AgenciaCodigo { get; set; } = null!;

    public string? AgenciaNombre { get; set; }

    public string? GerenteNombreCompleto { get; set; }

    public string? EjecutivoNegociosNombreCompleto { get; set; }
}
