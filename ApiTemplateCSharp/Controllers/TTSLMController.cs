using ApiTemplateCSharp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTemplateCSharp.Controllers
{
    public class TTSLMController : Controller
    {
        private readonly TT1DbContext _db;
        private readonly TT2DbContext _db2;
        public TTSLMController(TT1DbContext db, TT2DbContext db2)
        {
            _db = db;
            _db2 = db2;
        }
        private async Task<List<TT1>> GetTT1(int page_first_result = 0, int results_per_page = 10)
        {
            return await _db.TT1.Skip(page_first_result).Take(results_per_page).ToListAsync();
        }
        private async Task<List<TT2>> GetTT2ByC1(string c1, int page_first_result = 0, int results_per_page = 10)
        {
            return await _db2.TT2.Where(item => item.C1.StartsWith(c1)).Skip(page_first_result).Take(results_per_page).ToListAsync();
        }
        private async Task<int> CountTT1Data()
        {
            return await _db.TT1.CountAsync();
        }
        private async Task<int> CountTT2Data(string c1)
        {
            return await _db2.TT2.Where(item => item.C1.StartsWith(c1)).CountAsync();
        }
        [HttpGet]
        public async Task<IActionResult> CountTT1Async()
        {
            int total = await CountTT1Data();

            Dictionary<string, int> data = new Dictionary<string, int>
            {
                { "total", total }
            };

            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> CountTT2Async([FromQuery] string c1 = "")
        {
            int total = await CountTT2Data(c1);

            Dictionary<string, int> data = new Dictionary<string, int>
            {
                { "total", total }
            };

            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> LastPageTT1Async()
        {
            int results_per_page = 10;

            int number_of_result = await CountTT1Data();

            //determine the total number of pages available
            int number_of_page = (int)Math.Ceiling(Convert.ToDecimal(number_of_result) / Convert.ToDecimal(results_per_page));

            Dictionary<string, int> data = new Dictionary<string, int>
            {
                { "number_of_page", number_of_page }
            };

            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> LastPageTT2Async([FromQuery] string c1 = "")
        {
            int results_per_page = 10;

            int number_of_result = await CountTT2Data(c1);

            //determine the total number of pages available
            int number_of_page = (int)Math.Ceiling(Convert.ToDecimal(number_of_result) / Convert.ToDecimal(results_per_page));

            Dictionary<string, int> data = new Dictionary<string, int>
            {
                { "number_of_page", number_of_page }
            };

            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> LoadTT1Async([FromQuery] int current_page)
        {
            int results_per_page = 10;

            //determine the sql LIMIT starting number for the results on the displaying page
            int page_first_result = (current_page - 1) * results_per_page;

            var tt1s = await GetTT1(page_first_result, results_per_page);

            return Json(tt1s);
        }
        [HttpGet]
        public async Task<IActionResult> LoadTT2Async([FromQuery] int current_page, string c1 = "")
        {
            int results_per_page = 10;

            //determine the sql LIMIT starting number for the results on the displaying page
            int page_first_result = (current_page - 1) * results_per_page;

            var tt2s = await GetTT2ByC1(c1, page_first_result, results_per_page);

            return Json(tt2s);
        }
    }
}
