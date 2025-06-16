using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class Autorizacione
{
    public int Id { get; set; }

    public int RolId { get; set; }

    public int AccionId { get; set; }

    public virtual Accione Accion { get; set; } = null!;

    public virtual Role Rol { get; set; } = null!;
}
