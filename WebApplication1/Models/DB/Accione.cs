using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class Accione
{
    public int Id { get; set; }

    public int MenuId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Endpoint { get; set; }

    public virtual ICollection<Autorizacione> Autorizaciones { get; set; } = new List<Autorizacione>();

    public virtual Menu Menu { get; set; } = null!;
}
