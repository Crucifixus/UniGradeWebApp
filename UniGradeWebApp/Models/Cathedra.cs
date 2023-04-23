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
    [Required(ErrorMessage = "ID факультету не повинен бути порожнім"), ForeignKey("Faculty")]
    [Display(Name = "Айді факультету")]
    public int CathFac { get; set; }
    [Display(Name = "Факультет")]
    public virtual Faculty CathFacNavigation { get; set; } = null!;
    [Display(Name = "Група")]
    public virtual ICollection<Group> Groups { get; } = new List<Group>();
    [Display(Name = "Предмет")]
    public virtual ICollection<Subject> Subjects { get; } = new List<Subject>();
}
