﻿using System;
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

        public IActionResult ShowNotes()
        {

            ViewBag.BreadCrumb = "Дневник";
            return View(new NoteViewModel());
        }

        public async Task<ActionResult> GetNotes(DateTime start, DateTime end)
        {
            var noteModel = new NoteViewModel();

            var noteEvents = new List<NoteViewModel>();

            ApplicationUser user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return View("Index");
            }

            List<Note> notes = await db.Note.Where(n => n.UserId == user.Id).Where(n => n.DateCreate >= start && n.DateCreate <=  end).ToListAsync();

            foreach (Note note in notes)
            {
                noteEvents.Add(
                    new NoteViewModel
                    {
                        id = note.NoteId,
                        title = "Тема:" + note.Theme,
                        start = note.DateCreate.ToString("s"),
                        end = note.DateCreate.ToString("s"),
                        allDay = true
                    }
                    );
            }
            return Json(noteEvents.ToArray());
        }

        public IActionResult AddNote()
        {
            ViewBag.BreadCrumb = "Новая заметка";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNote(Note note)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.GetUserAsync(User);

                if (user == null)
                {
                    return View("Index");
                }

                note.DateCreate = DateTime.Now.Date;
                note.User = user;

                if (string.IsNullOrEmpty(note.Theme))
                {
                    note.Theme = "Без названия";
                }

                db.Add(note);
                await db.SaveChangesAsync();
                return RedirectToAction("GameRoom","Home");
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

            var note = await db.Note.SingleOrDefaultAsync(m => m.NoteId == id);
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

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await db.Note
                .Include(n => n.User)
                .SingleOrDefaultAsync(m => m.NoteId == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await db.Note.SingleOrDefaultAsync(m => m.NoteId == id);
            db.Note.Remove(note);
            await db.SaveChangesAsync();
            return RedirectToAction("GameRoom", "Home");
        }

        private bool NoteExists(int id)
        {
            return db.Note.Any(e => e.NoteId == id);
        }
    }
}