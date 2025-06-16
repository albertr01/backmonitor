using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class Menu
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public string Label { get; set; } = null!;

    public string? Icon { get; set; }

    public string? RouteLink { get; set; }

    public int OrderIndex { get; set; }

    public bool IsActive { get; set; }

    public bool IsSeparator { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Accione> Acciones { get; set; } = new List<Accione>();

    public virtual ICollection<Menu> InverseParent { get; set; } = new List<Menu>();

    public virtual Menu? Parent { get; set; }
}
