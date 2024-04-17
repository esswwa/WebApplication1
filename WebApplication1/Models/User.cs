using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class User
{
    public int IdUser { get; set; }

    public int IdRole { get; set; }

    public string Fullname { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Application> ApplicationIdExecutorNavigations { get; set; } = new List<Application>();

    public virtual ICollection<Application> ApplicationIdUserNavigations { get; set; } = new List<Application>();

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
