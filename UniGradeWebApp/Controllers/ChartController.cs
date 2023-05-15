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
            //List з назвами груп і їх середніми балами, для груп однієї кафедри (у якої заданий ID) 
            var grade_data = (from g in _context.Groups
                         join st in
                         (from s in _context.Students
                          join gr in
                          (from gr in _context.Grades
                           group gr by gr.GrdStn into grAvg
                           select new
                           {
                               GrdStn = grAvg.Key,
                               AvgPerStudent = grAvg.Average(gr => gr.GrdResult)
                           })
                          on s.StnId equals gr.GrdStn
                          group gr by s.StnGrp into stAvg
                          select new
                          {
                              StnGrp = stAvg.Key,
                              AvgPerGroup = stAvg.Average(st => st.AvgPerStudent)
                          })
                         on g.GrpId equals st.StnGrp
                         where g.GrpCath == 2
                         select new
                         {
                             GrpName = g.GrpName,
                             AvgPerGroup = st.AvgPerGroup
                         }).ToList();
            var result = new List<object> { new[] { "Група", "Середній бал" } };
            foreach (var g in grade_data)
            {
                result.Add(new object[] { g.GrpName, g.AvgPerGroup });
            }
            return new JsonResult(result);
        }
        [HttpGet("JsonStudentGradeData")]
        public JsonResult JsonStudentGradeData(int StnId)
        {
            //List з оцінки даного студента після назв відповідних предметів
            var result = new List<object> { new[] { "Предмет", "Бал" } };
            var grades = (from g in _context.Grades
                         join s in _context.Subjects on g.GrdSbj equals s.SbjId
                         where g.GrdStn == StnId
                         select new { SbjName = s.SbjName, GrdResult = g.GrdResult }).ToList();
            foreach (var g in grades)
            {
                result.Add(new object[] { g.SbjName, g.GrdResult });
            }
            return new JsonResult(result);
        }
    }
}
