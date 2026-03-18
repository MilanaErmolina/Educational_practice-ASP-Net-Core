using Shop.Data.Models;

namespace Shop.ViewModels
{
    public class HomeViewModel
    {
        public required IEnumerable<Car> favCars { get; set; }
        public required IEnumerable<Car> allCars { get; set; } // Добавьте
        public string? currCategory { get; set; }
    }
}
