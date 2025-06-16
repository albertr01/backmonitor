using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class Agencium
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<AlertaMonto> AlertaMontos { get; set; } = new List<AlertaMonto>();
}
