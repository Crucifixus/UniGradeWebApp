using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniGradeWebApp;

public partial class Faculty
{
    public int FacId { get; set; }
    [Required(ErrorMessage = "Назва факультету не повинна бути порожнім")]
    [Display(Name = "Факультет")]
    public string FacName { get; set; } = null!;
    [Display(Name = "Рік заснування факультету")]
    public short? FacFoundingYear { get; set; }

    public virtual ICollection<Cathedra> Cathedras { get; } = new List<Cathedra>();
}
