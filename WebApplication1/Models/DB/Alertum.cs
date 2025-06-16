using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class Alertum
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Codigo { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<SubcripcionAlertum> SubcripcionAlerta { get; set; } = new List<SubcripcionAlertum>();
}
