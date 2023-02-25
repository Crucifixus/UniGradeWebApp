using System;
using System.Collections.Generic;

namespace UniGradeWebApp;

public partial class Student
{
    public int StnId { get; set; }

    public string StnFullName { get; set; } = null!;

    public int StnGrp { get; set; }

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();

    public virtual Group StnGrpNavigation { get; set; } = null!;
}
