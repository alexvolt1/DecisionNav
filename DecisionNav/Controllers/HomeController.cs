using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DecisionNav.Models;
using System.Data.SqlClient;
using System.Text;
using DecisionNav.Models.ViewModels;

namespace DecisionNav.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //ViewData["Menu"] = GetAllMenuItems();
            //return View();
            Test();
            return View ();
        }

        public IActionResult Test()
        {
            //ViewData["Menu"] = GetAllMenuItems();
            //return View();
            // var menuItems = GetAllMenuItems();
            // return View(GetMenu(menuItems, null));
            //return PartialView("_MenuPartial", GetMenu(menuItems, null));

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public async Task<IActionResult> GetNavBar()
        public IActionResult GetNavBar()
        {
            return View();
        }


    }
}
