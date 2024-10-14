using ApiTemplateCSharp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTemplateCSharp.Controllers
{
    public class Sample1Controller : Controller
    {
        private readonly UserAccountsDbContext _db;
        public Sample1Controller(UserAccountsDbContext db)
        {
            _db = db;
        }
        private async Task<List<UserAccounts>> GetUserAccounts()
        {
            return await _db.UserAccounts.ToListAsync();
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
        public async Task<IActionResult> LoadUserAccountsJsonAsync()
        {
            var user_accounts = await GetUserAccounts();
            return Json(user_accounts);
        }
        [HttpGet]
        public async Task<IActionResult> SearchUserAccountsJsonAsync([FromQuery] string employee_no = "", string full_name = "", string user_type = "")
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
            }
            else
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
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSelected([FromBody] List<int> arr)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            if (!ModelState.IsValid)
                return BadRequest("Enter Required Fields");

            int count = arr.Count();
            int deleted_count = 0;

            foreach (var id in arr)
            {
                int deleted = await DeleteUserAccount(id);
                if (deleted > 0)
                {
                    deleted_count++;
                    count--;
                }
            }

            data.Add("deleted", deleted_count.ToString());

            if (count == 0)
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
        public async Task<IActionResult> Import3Async()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            var files = Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                string fileName = file.FileName;

                if (!string.IsNullOrEmpty(fileName))
                {
                    var csvData = new List<UserAccounts>();

                    try
                    {
                        // Use a MemoryStream to read the file directly from memory
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            memoryStream.Position = 0; // Reset the position to the beginning of the stream

                            using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                            {
                                // Skip first line (header)
                                await reader.ReadLineAsync();

                                string line;
                                while ((line = await reader.ReadLineAsync()) != null)
                                {
                                    var row = line.Split(',');

                                    var csvRow = new UserAccounts
                                    {
                                        Id = 0,
                                        IdNumber = row[0],
                                        FullName = row[1],
                                        Username = row[2],
                                        Password = row[3],
                                        Section = row[4],
                                        Role = row[5]
                                    };

                                    csvData.Add(csvRow);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data["message"] = "SYSTEM ERROR: " + ex.Message + " " + ex.ToString();
                        return Json(data);
                    }

                    var entitiesToAdd = new List<UserAccounts>();

                    foreach (var csvRow in csvData)
                    {
                        entitiesToAdd.Add(new UserAccounts
                        {
                            Id = csvRow.Id,
                            IdNumber = csvRow.IdNumber,
                            FullName = csvRow.FullName,
                            Username = csvRow.Username,
                            Password = csvRow.Password,
                            Section = csvRow.Section,
                            Role = csvRow.Role
                        });

                        if (entitiesToAdd.Count >= 250)
                        {
                            _db.UserAccounts.AddRange(entitiesToAdd);
                            await _db.SaveChangesAsync();
                            entitiesToAdd.Clear();
                        }
                    }

                    if (entitiesToAdd.Count > 0)
                    {
                        _db.UserAccounts.AddRange(entitiesToAdd);
                        await _db.SaveChangesAsync();
                    }

                    data["message"] = "success"; // Set success message
                    return Json(data); // Return success message here
                }
                else
                {
                    data["message"] = "Failed to get filename. Please upload file"; // Update the message
                    return Json(data);
                }
            }
            else
            {
                data["message"] = "Please upload a file"; // Update the message
                return Json(data);
            }

            // This part is no longer needed
            // data["message"] = "No file uploaded"; 
            // return Json(data);
        }
    }
}
