using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class Role
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public byte Estatus { get; set; }

    public virtual ICollection<Autorizacione> Autorizaciones { get; set; } = new List<Autorizacione>();

    public virtual ICollection<UsuariosAutorizado> UsuariosAutorizados { get; set; } = new List<UsuariosAutorizado>();
}
