using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class ClienteReferencia
{
    public int Id { get; set; }

    public string IdCliente { get; set; } = null!;

    public string TipoReferencia { get; set; } = null!;

    public string NombreReferencia { get; set; } = null!;

    public string? ContactoReferencia { get; set; }
}
