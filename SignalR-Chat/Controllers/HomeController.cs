﻿using Microsoft.AspNetCore.Mvc;
using SignalR_Chat.Models;
using System.Diagnostics;

namespace SignalR_Chat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetClientId([FromBody] ClientInfo model)
        {
            return Ok(new
            {
                UserId = Guid.NewGuid().ToString(),
                UserName = model.UserName
            });
        }

        public IActionResult GetLoadingView()
        {
            return PartialView("_Loading");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}