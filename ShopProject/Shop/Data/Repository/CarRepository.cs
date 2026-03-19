using Microsoft.EntityFrameworkCore;
using Shop.Data.interfaces;
using Shop.Data.Models;

namespace Shop.Data.Repository
{
    public class CarRepository : IAllCars
    {
        private readonly AppDBContent appDBContent;

        public CarRepository(AppDBContent appDBContent)
        {
            this.appDBContent = appDBContent;
        }

        public IEnumerable<Car> Cars => appDBContent.Car.Include(c => c.Category);

        public IEnumerable<Car> getFavCars => appDBContent.Car.Where(p => p.isFavourite).Include(c => c.Category);

        public Car getObjectCar(int carId) => appDBContent.Car.FirstOrDefault(p => p.id == carId);

        public void AddCar(Car car)
        {
            try
            {
                appDBContent.Car.Add(car);
                appDBContent.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении: {ex.Message}");
                throw; 
            }
        }

        public void UpdateCar(Car car)
        {
            appDBContent.SaveChanges();
        }

        public void DeleteCar(int carId)
        {
            var car = appDBContent.Car.Find(carId);
            if (car != null)
            {
                appDBContent.Car.Remove(car);
                appDBContent.SaveChanges();
            }
        }
    }
}
