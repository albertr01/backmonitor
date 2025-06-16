using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class AlertaMonto
{
    public decimal Id { get; set; }

    public DateTime FechaConfiguracion { get; set; }

    public decimal? IdTipoOperacion { get; set; }

    public decimal? IdTipoMoneda { get; set; }

    public decimal? Umbral { get; set; }

    public string? Comentario { get; set; }

    public string? Codigo { get; set; }

    public int? IdCliente { get; set; }

    public int? IdAgencia { get; set; }

    public virtual Agencium? IdAgenciaNavigation { get; set; }

    public virtual TipoMonedum? IdTipoMonedaNavigation { get; set; }

    public virtual TipoOperacion? IdTipoOperacionNavigation { get; set; }
}
