using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Equipment
{
    public int Idequipment { get; set; }

    public string Equipment1 { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
