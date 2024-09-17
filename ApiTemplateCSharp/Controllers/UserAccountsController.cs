using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTemplateCSharp.Entities;

namespace ApiTemplateCSharp.Controllers
{
    public class UserAccountsController : Controller
    {
        private readonly UserAccountsDbContext _db;
        public UserAccountsController(UserAccountsDbContext db)
        {
            _db = db;
        }
        private async Task<List<UserAccounts>> GetUserAccounts()
        {
            return await _db.UserAccounts.ToListAsync();
        }
        private async Task<int> CountUserAccounts(string id_number, string full_name = "", string role = "")
        {
            var query = _db.UserAccounts.AsQueryable();

            if (!string.IsNullOrEmpty(id_number))
            {
                query = query.Where(item => item.IdNumber.StartsWith(id_number));
            }

            if (!string.IsNullOrEmpty(full_name))
            {
                query = query.Where(item => item.FullName.StartsWith(full_name));
            }

            if (!string.IsNullOrEmpty(role))
            {
                query = query.Where(item => item.Role == role);
            }

            return await query.CountAsync();
        }
        private async Task<UserAccounts> GetUserAccountById(int id)
        {
            return await _db.UserAccounts.FirstOrDefaultAsync(ua => ua.Id == id);
        }
        private async Task<List<UserAccounts>> GetUserAccountsSearch(string id_number, string full_name, string role)
        {
            //return await _db.UserAccounts.Where(ua => ua.id_number.Contains(id_number) && ua.full_name.Contains(full_name) && ua.role.Contains(role)).ToListAsync();
            var query = _db.UserAccounts.AsQueryable();

            if (!string.IsNullOrEmpty(id_number))
            {
                //query = query.Where(item => item.id_number.Contains(id_number));
                query = query.Where(item => item.IdNumber.StartsWith(id_number));
            }

            if (!string.IsNullOrEmpty(full_name))
            {
                //query = query.Where(item => item.full_name.Contains(full_name));
                query = query.Where(item => item.FullName.StartsWith(full_name));
            }

            if (!string.IsNullOrEmpty(role))
            {
                //query = query.Where(item => item.role.Contains(role));
                query = query.Where(item => item.Role == role);
            }

            return await query.ToListAsync();
        }
        private async Task<int> InsertUserAccount(UserAccounts user_account)
        {
            _db.UserAccounts.Add(user_account);
            return await _db.SaveChangesAsync();
        }
        private async Task<int> UpdateUserAccount(UserAccounts updated_user_account)
        {
            var user_account = await GetUserAccountById(updated_user_account.Id);
            if (user_account != null)
            {
                user_account.IdNumber = updated_user_account.IdNumber;
                user_account.FullName = updated_user_account.FullName;
                user_account.Username = updated_user_account.Username;
                if (!string.IsNullOrEmpty(updated_user_account.Password))
                {
                    user_account.Password = updated_user_account.Password;
                }
                user_account.Section = updated_user_account.Section;
                user_account.Role = updated_user_account.Role;
                return await _db.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }
        private async Task<int> DeleteUserAccount(int id)
        {
            _db.UserAccounts.Remove(await GetUserAccountById(id));
            return await _db.SaveChangesAsync();
        }
        [HttpGet]
        public async Task<IActionResult> CountAsync([FromQuery] string employee_no = "", string full_name = "", string user_type = "")
        {
            int total = await CountUserAccounts(employee_no, full_name, user_type);

            Dictionary<string, int> data = new Dictionary<string, int>();

            data.Add("total", total);

            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> LoadUserAccountsAsync()
        {
            var user_accounts = await GetUserAccounts();

            return Json(user_accounts);
        }
        [HttpGet]
        public async Task<IActionResult> SearchUserAccountsAsync([FromQuery] string employee_no = "", string full_name = "", string user_type = "")
        {
            var user_accounts = await GetUserAccountsSearch(employee_no, full_name, user_type);

            return Json(user_accounts);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Insert(UserAccounts user_account)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            if (!ModelState.IsValid)
                return BadRequest("Enter Required Fields");

            int inserted = await InsertUserAccount(user_account);

            data.Add("inserted", inserted.ToString());

            if (inserted > 0)
            {
                data.Add("message", "success");
            } else
            {
                data.Add("message", "failed");
            }

            return Json(data);
        }
        /*[HttpPost]
        public async Task<IActionResult> InsertAsync(IFormCollection fc)
        {
            string data = "";

            UserAccountsModel user_account = new UserAccountsModel
            {
                Id = 0,
                IdNumber = fc["IdNumber"],
                FullName = fc["FullName"],
                Username = fc["Username"],
                Password = fc["Password"],
                Section = fc["Section"],
                Role = fc["Role"]
            };

            int inserted = await InsertUserAccount(user_account);

            if (inserted > 0)
            {
                data = "success";
            }

            return Content(data, "text/html");
        }*/
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserAccounts user_account)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            if (!ModelState.IsValid)
                return BadRequest("Enter Required Fields");

            int updated = await UpdateUserAccount(user_account);

            data.Add("updated", updated.ToString());

            if (updated > 0)
            {
                data.Add("message", "success");
            }
            else
            {
                data.Add("message", "failed");
            }

            return Json(data);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(IFormCollection fc)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            if (!ModelState.IsValid)
                return BadRequest("Enter Required Fields");

            int deleted = await DeleteUserAccount(int.Parse(fc["Id"]));

            data.Add("deleted", deleted.ToString());

            if (deleted > 0)
            {
                data.Add("message", "success");
            }
            else
            {
                data.Add("message", "failed");
            }

            return Json(data);
        }
    }
}
