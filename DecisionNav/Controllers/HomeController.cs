using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DecisionNav.Models;
using DecisionNav.Models.NavBarModel;

namespace DecisionNav.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
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

            AvailWeek availWeek1 = new AvailWeek()
            {
                Id = 1,
                Name = "Name1",
                Description = "Description1",
                Image = "Image1",
                Spicyness = "Spicyness1",
                Price = 2,
                CategoryId=1,
                SubCategoryId=11
            };
            AvailWeek availWeek2 = new AvailWeek()
            {
                Id = 2,
                Name = "Name2",
                Description = "Description2",
                Image = "Image2",
                Spicyness = "Spicyness2",
                Price = 3,
                CategoryId = 2,
                SubCategoryId = 22
            };
            SubCategory subCategory1 = new SubCategory()
            {
                Id = 1,
                Name = "Name1",
                AgeFrom = 5,
                AgeTo = 7,
                CategoryId = 1
            };
            SubCategory subCategory2 = new SubCategory()
            {
                Id = 2,
                Name = "Name2",
                AgeFrom = 9,
                AgeTo = 12,
                CategoryId = 2
            };
            Category category1 = new Category()
            {
                Id = 1,
                Name = "Name1",
                DisplayOrder=1
            };

            Category category2 = new Category()
            {
                Id = 2,
                Name = "Name2",
                DisplayOrder=2
            };

            //IEnumerable<AvailWeek> ienumAvailWeek = new List<AvailWeek>() { availWeek };


            NavBarModel NavBarVM = new NavBarModel()
            {
                //AvailWeek = await _db.AvailWeek.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync(),
                //Category = _db.Category.OrderBy(c => c.DisplayOrder),
                //SubCategory = _db.SubCategory.Where(c => c.IsAvailable).ToList(),
                //Coupons = _db.Coupons.Where(c => c.isActive == true).ToList()

                AvailWeek = new List<AvailWeek>() { availWeek1, availWeek2 },
                Category = new List<Category>() { category1, category2 },
                SubCategory = new List<SubCategory>() { subCategory1, subCategory2 }

            };
            return View(NavBarVM);
        }

    }
}
