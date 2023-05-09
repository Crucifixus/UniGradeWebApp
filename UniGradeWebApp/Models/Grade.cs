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
    [Display(Name = "Айді студента")]
    public int GrdStn { get; set; }
    [Display(Name = "Бал")]
    [Range(0, 100)]
    public byte? GrdResult { get; set; }
    [Display(Name = "Предмет")]
    public virtual Subject GrdSbjNavigation { get; set; } = null!;
    [Display(Name = "Студент")]
    public virtual Student GrdStnNavigation { get; set; } = null!;
}
