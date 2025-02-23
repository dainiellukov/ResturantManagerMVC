namespace ResturantManagerMVC.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; } = null!;
        public Customer Customer { get; set; } = null!;

        public ICollection<OrderDish> OrderDishes { get; set; } = new List<OrderDish>();

    }
}
