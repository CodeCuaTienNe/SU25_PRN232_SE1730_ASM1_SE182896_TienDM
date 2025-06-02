using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DNATestingSystem.Repository.TienDM.DBContext;
using DNATestingSystem.Repository.TienDM.Models;
using DNATestingSystem.MVCWebApp.FE.TienDM.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace DNATestingSystem.MVCWebApp.FE.TienDM.Controllers
{    /// <summary>
     /// AppointmentsTienDms Controller - API Integration Status:
     /// ✓ ACTIVE API CALLS: All CRUD operations (GetAll, GetById, Create, Update, Delete, Search)
     /// ✓ DROPDOWN DATA: Fully implemented with API calls to ServicesNhanVt, AppointmentStatusesTienDM, SystemUserAccount
     /// 
     /// All operations are now using API endpoints with proper JWT authentication.
     /// Search functionality implemented with correct parameters (id, contactPhone, totalAmount).
     /// LoadDropdownsAsync properly loads all dropdown data from Backend APIs.
     /// </summary>
    [Authorize]
    public class AppointmentsTienDmsController : Controller
    {
        private string APIEndPoint = "http://localhost:8080/api/";

        public AppointmentsTienDmsController() { }        // GET: AppointmentsTienDms
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                if (!string.IsNullOrEmpty(tokenString))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                }

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
        }        // POST: AppointmentsTienDms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentDate,AppointmentTime,SamplingMethod,Address,ContactPhone,Notes,TotalAmount,IsPaid,AppointmentStatusesTienDmid,ServicesNhanVtid")] AppointmentsTienDm appointmentsTienDm)
        {
            // Enhanced logging for debugging
            Console.WriteLine($"=== CREATE APPOINTMENT DEBUG ===");
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
            Console.WriteLine($"AppointmentDate: {appointmentsTienDm.AppointmentDate}");
            Console.WriteLine($"AppointmentTime: {appointmentsTienDm.AppointmentTime}");
            Console.WriteLine($"ContactPhone: {appointmentsTienDm.ContactPhone}");
            Console.WriteLine($"TotalAmount: {appointmentsTienDm.TotalAmount}");
            Console.WriteLine($"ServicesNhanVtid: {appointmentsTienDm.ServicesNhanVtid}");
            Console.WriteLine($"AppointmentStatusesTienDmid: {appointmentsTienDm.AppointmentStatusesTienDmid}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("=== MODEL STATE ERRORS ===");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // API Call Implementation
                    using (var httpClient = new HttpClient())
                    {
                        var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                        if (string.IsNullOrEmpty(tokenString))
                        {
                            Console.WriteLine("ERROR: No token found in cookies");
                            ModelState.AddModelError("", "Authentication token not found. Please login again.");
                            await LoadDropdownsAsync();
                            return View(appointmentsTienDm);
                        }

                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                        appointmentsTienDm.CreatedDate = DateTime.Now;
                        appointmentsTienDm.ModifiedDate = DateTime.Now;
                        // UserAccountId will be auto-assigned by the Backend API from JWT token

                        // Enhanced JSON serialization with custom converters
                        var json = JsonConvert.SerializeObject(appointmentsTienDm, new JsonSerializerSettings
                        {
                            DateFormatHandling = DateFormatHandling.IsoDateFormat,
                            Converters = new List<JsonConverter>
                            {
                                new Converters.DateOnlyConverter(),
                                new Converters.TimeOnlyConverter()
                            }
                        });

                        Console.WriteLine($"Serialized JSON: {json}");

                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        using (var response = await httpClient.PostAsync(APIEndPoint + "AppointmentsTienDM", content))
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"API Response Status: {response.StatusCode}");
                            Console.WriteLine($"API Response Content: {responseContent}");

                            if (response.IsSuccessStatusCode)
                            {
                                Console.WriteLine("SUCCESS: Appointment created successfully");
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                // Enhanced error logging for debugging
                                Console.WriteLine($"ERROR: Failed to create appointment");
                                Console.WriteLine($"Status Code: {response.StatusCode}");
                                Console.WriteLine($"Reason Phrase: {response.ReasonPhrase}");
                                Console.WriteLine($"Response Headers: {response.Headers}");

                                ModelState.AddModelError("", $"Failed to create appointment. Status: {response.StatusCode}, Details: {responseContent}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"EXCEPTION in Create: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
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
                // API Call Implementation
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
                        else
                        {
                            // Log the error or handle failed update
                            ModelState.AddModelError("", "Failed to update appointment. Please try again.");
                        }
                    }
                }
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
            // API Call Implementation
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
                    else
                    {
                        // Log the error or handle failed deletion
                        TempData["ErrorMessage"] = "Failed to delete appointment. Please try again.";
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }        // GET: AppointmentsTienDms/Search
        public async Task<IActionResult> Search(int id = 0, string contactPhone = "", decimal totalAmount = 0)
        {
            // Pass search parameters to ViewBag for maintaining search state
            ViewBag.SearchId = id;
            ViewBag.SearchContactPhone = contactPhone;
            ViewBag.SearchTotalAmount = totalAmount;

            // API Call Implementation
            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                if (!string.IsNullOrEmpty(tokenString))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                }

                var searchQuery = $"?id={id}&contactPhone={Uri.EscapeDataString(contactPhone ?? "")}&totalAmount={totalAmount}";

                using (var response = await httpClient.GetAsync(APIEndPoint + "AppointmentsTienDM/search" + searchQuery))
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
                    else
                    {
                        // Handle API error
                        TempData["ErrorMessage"] = "Failed to search appointments. Please try again.";
                    }
                }
            }

            return View("Index", new List<AppointmentsTienDm>());
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
                }

                // Load services
                try
                {
                    using (var response = await httpClient.GetAsync(APIEndPoint + "ServicesNhanVt"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var services = JsonConvert.DeserializeObject<List<ServicesNhanVt>>(content);
                            ViewData["ServicesNhanVtid"] = new SelectList(services, "ServicesNhanVtid", "ServiceName");
                        }
                    }
                }
                catch
                {
                    ViewData["ServicesNhanVtid"] = new SelectList(new List<ServicesNhanVt>(), "ServicesNhanVtid", "ServiceName");
                }

                // Load user accounts - for admin use
                try
                {
                    using (var response = await httpClient.GetAsync(APIEndPoint + "SystemUserAccount"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var users = JsonConvert.DeserializeObject<List<SystemUserAccount>>(content);
                            ViewData["UserAccountId"] = new SelectList(users, "SystemUserAccountId", "Email");
                        }
                    }
                }
                catch
                {
                    ViewData["UserAccountId"] = new SelectList(new List<SystemUserAccount>(), "SystemUserAccountId", "Email");
                }
            }
        }
    }
}
