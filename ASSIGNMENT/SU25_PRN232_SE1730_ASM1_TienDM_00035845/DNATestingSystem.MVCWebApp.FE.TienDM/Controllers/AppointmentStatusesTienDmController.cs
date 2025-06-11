using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DNATestingSystem.Repository.TienDM.Models;
using System.Text.Json;
using System.Text;

namespace DNATestingSystem.MVCWebApp.FE.TienDM.Controllers
{
    public class AppointmentStatusesTienDmsController : Controller
    {
        private readonly string APIEndPoint = "http://localhost:8080/api/";

        /// <summary>
        /// Main list view with Ajax functionality
        /// </summary>
        public IActionResult AppointmentStatusesTienDmList()
        {
            return View();
        }

        /// <summary>
        /// Create view
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Edit view
        /// </summary>
        public IActionResult Edit(int id)
        {
            return View();
        }

        /// <summary>
        /// Details view
        /// </summary>
        public IActionResult Details(int id)
        {
            return View();
        }

        /// <summary>
        /// Delete view
        /// </summary>
        public IActionResult Delete(int id)
        {
            return View();
        }

        /// <summary>
        /// Confirm Delete Popup (Partial View)
        /// </summary>
        public IActionResult ConfirmDeletePopup()
        {
            return PartialView("_ConfirmDeletePopup");
        }

        /// <summary>
        /// View Detail Popup (Partial View)
        /// </summary>
        public IActionResult ViewDetailPopup()
        {
            return PartialView("_ViewDetailPopup");
        }

        /// <summary>
        /// API call to create appointment status
        /// </summary>
        [HttpPost]
        [Route("AppointmentStatusesTienDms/CreateStatus")]
        public async Task<IActionResult> CreateStatus([FromBody] AppointmentStatusesTienDm model)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var json = JsonSerializer.Serialize(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(APIEndPoint + "AppointmentStatusesTienDM", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "Status created successfully" });
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return Json(new { success = false, message = "Failed to create status: " + errorContent });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// API call to update appointment status
        /// </summary>
        [HttpPost]
        [Route("AppointmentStatusesTienDms/UpdateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] AppointmentStatusesTienDm model)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var json = JsonSerializer.Serialize(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PutAsync(APIEndPoint + $"AppointmentStatusesTienDM/{model.AppointmentStatusesTienDmid}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "Status updated successfully" });
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return Json(new { success = false, message = "Failed to update status: " + errorContent });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// API call to delete appointment status
        /// </summary>
        [HttpPost]
        [Route("AppointmentStatusesTienDms/DeleteStatus/{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.DeleteAsync(APIEndPoint + $"AppointmentStatusesTienDM/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "Status deleted successfully" });
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return Json(new { success = false, message = "Failed to delete status: " + errorContent });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
