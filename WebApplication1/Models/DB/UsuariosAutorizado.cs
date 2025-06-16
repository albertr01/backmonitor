using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class UsuariosAutorizado
{
    public int Id { get; set; }

    public string UsuarioAd { get; set; } = null!;

    public int RolId { get; set; }

    public byte Estatus { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<RevokedToken> RevokedTokens { get; set; } = new List<RevokedToken>();

    public virtual Role Rol { get; set; } = null!;
}
