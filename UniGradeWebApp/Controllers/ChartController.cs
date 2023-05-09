using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UniGradeWebApp.Controllers
{
    [Route("api/Chart")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DbUniGradeSystemContext _context;
        public ChartController(DbUniGradeSystemContext context)
        {
            _context = context;
        }
        [HttpGet("JsonGroupGradeData")]
        public JsonResult JsonGroupGradeData(int CathId)
        {
            var groupNameById = new Dictionary<int, string>();
            foreach (var g in _context.Groups)
            {
                if (g.GrpCath == CathId)
                {
                    groupNameById.Add(g.GrpId, g.GrpName);
                }
            }
            var students = new Dictionary<int, (int, double?)>();
            foreach (var s in _context.Students.Include(s =>s.Grades))
            {
                if (groupNameById.ContainsKey(s.StnGrp))
                {
                    double? avg = s.Grades.Average(g => g.GrdResult);
                    students.Add(s.StnId, (s.StnGrp, avg));
                }
            }
            var groupGrades = new Dictionary<int, List<double>>();
            foreach (var s in students)
            {
                if (s.Value.Item2 is not null)
                {
                    groupGrades.TryAdd(s.Value.Item1, new List<double>());
                    groupGrades[s.Value.Item1].Add((double)s.Value.Item2);
                }
            }
            var result = new List<object> { new[] { "Група", "Середній бал" } };
            foreach (var g in groupGrades)
            {
                result.Add(new object[] { groupNameById[g.Key], g.Value.Average() });
            }
            return new JsonResult(result);
        }
        [HttpGet("JsonStudentGradeData")]
        public JsonResult JsonStudentGradeData(int StnId)
        {
            var grds = new List<object> { new[] { "Предмет", "Бал" } };
            foreach (var s in _context.Students.Include(s => s.Grades).ThenInclude(g => g.GrdSbjNavigation))
            {
                if (s.StnId == StnId)
                {
                    foreach (var g in s.Grades)
                    {
                        grds.Add(new object[] { g.GrdSbjNavigation.SbjName, (double)g.GrdResult });
                    }
                }
            }
            return new JsonResult(grds);
        }
    }
}
