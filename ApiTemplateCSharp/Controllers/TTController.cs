using ApiTemplateCSharp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTemplateCSharp.Controllers
{
    public class TTController : Controller
    {
        private readonly TT1DbContext _db;
        private readonly TT2DbContext _db2;
        public TTController(TT1DbContext db, TT2DbContext db2)
        {
            _db = db;
            _db2 = db2;
        }
        private async Task<List<TT1>> GetTT1()
        {
            return await _db.TT1.ToListAsync();
        }
        private async Task<List<TT2>> GetTT2ByC1(string c1)
        {
            return await _db2.TT2.Where(item => item.C1.StartsWith(c1)).ToListAsync();
        }
        [HttpGet]
        public async Task<IActionResult> LoadTT1Async()
        {
            var tt1s = await GetTT1();

            return Json(tt1s);
        }
        [HttpGet]
        public async Task<IActionResult> LoadTT2Async([FromQuery] string c1 = "")
        {
            var tt2s = await GetTT2ByC1(c1);

            return Json(tt2s);
        }
    }
}
