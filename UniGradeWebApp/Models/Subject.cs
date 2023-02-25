using System;
using System.Collections.Generic;

namespace UniGradeWebApp;

public partial class Subject
{
    public int SbjId { get; set; }

    public int? SbjCath { get; set; }

    public string SbjTeach { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();

    public virtual Cathedra? SbjCathNavigation { get; set; }
}
