using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniGradeWebApp;

public partial class Subject
{
    public int SbjId { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Дисципліна")]
    public string SbjName { get; set; } = null!;
    [Display(Name = "Кафедра")]
    public int? SbjCath { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Викладач")]
    public string SbjTeach { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();

    public virtual Cathedra? SbjCathNavigation { get; set; }
}
