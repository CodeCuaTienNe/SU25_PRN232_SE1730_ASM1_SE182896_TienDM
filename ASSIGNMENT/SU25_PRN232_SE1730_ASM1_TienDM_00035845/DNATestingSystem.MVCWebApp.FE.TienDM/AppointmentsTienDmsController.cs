using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DNATestingSystem.Repository.TienDM.DBContext;
using DNATestingSystem.Repository.TienDM.Models;

namespace DNATestingSystem.MVCWebApp.FE.TienDM
{
    public class AppointmentsTienDmsController : Controller
    {
        private readonly SE18_PRN232_SE1730_G3_DNATestingSystemContext _context;

        public AppointmentsTienDmsController(SE18_PRN232_SE1730_G3_DNATestingSystemContext context)
        {
            _context = context;
        }

        // GET: AppointmentsTienDms
        public async Task<IActionResult> Index()
        {
            var sE18_PRN232_SE1730_G3_DNATestingSystemContext = _context.AppointmentsTienDms.Include(a => a.AppointmentStatusesTienDm).Include(a => a.ServicesNhanVt).Include(a => a.UserAccount);
            return View(await sE18_PRN232_SE1730_G3_DNATestingSystemContext.ToListAsync());
        }

        // GET: AppointmentsTienDms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentsTienDm = await _context.AppointmentsTienDms
                .Include(a => a.AppointmentStatusesTienDm)
                .Include(a => a.ServicesNhanVt)
                .Include(a => a.UserAccount)
                .FirstOrDefaultAsync(m => m.AppointmentsTienDmid == id);
            if (appointmentsTienDm == null)
            {
                return NotFound();
            }

            return View(appointmentsTienDm);
        }

        // GET: AppointmentsTienDms/Create
        public IActionResult Create()
        {
            ViewData["AppointmentStatusesTienDmid"] = new SelectList(_context.AppointmentStatusesTienDms, "AppointmentStatusesTienDmid", "StatusName");
            ViewData["ServicesNhanVtid"] = new SelectList(_context.ServicesNhanVts, "ServicesNhanVtid", "ServiceName");
            ViewData["UserAccountId"] = new SelectList(_context.SystemUserAccounts, "UserAccountId", "Email");
            return View();
        }

        // POST: AppointmentsTienDms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentsTienDmid,UserAccountId,ServicesNhanVtid,AppointmentStatusesTienDmid,AppointmentDate,AppointmentTime,SamplingMethod,Address,ContactPhone,Notes,CreatedDate,ModifiedDate,TotalAmount,IsPaid,ServiceName,StatusName,UserName")] AppointmentsTienDm appointmentsTienDm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointmentsTienDm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppointmentStatusesTienDmid"] = new SelectList(_context.AppointmentStatusesTienDms, "AppointmentStatusesTienDmid", "StatusName", appointmentsTienDm.AppointmentStatusesTienDmid);
            ViewData["ServicesNhanVtid"] = new SelectList(_context.ServicesNhanVts, "ServicesNhanVtid", "ServiceName", appointmentsTienDm.ServicesNhanVtid);
            ViewData["UserAccountId"] = new SelectList(_context.SystemUserAccounts, "UserAccountId", "Email", appointmentsTienDm.UserAccountId);
            return View(appointmentsTienDm);
        }

        // GET: AppointmentsTienDms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentsTienDm = await _context.AppointmentsTienDms.FindAsync(id);
            if (appointmentsTienDm == null)
            {
                return NotFound();
            }
            ViewData["AppointmentStatusesTienDmid"] = new SelectList(_context.AppointmentStatusesTienDms, "AppointmentStatusesTienDmid", "StatusName", appointmentsTienDm.AppointmentStatusesTienDmid);
            ViewData["ServicesNhanVtid"] = new SelectList(_context.ServicesNhanVts, "ServicesNhanVtid", "ServiceName", appointmentsTienDm.ServicesNhanVtid);
            ViewData["UserAccountId"] = new SelectList(_context.SystemUserAccounts, "UserAccountId", "Email", appointmentsTienDm.UserAccountId);
            return View(appointmentsTienDm);
        }

        // POST: AppointmentsTienDms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentsTienDmid,UserAccountId,ServicesNhanVtid,AppointmentStatusesTienDmid,AppointmentDate,AppointmentTime,SamplingMethod,Address,ContactPhone,Notes,CreatedDate,ModifiedDate,TotalAmount,IsPaid,ServiceName,StatusName,UserName")] AppointmentsTienDm appointmentsTienDm)
        {
            if (id != appointmentsTienDm.AppointmentsTienDmid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentsTienDm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentsTienDmExists(appointmentsTienDm.AppointmentsTienDmid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppointmentStatusesTienDmid"] = new SelectList(_context.AppointmentStatusesTienDms, "AppointmentStatusesTienDmid", "StatusName", appointmentsTienDm.AppointmentStatusesTienDmid);
            ViewData["ServicesNhanVtid"] = new SelectList(_context.ServicesNhanVts, "ServicesNhanVtid", "ServiceName", appointmentsTienDm.ServicesNhanVtid);
            ViewData["UserAccountId"] = new SelectList(_context.SystemUserAccounts, "UserAccountId", "Email", appointmentsTienDm.UserAccountId);
            return View(appointmentsTienDm);
        }

        // GET: AppointmentsTienDms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentsTienDm = await _context.AppointmentsTienDms
                .Include(a => a.AppointmentStatusesTienDm)
                .Include(a => a.ServicesNhanVt)
                .Include(a => a.UserAccount)
                .FirstOrDefaultAsync(m => m.AppointmentsTienDmid == id);
            if (appointmentsTienDm == null)
            {
                return NotFound();
            }

            return View(appointmentsTienDm);
        }

        // POST: AppointmentsTienDms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentsTienDm = await _context.AppointmentsTienDms.FindAsync(id);
            if (appointmentsTienDm != null)
            {
                _context.AppointmentsTienDms.Remove(appointmentsTienDm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentsTienDmExists(int id)
        {
            return _context.AppointmentsTienDms.Any(e => e.AppointmentsTienDmid == id);
        }
    }
}
