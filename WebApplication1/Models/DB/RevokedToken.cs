using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class RevokedToken
{
    public Guid Id { get; set; }

    public string Token { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime? RevokedAt { get; set; }

    public virtual UsuariosAutorizado User { get; set; } = null!;
}
