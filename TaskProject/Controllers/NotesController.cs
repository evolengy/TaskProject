using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskProject.Models;

namespace TaskProject.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public NotesController(ApplicationDbContext _db,UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        public async Task<IActionResult> ShowNotes()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                RedirectToAction("Logout", "Account");
            }

            ViewBag.BreadCrumb = "Дневник";
            List<Note> notes = await db.Notes.Where(n => n.UserId == user.Id).ToListAsync();
            return View(notes);
        }

        public IActionResult AddNote(bool today = false)
        {
            ViewBag.BreadCrumb = "Новая заметка";
            ViewBag.TodayNote = today;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNote(Note note)
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                RedirectToAction("Logout", "Account");
            }

            if (note.DateCreate != null)
            {
                if (await db.Notes.AnyAsync(n => n.DateCreate.Value.Date == note.DateCreate.Value.Date && n.UserId == user.Id))
                {
                    ModelState.AddModelError("DateCreate", "Заметка с такой датой уже существует");
                }
            }    

            if (ModelState.IsValid)
            {
                note.User = user;

                if (string.IsNullOrEmpty(note.Theme))
                {
                    note.Theme = "Без названия";
                }

                await db.AddAsync(note);

                await db.Notifications.AddAsync(new Notification()
                {
                    Name = "Добавлена новая запись в дневнике: " + note.Theme,
                    DateCreate = DateTime.Now,
                    UserId = user.Id
                });

                await db.SaveChangesAsync();
                return RedirectToAction("ShowNotes","Notes");
            }

            ViewBag.BreadCrumb = "Новая заметка";
            return View(note);
        }

        public async Task<IActionResult> ShowNote(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await db.Notes.SingleOrDefaultAsync(m => m.NoteId == id);
            if (note == null)
            {
                return NotFound();
            }

            ViewBag.BreadCrumb = "Просмотр заметки";
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowNote(int id, Note note)
        {
            if (id != note.NoteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = await userManager.GetUserAsync(User);
                    note.User = user;

                    if (string.IsNullOrEmpty(note.Theme))
                    {
                        note.Theme = "Без названия";
                    }

                    db.Update(note);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.NoteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("GameRoom","Home");
            }

            ViewBag.BreadCrumb = "Просмотр заметки";
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNote(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await db.Notes.SingleOrDefaultAsync(m => m.NoteId == id);

            if (note == null)
            {
                return NotFound();
            }

            db.Notifications.Add(new Notification()
            {
                Name = "Удалена запись в дневнике: " + note.Theme,
                DateCreate = DateTime.Now,
                UserId = note.UserId
            });

            db.Notes.Remove(note);

            await db.SaveChangesAsync();
            return RedirectToAction("ShowNotes", "Notes");
        }

        private bool NoteExists(int id)
        {
            return db.Notes.Any(e => e.NoteId == id);
        }
    }
}
