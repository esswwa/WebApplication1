using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class TypeMalfunction
{
    public int IdtypeMalfunction { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
