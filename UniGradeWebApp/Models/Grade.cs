using System;
using System.Collections.Generic;

namespace UniGradeWebApp;

public partial class Grade
{
    public int GrdId { get; set; }

    public int GrdSbj { get; set; }

    public int GrdStn { get; set; }

    public byte? GrdResult { get; set; }

    public virtual Subject GrdSbjNavigation { get; set; } = null!;

    public virtual Student GrdStnNavigation { get; set; } = null!;
}
