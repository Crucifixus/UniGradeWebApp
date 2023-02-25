using System;
using System.Collections.Generic;

namespace UniGradeWebApp;

public partial class Faculty
{
    public int FacId { get; set; }

    public string FacName { get; set; } = null!;

    public short? FacFoundingYear { get; set; }

    public virtual ICollection<Cathedra> Cathedras { get; } = new List<Cathedra>();
}
