using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniGradeWebApp;

public partial class Grade
{
    public int GrdId { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Дисципліна")]
    public int GrdSbj { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Студент")]
    public int GrdStn { get; set; }
    [Display(Name = "Бал")]
    public byte? GrdResult { get; set; }

    public virtual Subject GrdSbjNavigation { get; set; } = null!;

    public virtual Student GrdStnNavigation { get; set; } = null!;
}
