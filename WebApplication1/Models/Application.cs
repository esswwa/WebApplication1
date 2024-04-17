using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Application
{
    public int IdApplication { get; set; }

    public int IdEquipment { get; set; }

    public int IdType { get; set; }

    public int IdExecutor { get; set; }

    public int IdUser { get; set; }

    public string Description { get; set; } = null!;

    public string Cost { get; set; } = null!;

    public string Material { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Phase { get; set; } = null!;

    public DateOnly DateAdd { get; set; }

    public DateOnly? DateEnd { get; set; }

    public string? Time { get; set; }

    public virtual Equipment IdEquipmentNavigation { get; set; } = null!;

    public virtual User IdExecutorNavigation { get; set; } = null!;

    public virtual TypeMalfunction IdTypeNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
