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
    public class AtributesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> usermanager;

        public AtributesController(ApplicationDbContext _db, UserManager<ApplicationUser> _usermanager)
        {
            db = _db;
            usermanager = _usermanager;
        }

        public async Task<IActionResult> GetAtributes()
        {
            var user = await usermanager.GetUserAsync(User);
            var atributes = await db.Atributes.Where(a => a.UserId == user.Id).ToListAsync();

            ViewBag.BreadCrumb = "Характеристики";

            return View(atributes);
        }

    }
}
