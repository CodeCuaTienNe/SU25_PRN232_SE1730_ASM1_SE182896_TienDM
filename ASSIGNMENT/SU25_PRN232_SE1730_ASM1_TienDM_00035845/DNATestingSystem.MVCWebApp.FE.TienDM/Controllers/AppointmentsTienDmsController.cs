using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DNATestingSystem.Repository.TienDM.DBContext;
using DNATestingSystem.Repository.TienDM.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace DNATestingSystem.MVCWebApp.FE.TienDM.Controllers
{
    /// <summary>
    /// AppointmentsTienDms Controller - API Integration Status:
    /// ✓ ACTIVE API CALLS: GetAll (Index), GetById (Details)
    /// ⚠ COMMENTED API CALLS: Create, Update/Edit, Delete, Search
    /// 
    /// The commented API calls contain full implementation for future use.
    /// Currently using temporary placeholders that redirect to Index.
    /// </summary>
    [Authorize]
    public class AppointmentsTienDmsController : Controller
    {
        private string APIEndPoint = "http://localhost:8080/api/";

        public AppointmentsTienDmsController() { }

        // GET: AppointmentsTienDms
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                using (var response = await httpClient.GetAsync(APIEndPoint + "AppointmentsTienDM"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<List<AppointmentsTienDm>>(content);

                        if (result != null)
                        {
                            return View(result);
                        }
                    }
                }
            }

            return View(new List<AppointmentsTienDm>());
        }

        // GET: AppointmentsTienDms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                using (var response = await httpClient.GetAsync(APIEndPoint + $"AppointmentsTienDM/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var appointment = JsonConvert.DeserializeObject<AppointmentsTienDm>(content);
                        return View(appointment);
                    }
                }
            }

            return NotFound();
        }        // GET: AppointmentsTienDms/Create
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View();
        }

        // POST: AppointmentsTienDms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentDate,AppointmentTime,SamplingMethod,Address,ContactPhone,Notes,TotalAmount,IsPaid,AppointmentStatusesTienDmid,ServicesNhanVtid,UserAccountId")] AppointmentsTienDm appointmentsTienDm)
        {
            if (ModelState.IsValid)
            {
                // TODO: Implement create logic (temporarily return to index)
                return RedirectToAction(nameof(Index));

                // API Call Implementation (commented for future use)
                /*
                using (var httpClient = new HttpClient())
                {
                    var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                    appointmentsTienDm.CreatedDate = DateTime.Now;
                    appointmentsTienDm.ModifiedDate = DateTime.Now;

                    var json = JsonConvert.SerializeObject(appointmentsTienDm);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(APIEndPoint + "AppointmentsTienDM", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                */
            }

            await LoadDropdownsAsync();
            return View(appointmentsTienDm);
        }

        // GET: AppointmentsTienDms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                using (var response = await httpClient.GetAsync(APIEndPoint + $"AppointmentsTienDM/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var appointment = JsonConvert.DeserializeObject<AppointmentsTienDm>(content);

                        await LoadDropdownsAsync();
                        return View(appointment);
                    }
                }
            }

            return NotFound();
        }        // POST: AppointmentsTienDms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentsTienDmid,AppointmentDate,AppointmentTime,SamplingMethod,Address,ContactPhone,Notes,CreatedDate,TotalAmount,IsPaid,AppointmentStatusesTienDmid,ServicesNhanVtid,UserAccountId")] AppointmentsTienDm appointmentsTienDm)
        {
            if (id != appointmentsTienDm.AppointmentsTienDmid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // TODO: Implement edit logic (temporarily return to index)
                return RedirectToAction(nameof(Index));

                // API Call Implementation (commented for future use)
                /*
                using (var httpClient = new HttpClient())
                {
                    var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                    appointmentsTienDm.ModifiedDate = DateTime.Now;

                    var json = JsonConvert.SerializeObject(appointmentsTienDm);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PutAsync(APIEndPoint + $"AppointmentsTienDM/{id}", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                */
            }

            await LoadDropdownsAsync();
            return View(appointmentsTienDm);
        }

        // GET: AppointmentsTienDms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                using (var response = await httpClient.GetAsync(APIEndPoint + $"AppointmentsTienDM/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var appointment = JsonConvert.DeserializeObject<AppointmentsTienDm>(content);
                        return View(appointment);
                    }
                }
            }

            return NotFound();
        }        // POST: AppointmentsTienDms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // TODO: Implement delete logic (temporarily return to index)
            return RedirectToAction(nameof(Index));

            // API Call Implementation (commented for future use)
            /*
            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                using (var response = await httpClient.DeleteAsync(APIEndPoint + $"AppointmentsTienDM/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return RedirectToAction(nameof(Index));
            */
        }

        // GET: AppointmentsTienDms/Search
        public async Task<IActionResult> Search(string searchTerm, DateTime? fromDate, DateTime? toDate)
        {
            // TODO: Implement search logic (temporarily return all)
            return await Index();

            // API Call Implementation (commented for future use)
            /*
            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                var searchQuery = $"?searchTerm={searchTerm}";
                if (fromDate.HasValue)
                    searchQuery += $"&fromDate={fromDate.Value:yyyy-MM-dd}";
                if (toDate.HasValue)
                    searchQuery += $"&toDate={toDate.Value:yyyy-MM-dd}";

                using (var response = await httpClient.GetAsync(APIEndPoint + "AppointmentsTienDM/Search" + searchQuery))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<List<AppointmentsTienDm>>(content);

                        if (result != null)
                        {
                            return View("Index", result);
                        }
                    }
                }
            }

            return View("Index", new List<AppointmentsTienDm>());
            */
        }

        private async Task LoadDropdownsAsync()
        {
            // Load dropdown data from APIs
            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                // Load appointment statuses
                try
                {
                    using (var response = await httpClient.GetAsync(APIEndPoint + "AppointmentStatusesTienDM"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var statuses = JsonConvert.DeserializeObject<List<AppointmentStatusesTienDm>>(content);
                            ViewData["AppointmentStatusesTienDmid"] = new SelectList(statuses, "AppointmentStatusesTienDmid", "StatusName");
                        }
                    }
                }
                catch
                {
                    ViewData["AppointmentStatusesTienDmid"] = new SelectList(new List<AppointmentStatusesTienDm>(), "AppointmentStatusesTienDmid", "StatusName");
                }                // Load services - assuming there's an API endpoint
                try
                {
                    // Note: You may need to adjust this endpoint based on your actual API
                    ViewData["ServicesNhanVtid"] = new SelectList(new List<object>(), "Id", "Name");
                }
                catch
                {
                    ViewData["ServicesNhanVtid"] = new SelectList(new List<object>(), "Id", "Name");
                }

                // Load user accounts - for admin use
                try
                {
                    // Note: You may need to adjust this endpoint based on your actual API  
                    ViewData["UserAccountId"] = new SelectList(new List<object>(), "Id", "Email");
                }
                catch
                {
                    ViewData["UserAccountId"] = new SelectList(new List<object>(), "Id", "Email");
                }
            }
        }
    }
}
