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
using DNATestingSystem.Repository.TienDM.ModelExtensions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.Identity.Client;

namespace DNATestingSystem.MVCWebApp.FE.TienDM.Controllers
{
    [Authorize]
    public class AppointmentsTienDmsController : Controller
    {
        private string APIEndPoint = "http://localhost:8080/api/";

        public AppointmentsTienDmsController() { }
        // GET: AppointmentsTienDms (phân trang + search)
        public async Task<IActionResult> Index(int page = 1, int pageSize = 3, int searchId = 0, string searchContactPhone = "", decimal searchTotalAmount = 0)
        {
            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                if (!string.IsNullOrEmpty(tokenString))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                }

                // Chuẩn bị search request
                var searchRequest = new
                {
                    AppointmentsTienDmid = searchId > 0 ? searchId : (int?)null,
                    ContactPhone = string.IsNullOrWhiteSpace(searchContactPhone) ? null : searchContactPhone,
                    TotalAmount = searchTotalAmount > 0 ? searchTotalAmount : (decimal?)null,
                    CurrentPage = page,
                    PageSize = pageSize
                };
                var json = JsonConvert.SerializeObject(searchRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(APIEndPoint + "AppointmentsTienDM/search", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        // Deserialize JSON bằng model đặc biệt cho API để giữ nguyên tên thuộc tính
                        var apiResponse = JsonConvert.DeserializeObject<ApiPaginationResult<List<ApiAppointmentDTO>>>(responseContent);

                        // Chuyển đổi giữa các mô hình cho frontend
                        var paginationResult = new DNATestingSystem.MVCWebApp.FE.TienDM.Models.PaginationResult<List<AppointmentsTienDm>>
                        {
                            TotalItems = apiResponse?.totalItems ?? 0,
                            TotalPages = apiResponse?.totalPages ?? 1,
                            CurrentPages = apiResponse?.currentPages ?? 1,
                            PageSize = apiResponse?.pageSize ?? pageSize,
                            Items = apiResponse?.items?.Select(a => new AppointmentsTienDm
                            {
                                AppointmentsTienDmid = a.appointmentsTienDmid,
                                UserAccountId = a.userAccountId,
                                ServicesNhanVtid = a.servicesNhanVtid,
                                AppointmentStatusesTienDmid = a.appointmentStatusesTienDmid,
                                AppointmentDate = DateOnly.Parse(a.appointmentDate),
                                AppointmentTime = TimeOnly.Parse(a.appointmentTime),
                                SamplingMethod = a.samplingMethod,
                                Address = a.address,
                                ContactPhone = a.contactPhone,
                                Notes = a.notes,
                                CreatedDate = a.createdDate,
                                ModifiedDate = a.modifiedDate,
                                TotalAmount = a.totalAmount,
                                IsPaid = a.isPaid,
                                // Thêm thuộc tính tạm thời để giữ giá trị từ API
                                UserAccount = new SystemUserAccount { UserName = a.username },
                                ServicesNhanVt = new ServicesNhanVt { ServiceName = a.serviceName },
                                AppointmentStatusesTienDm = new AppointmentStatusesTienDm { StatusName = a.statusName }
                            }).ToList() ?? new List<AppointmentsTienDm>()
                        };
                        ViewBag.TotalPages = paginationResult?.TotalPages ?? 1;
                        ViewBag.CurrentPage = paginationResult?.CurrentPages ?? 1;
                        ViewBag.PageSize = paginationResult?.PageSize ?? pageSize;
                        ViewBag.TotalItems = paginationResult?.TotalItems ?? 0;
                        ViewBag.SearchId = searchId;
                        ViewBag.SearchContactPhone = searchContactPhone;
                        ViewBag.SearchTotalAmount = searchTotalAmount;

                        // Chuyển đổi từ các giá trị đã được lấy từ API sang AppointmentsTienDmDto
                        var dtoList = new List<DNATestingSystem.Repository.TienDM.ModelExtensions.AppointmentsTienDmDto>();

                        foreach (var a in paginationResult?.Items ?? new List<AppointmentsTienDm>())
                        {
                            dtoList.Add(new DNATestingSystem.Repository.TienDM.ModelExtensions.AppointmentsTienDmDto
                            {
                                AppointmentsTienDmid = a.AppointmentsTienDmid,
                                UserAccountId = a.UserAccountId,
                                // Lấy giá trị từ các đối tượng đã được khởi tạo với dữ liệu từ API
                                Username = a.UserAccount?.UserName,
                                ServicesNhanVtid = a.ServicesNhanVtid,
                                ServiceName = a.ServicesNhanVt?.ServiceName,
                                AppointmentStatusesTienDmid = a.AppointmentStatusesTienDmid,
                                StatusName = a.AppointmentStatusesTienDm?.StatusName,
                                AppointmentDate = a.AppointmentDate,
                                AppointmentTime = a.AppointmentTime,
                                SamplingMethod = a.SamplingMethod,
                                Address = a.Address,
                                ContactPhone = a.ContactPhone,
                                Notes = a.Notes,
                                CreatedDate = a.CreatedDate,
                                ModifiedDate = a.ModifiedDate,
                                TotalAmount = a.TotalAmount,
                                IsPaid = a.IsPaid
                            });
                        }

                        return View(dtoList ?? new List<DNATestingSystem.Repository.TienDM.ModelExtensions.AppointmentsTienDmDto>());
                    }
                }
            }
            ViewBag.TotalPages = 1;
            ViewBag.CurrentPage = 1;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = 0;
            ViewBag.SearchId = searchId;
            ViewBag.SearchContactPhone = searchContactPhone;
            ViewBag.SearchTotalAmount = searchTotalAmount;
            return View(new List<DNATestingSystem.Repository.TienDM.ModelExtensions.AppointmentsTienDmDto>());
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
            Console.WriteLine($"SamplingMethod: {appointmentsTienDm.SamplingMethod}");
            Console.WriteLine($"ContactPhone: {appointmentsTienDm.ContactPhone}");
            Console.WriteLine($"TotalAmount: {appointmentsTienDm.TotalAmount}");
            Console.WriteLine($"ServicesNhanVtid: {appointmentsTienDm.ServicesNhanVtid}");
            Console.WriteLine($"AppointmentStatusesTienDmid: {appointmentsTienDm.AppointmentStatusesTienDmid}");

