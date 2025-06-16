using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class RefreshToken
{
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime Expiration { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual UsuariosAutorizado User { get; set; } = null!;
}
