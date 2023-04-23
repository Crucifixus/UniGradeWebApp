using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniGradeWebApp;

public partial class Student
{
    public int StnId { get; set; }
    [Required(ErrorMessage = "ПІБ студента не повинен бути порожнім")]
    [Display(Name = "ПІБ")]
    public string StnFullName { get; set; } = null!;
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Айді групи")]
    public int StnGrp { get; set; }
    [Display(Name = "Бал")]
    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();
    [Display(Name = "Група")]
    public virtual Group StnGrpNavigation { get; set; } = null!;
}
