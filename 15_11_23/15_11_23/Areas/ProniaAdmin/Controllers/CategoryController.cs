﻿using Microsoft.AspNetCore.Mvc;

namespace _15_11_23.Areas.ProniaAdmin.Controllers
{
    public class CategoryController : Controller
    {
        [Area("ProniaAdmin")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
