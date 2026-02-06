using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListManager.Data;
using TodoListManager.Models;

namespace TodoListManager.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TasksController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ============================
        // GET: Tasks
        // ============================
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var tasks = await _context.TodoTasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.IsPinned)
                .ThenByDescending(t => t.Priority)
                .ThenBy(t => t.IsCompleted)
                .ToListAsync();

            return View(tasks);
        }

        // ============================
        // GET: Tasks/Details/5
        // ============================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User);

            var todoTask = await _context.TodoTasks
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (todoTask == null) return NotFound();

            return View(todoTask);
        }

        // ============================
        // GET: Tasks/Create
        // ============================
        public IActionResult Create()
        {
            return View();
        }

        // ============================
        // POST: Tasks/Create
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoTask todoTask)
        {
            if (ModelState.IsValid)
            {
                todoTask.UserId = _userManager.GetUserId(User);
                todoTask.CreatedAt = DateTime.Now;

                _context.Add(todoTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(todoTask);
        }

        // ============================
        // GET: Tasks/Edit/5
        // ============================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User);

            var todoTask = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todoTask == null) return NotFound();

            return View(todoTask);
        }

        // ============================
        // POST: Tasks/Edit/5
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TodoTask todoTask)
        {
            if (id != todoTask.Id) return NotFound();

            var userId = _userManager.GetUserId(User);

            var existingTask = await _context.TodoTasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (existingTask == null) return NotFound();

            if (ModelState.IsValid)
            {
                todoTask.UserId = userId;
                todoTask.CreatedAt = existingTask.CreatedAt;

                _context.Update(todoTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(todoTask);
        }

        // ============================
        // GET: Tasks/Delete/5
        // ============================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User);

            var todoTask = await _context.TodoTasks
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (todoTask == null) return NotFound();

            return View(todoTask);
        }

        // ============================
        // POST: Tasks/Delete/5
        // ============================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);

            var todoTask = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todoTask != null)
            {
                _context.TodoTasks.Remove(todoTask);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // POST: Toggle Complete
        // ============================
        [HttpPost]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var userId = _userManager.GetUserId(User);

            var task = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // POST: Toggle Pin
        // ============================
        [HttpPost]
        public async Task<IActionResult> TogglePin(int id)
        {
            var userId = _userManager.GetUserId(User);

            var task = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task != null)
            {
                task.IsPinned = !task.IsPinned;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // Helper
        // ============================
        private bool TodoTaskExists(int id)
        {
            return _context.TodoTasks.Any(e => e.Id == id);
        }
    }
}
