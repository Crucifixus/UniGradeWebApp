using System;
using System.Collections.Generic;

namespace UniGradeWebApp;

public partial class Cathedra
{
    public int CathId { get; set; }

    public string CathName { get; set; } = null!;

    public int CathFac { get; set; }

    public virtual Faculty CathFacNavigation { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; } = new List<Group>();

    public virtual ICollection<Subject> Subjects { get; } = new List<Subject>();
}
