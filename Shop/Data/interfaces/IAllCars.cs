using Shop.Data.Models;

namespace Shop.Data.interfaces
{
    public interface IAllCars
    {
        IEnumerable<Car> Cars { get; }
        IEnumerable<Car> getFavCars { get; }
        Car getObjectCar(int carId);
        void AddCar(Car car);
        void UpdateCar(Car car);
        void DeleteCar(int carId);
    }
}