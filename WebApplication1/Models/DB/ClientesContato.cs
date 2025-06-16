using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class ClientesContato
{
    public int Id { get; set; }

    public string IdCliente { get; set; } = null!;

    public string TipoContacto { get; set; } = null!;

    public string? TipoDireccion { get; set; }

    public string? DireccionCalle { get; set; }

    public string? DireccionNumero { get; set; }

    public string? DireccionApartamento { get; set; }

    public string? DireccionUrbanizacion { get; set; }

    public string? DireccionMunicipio { get; set; }

    public string? DireccionEstado { get; set; }

    public string? DireccionCodigoPostal { get; set; }

    public string? DireccionPais { get; set; }

    public string? TipoTelefono { get; set; }

    public string? TelefonoCodigoPais { get; set; }

    public string? TelefonoNumero { get; set; }

    public string? CorreoElectronico { get; set; }
}
