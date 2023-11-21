﻿using _15_11_23.DAL;
using Microsoft.AspNetCore.Mvc;

namespace _15_11_23.Areas.ProniaAdmin.Controllers
{
    public class ColorsController : Controller
    {
        private readonly AppDbContext _context;

        public ColorsController(AppDbContext context)
        {
            _context = context;
        }
        [Area("ProniaAdmin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            return View();
        }
    }
}
