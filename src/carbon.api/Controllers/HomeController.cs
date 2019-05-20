using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using carbon.api.Models;
using carbon.core.domain.model;
using carbon.persistence.interfaces;

namespace carbon.api.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        
        public HomeController(IReadOnlyRepository readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
        }
        
        public IActionResult Index()
        {

            var obj = _readOnlyRepository.Table<Test, Guid>().FirstOrDefault();
            
            Console.WriteLine(obj?.Name);
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}