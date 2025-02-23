namespace ResturantManagerMVC.Models
{
    public class OrderDish
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid DishId { get; set; }
        public Dish Dish { get; set; } = null!;
    }
}
