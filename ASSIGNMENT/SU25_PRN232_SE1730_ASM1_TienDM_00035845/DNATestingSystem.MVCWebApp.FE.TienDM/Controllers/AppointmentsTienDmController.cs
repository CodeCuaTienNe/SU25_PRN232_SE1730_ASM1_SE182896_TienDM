using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DNATestingSystem.MVCWebApp.FE.TienDM.Controllers
{
    public class AppointmentsTienDmController : Controller
    {
        // GET: AppointmentsTienDmController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AppointmentsTienDmController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AppointmentsTienDmController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppointmentsTienDmController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AppointmentsTienDmController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AppointmentsTienDmController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AppointmentsTienDmController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AppointmentsTienDmController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        //add to authorize role 1 and 2
        //var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
        ////tokenString = HttpContext.Request.Cookies["TokenString"];

        //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
    }
}
