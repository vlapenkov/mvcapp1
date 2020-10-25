﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using mvcapp.Models;

namespace mvcapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
           
           var valuesSection = _configuration.GetSection("Menu:Items");
            foreach (IConfigurationSection section in valuesSection.GetChildren())
            {
                var key = section.GetValue<string>("Key");
                var value = section.GetValue<string>("Value");
                _logger.LogInformation($"key: {key} value:{value}");
            }
            _logger.LogWarning($"Some warning from {nameof(HomeController)}");
            return View();
        }

        public IActionResult Privacy()
        {
            throw new NullReferenceException("some exception message");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Restricted()
        {
            return View();
        }

        public IActionResult Logout()
        {
            // return new SignOutResult(new[] { "oidc", "Cookies" });
            return SignOut("Cookies", "oidc");
        }
    }
}
