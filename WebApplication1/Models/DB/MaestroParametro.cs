using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class MaestroParametro
{
    public int Id { get; set; }

    public string Parametro { get; set; } = null!;

    public int Idparametro { get; set; }

    public string? TipoValor { get; set; }

    public string? Valor { get; set; }

    public string? Color { get; set; }
}
