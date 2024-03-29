﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniGradeWebApp;

public partial class Group
{
    public int GrpId { get; set; }
    [Required(ErrorMessage = "Назва групи не повинна бути порожньою")]
    [Display(Name = "Група")]
    public string GrpName { get; set; } = null!;
    [Required(ErrorMessage = "Рік вступу групи не повинен бути порожнім")]
    [Display(Name = "Рік вступу")]
    public short GrpEnrollmentYear { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Айді кафедра")]
    public int GrpCath { get; set; }
    [Display(Name = "Кафедра")]
    public virtual Cathedra GrpCathNavigation { get; set; } = null!;
    [Display(Name = "Студент")]
    public virtual ICollection<Student> Students { get; } = new List<Student>();
}
