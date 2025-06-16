using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class MstAgencia
{
    public int Id { get; set; }

    public string Agencia { get; set; } = null!;

    public string NombreGerente { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string Municipio { get; set; } = null!;

    public bool OfcFronteriza { get; set; }
}
