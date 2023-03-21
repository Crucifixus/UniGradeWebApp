using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniGradeWebApp;

public partial class Group
{
    public int GrpId { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Група")]
    public string GrpName { get; set; } = null!;
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Рік вступу")]
    public short GrpEnrollmentYear { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Кафедра")]
    public int GrpCath { get; set; }

    public virtual Cathedra GrpCathNavigation { get; set; } = null!;

    public virtual ICollection<Student> Students { get; } = new List<Student>();
}
