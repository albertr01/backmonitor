using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class ListaPep
{
    public int Id { get; set; }

    public string? TipPersonaExpuesta { get; set; }

    public int NumDocumento { get; set; }

    public string NombApellido { get; set; } = null!;

    public string EnteAdscripcion { get; set; } = null!;

    public string Cargo { get; set; } = null!;

    public string Pais { get; set; } = null!;
}
