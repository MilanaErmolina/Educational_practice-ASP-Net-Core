using System.ComponentModel.DataAnnotations;

namespace Shop.Data.Models
{
    public class ShopCartItem
    {
        [Key]
        public int Id { get; set; } 
        public string ShopCartId { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public int Price { get; set; }
    }
}
