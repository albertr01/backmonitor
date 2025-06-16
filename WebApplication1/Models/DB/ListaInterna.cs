using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class ListaInterna
{
    public int Id { get; set; }

    public int NumDocumento { get; set; }

    public string NombRazonSocial { get; set; } = null!;

    public string Motivo { get; set; } = null!;
}