            // Remove validation errors for navigation properties
            ModelState.Remove("UserAccount");
            ModelState.Remove("ServicesNhanVt");
            ModelState.Remove("AppointmentStatusesTienDm");
            ModelState.Remove("SampleThinhLcs");

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
                if (string.IsNullOrEmpty(tokenString))
                {
                    return RedirectToAction("Login", "Account");
                }

                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                using (var response = await httpClient.GetAsync(APIEndPoint + $"AppointmentsTienDM/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        // First deserialize to ApiAppointmentDTO to ensure property names match the API
                        var apiAppointment = JsonConvert.DeserializeObject<ApiAppointmentDTO>(content);

                        if (apiAppointment == null)
                        {
                            return NotFound();
                        }

                        // Then map to the AppointmentsTienDm model
                        var appointment = new AppointmentsTienDm
                        {
                            AppointmentsTienDmid = apiAppointment.appointmentsTienDmid,
                            UserAccountId = apiAppointment.userAccountId,
                            ServicesNhanVtid = apiAppointment.servicesNhanVtid,
                            AppointmentStatusesTienDmid = apiAppointment.appointmentStatusesTienDmid,
                            AppointmentDate = DateOnly.Parse(apiAppointment.appointmentDate),
                            AppointmentTime = TimeOnly.Parse(apiAppointment.appointmentTime),
                            SamplingMethod = apiAppointment.samplingMethod, // This ensures the sampling method is correctly set
                            Address = apiAppointment.address,
                            ContactPhone = apiAppointment.contactPhone,
                            Notes = apiAppointment.notes,
                            CreatedDate = apiAppointment.createdDate,
                            ModifiedDate = apiAppointment.modifiedDate,
                            TotalAmount = apiAppointment.totalAmount,
                            IsPaid = apiAppointment.isPaid,
                            // Set navigation properties for display
                            UserAccount = new SystemUserAccount { UserName = apiAppointment.username },
                            ServicesNhanVt = new ServicesNhanVt { ServiceName = apiAppointment.serviceName },
                            AppointmentStatusesTienDm = new AppointmentStatusesTienDm { StatusName = apiAppointment.statusName }
                        };

                        // Log the sampling method for debugging
                        Console.WriteLine($"Loaded appointment with SamplingMethod: {appointment.SamplingMethod}");

                        // Debug the sampling method binding
                        DebugSelectBinding("SamplingMethod", appointment.SamplingMethod);

                        await LoadDropdownsAsync();

                        // Pass the current sampling method to ViewBag for the view
                        ViewBag.CurrentSamplingMethod = appointment.SamplingMethod;
                        return View(appointment);
                    }
                    else
                    {
                        // Log the error
                        Console.WriteLine($"API Error: {response.StatusCode}, {response.ReasonPhrase}");
                        return NotFound($"API Error: {response.StatusCode}");
                    }
                }
            }
        }        // POST: AppointmentsTienDms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentsTienDmid,AppointmentDate,AppointmentTime,SamplingMethod,Address,ContactPhone,Notes,CreatedDate,TotalAmount,IsPaid,AppointmentStatusesTienDmid,ServicesNhanVtid,UserAccountId")] AppointmentsTienDm appointmentsTienDm)
        {
            if (id != appointmentsTienDm.AppointmentsTienDmid)
            {
                return NotFound();
            }

            // Log the sampling method for debugging
            Console.WriteLine($"Form submitted with SamplingMethod: {appointmentsTienDm.SamplingMethod}");

            // Remove validation errors for navigation properties
            ModelState.Remove("UserAccount");
            ModelState.Remove("ServicesNhanVt");
            ModelState.Remove("AppointmentStatusesTienDm");
            ModelState.Remove("SampleThinhLcs");

            // Debug ModelState errors if any
            if (!ModelState.IsValid)
            {
                Console.WriteLine("=== EDIT MODEL STATE ERRORS ===");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            if (ModelState.IsValid)
            {
                // API Call Implementation
                using (var httpClient = new HttpClient())
                {
                    var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                    if (string.IsNullOrEmpty(tokenString))
                    {
                        ModelState.AddModelError("", "Authentication token not found. Please login again.");
                        await LoadDropdownsAsync();
                        return View(appointmentsTienDm);
                    }

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                    appointmentsTienDm.ModifiedDate = DateTime.Now;

                    // Enhanced JSON serialization with custom converters for DateOnly and TimeOnly
                    var json = JsonConvert.SerializeObject(appointmentsTienDm, new JsonSerializerSettings
                    {
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        Converters = new List<JsonConverter>
                        {
                            new Converters.DateOnlyConverter(),
                            new Converters.TimeOnlyConverter()
                        }
                    });

                    Console.WriteLine($"Serialized appointment for update: {json}");
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

        public IActionResult AppointmentsTienDmList()
        {
            return View();
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

                // Create a list of sampling methods for dropdown
                var samplingMethods = new List<string>
                {
                    "Buccal Swab",
                    "Blood Sample",
                    "Saliva Collection",
                    "Hair Follicle",
                    "Nail Clipping",
                    "Tissue Sample",
                    "Amniotic Fluid",
                    "Chorionic Villus",
                    "Home Visit"
                };

                // Add this data to ViewBag for debugging
                ViewBag.SamplingMethods = samplingMethods;
            }
        }

        // Helper method to debug select list binding
        private void DebugSelectBinding(string propertyName, string propertyValue)
        {
            Console.WriteLine($"Debugging select binding for {propertyName}: Value = '{propertyValue}'");

            if (propertyName == "SamplingMethod")
            {
                var samplingMethods = new List<string>
                {
                    "Buccal Swab",
                    "Blood Sample",
                    "Saliva Collection",
                    "Hair Follicle",
                    "Nail Clipping",
                    "Tissue Sample",
                    "Amniotic Fluid",
                    "Chorionic Villus",
                    "Home Visit"
                };

                if (string.IsNullOrEmpty(propertyValue))
                {
                    Console.WriteLine("SamplingMethod is null or empty");
                }
                else if (samplingMethods.Contains(propertyValue))
                {
                    Console.WriteLine($"SamplingMethod '{propertyValue}' is in the list of valid options");
                }
                else
                {
                    Console.WriteLine($"SamplingMethod '{propertyValue}' is NOT in the list of valid options");
                    Console.WriteLine($"Valid options: {string.Join(", ", samplingMethods)}");
                }
            }
        }
    }
}
