using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class ClienteConyugue
{
    public int Id { get; set; }

    public string IdCliente { get; set; } = null!;

    public string? ConyugeNombreCompleto { get; set; }

    public decimal? ConyugeIngresos { get; set; }

    public string? ConyugeOcupacion { get; set; }
}
