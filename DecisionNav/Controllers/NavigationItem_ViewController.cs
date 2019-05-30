using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DecisionNav.Data;
using DecisionNav.Models;

namespace DecisionNav.Controllers
{
    public class NavigationItem_ViewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NavigationItem_ViewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NavigationItem_View
        public async Task<IActionResult> Index()
        {
            return View(await _context.NavigationItem_View.ToListAsync());
        }

        // GET: NavigationItem_View/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navigationItem_View = await _context.NavigationItem_View
                .FirstOrDefaultAsync(m => m.Id == id);
            if (navigationItem_View == null)
            {
                return NotFound();
            }

            return View(navigationItem_View);
        }

        // GET: NavigationItem_View/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NavigationItem_View/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientID,TopicId,ViewId,RType")] NavigationItem_View navigationItem_View)
        {
            if (ModelState.IsValid)
            {
                _context.Add(navigationItem_View);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(navigationItem_View);
        }

        // GET: NavigationItem_View/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navigationItem_View = await _context.NavigationItem_View.FindAsync(id);
            if (navigationItem_View == null)
            {
                return NotFound();
            }
            return View(navigationItem_View);
        }

        // POST: NavigationItem_View/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientID,TopicId,ViewId,RType")] NavigationItem_View navigationItem_View)
        {
            if (id != navigationItem_View.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(navigationItem_View);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NavigationItem_ViewExists(navigationItem_View.Id))
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
            return View(navigationItem_View);
        }

        // GET: NavigationItem_View/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navigationItem_View = await _context.NavigationItem_View
                .FirstOrDefaultAsync(m => m.Id == id);
            if (navigationItem_View == null)
            {
                return NotFound();
            }

            return View(navigationItem_View);
        }

        // POST: NavigationItem_View/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var navigationItem_View = await _context.NavigationItem_View.FindAsync(id);
            _context.NavigationItem_View.Remove(navigationItem_View);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NavigationItem_ViewExists(int id)
        {
            return _context.NavigationItem_View.Any(e => e.Id == id);
        }
    }
}
