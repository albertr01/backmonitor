using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class TipoMonedum
{
    public decimal Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<AlertaMonto> AlertaMontos { get; set; } = new List<AlertaMonto>();
}
