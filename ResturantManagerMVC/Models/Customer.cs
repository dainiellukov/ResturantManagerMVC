using Microsoft.AspNetCore.Identity;

namespace ResturantManagerMVC.Models
{
    public class Customer
    {
        public string CustomerId { get; set; }
        public string FullName { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
