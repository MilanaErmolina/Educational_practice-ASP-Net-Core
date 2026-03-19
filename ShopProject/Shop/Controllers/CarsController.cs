
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Data.interfaces;
using Shop.Data.Models;
using Shop.Data.Repository;
using Shop.ViewModels;

namespace Shop.Controllers
{
    public class CarsController : Controller
    {
        private readonly IAllCars _allCars;
        private readonly ICarsCategory _allCategories;

        public CarsController(IAllCars iAllCars, ICarsCategory iCarsCat)
        {
            _allCars = iAllCars;
            _allCategories = iCarsCat;
        }

        [Route("Cars/List")]
        [Route("Cars/List/{category}")]
        public ViewResult List(string category)
        {
            IEnumerable<Car> cars;
            string currCategory;

            if (string.IsNullOrEmpty(category))
            {
                cars = _allCars.Cars.OrderBy(i => i.id);
                currCategory = "Все автомобили";
            }
            else if (string.Equals("electro", category, StringComparison.OrdinalIgnoreCase))
            {
                cars = _allCars.Cars.Where(i => i.Category.categoryName.Equals("Электромобили")).OrderBy(i => i.id);
                currCategory = "Электромобили";
            }
            else if (string.Equals("fuel", category, StringComparison.OrdinalIgnoreCase))
            {
                cars = _allCars.Cars.Where(i => i.Category.categoryName.Equals("Классические автомобили")).OrderBy(i => i.id);
                currCategory = "Классические автомобили";
            }
            else
            {
                cars = Enumerable.Empty<Car>();
                currCategory = "Категория не найдена";
            }

            var carObj = new CarsListViewModel
            {
                allCars = cars,
                currCategory = currCategory
            };

            ViewBag.Title = "Страница с автомобилями";
            return View(carObj);
        }

        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return RedirectToAction("List");

            var lowerQuery = query.ToLower();

            var cars = _allCars.Cars
                .Where(c => c.name.ToLower().Contains(lowerQuery)
                         || c.shortDesc.ToLower().Contains(lowerQuery)
                         || c.longDesc.ToLower().Contains(lowerQuery))
                .ToList();

            var model = new CarsListViewModel
            {
                allCars = cars,
                currCategory = "Результаты поиска"
            };

            ViewBag.Title = "Поиск";
            return View("List", model); 
        }
    }
}
