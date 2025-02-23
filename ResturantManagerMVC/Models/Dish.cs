namespace ResturantManagerMVC.Models
{
    public class Dish
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public ICollection<OrderDish> OrderDishes { get; set; } = new List<OrderDish>();
    }
}
