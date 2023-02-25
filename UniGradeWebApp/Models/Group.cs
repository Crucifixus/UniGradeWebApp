using System;
using System.Collections.Generic;

namespace UniGradeWebApp;

public partial class Group
{
    public int GrpId { get; set; }

    public string GrpName { get; set; } = null!;

    public short GrpEnrollmentYear { get; set; }

    public int GrpCath { get; set; }

    public virtual Cathedra GrpCathNavigation { get; set; } = null!;

    public virtual ICollection<Student> Students { get; } = new List<Student>();
}
