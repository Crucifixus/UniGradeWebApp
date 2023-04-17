using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UniGradeWebApp;

public partial class Cathedra
{
    [Key]
    public int CathId { get; set; }
    [Required(ErrorMessage = "Власна назва не повинна бути порожньою")]
    [Display(Name = "Кафедра")]
    public string CathName { get; set; } = null!;
    //[Required(ErrorMessage = "ID факультету не повинна бути порожньою"), ForeignKey("Faculty")]
    //[Display(Name = "Факультети кафедри")]
    public int CathFac { get; set; }

    public virtual Faculty CathFacNavigation { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; } = new List<Group>();

    public virtual ICollection<Subject> Subjects { get; } = new List<Subject>();
}
