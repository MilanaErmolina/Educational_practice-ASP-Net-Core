using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Data.interfaces;
using Shop.Data.Models;
using System;
using System.IO;
using System.Linq;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CarsController : Controller
    {
        private readonly IAllCars _carRepository;
        private readonly ICarsCategory _categoryRepository;

        public CarsController(IAllCars carRepository, ICarsCategory categoryRepository)
        {
            _carRepository = carRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var cars = _carRepository.Cars.ToList();
            return View(cars);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryRepository.AllCategories, "id", "categoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Car car, IFormFile uploadedFile)
        {
            ModelState.Remove("img");
            ModelState.Remove("Category");
            ModelState.Remove("uploadedFile");

            if (ModelState.IsValid)
            {
                if (uploadedFile != null && uploadedFile.Length > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName)
                                      + "_" + Guid.NewGuid().ToString().Substring(0, 8)
                                      + Path.GetExtension(uploadedFile.FileName);
                    string path = "/img/" + fileName;
                    string fullPath = Directory.GetCurrentDirectory() + "/wwwroot" + path;

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        uploadedFile.CopyTo(fileStream);
                    }
                    car.img = path;
                }
                else
                {
                    car.img = "/img/no-image.jpg";
                }

                _carRepository.AddCar(car);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_categoryRepository.AllCategories, "id", "categoryName", car.categoryID);
            return View(car);
        }

        public IActionResult Edit(int id)
        {
            var car = _carRepository.getObjectCar(id);
            if (car == null) return NotFound();

            ViewBag.Categories = new SelectList(_categoryRepository.AllCategories, "id", "categoryName", car.categoryID);
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Car car, IFormFile uploadedFile)
        {
            if (id != car.id)
                return NotFound();

            ModelState.Remove("Category");
            ModelState.Remove("uploadedFile");

            if (ModelState.IsValid)
            {
                var existingCar = _carRepository.getObjectCar(id);
                if (existingCar == null)
                    return NotFound();

                existingCar.name = car.name;
                existingCar.shortDesc = car.shortDesc;
                existingCar.longDesc = car.longDesc;
                existingCar.price = car.price;
                existingCar.categoryID = car.categoryID;
                existingCar.isFavourite = car.isFavourite;
                existingCar.available = car.available;

                if (uploadedFile != null && uploadedFile.Length > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName)
                                      + "_" + Guid.NewGuid().ToString().Substring(0, 8)
                                      + Path.GetExtension(uploadedFile.FileName);
                    string path = "/img/" + fileName;
                    string fullPath = Directory.GetCurrentDirectory() + "/wwwroot" + path;

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        uploadedFile.CopyTo(fileStream);
                    }
                    existingCar.img = path;
                }

                _carRepository.UpdateCar(existingCar);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_categoryRepository.AllCategories, "id", "categoryName", car.categoryID);
            return View(car);
        }

        public IActionResult Delete(int id)
        {
            var car = _carRepository.getObjectCar(id);
            if (car == null) return NotFound();
            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _carRepository.DeleteCar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}