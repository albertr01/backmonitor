using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class ClienteBase
{
    public int Id { get; set; }

    public string IdCliente { get; set; } = null!;

    public string TipoPersona { get; set; } = null!;

    public string NombreCompletoPrimerNombre { get; set; } = null!;

    public string? NombreCompletoSegundoNombre { get; set; }

    public string NombreCompletoPrimerApellido { get; set; } = null!;

    public string? NombreCompletoSegundoApellido { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string? Nacionalidad { get; set; }

    public string? Profesion { get; set; }

    public string? Oficio { get; set; }

    public string? ActividadEconomica { get; set; }

    public string? Vivienda { get; set; }

    public int? CargaFamiliar { get; set; }

    public bool? EsPep { get; set; }

    public string? RelacionPep { get; set; }

    public string? RazonSocial { get; set; }

    public string? Rif { get; set; }

    public DateOnly? FechaConstitucion { get; set; }

    public string? CategoriaEspecial { get; set; }
}
