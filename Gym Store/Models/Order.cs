namespace Gym_Store.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // IdentityUser.Id
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
