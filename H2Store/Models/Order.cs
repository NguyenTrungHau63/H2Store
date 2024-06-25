using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace H2Store.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public Order()
        {
            OrderDate = DateTime.Now;

        }
        public decimal TotalPrice { get; set; }
        public string ShippingAddress { get; set; }
        public string Notes { get; set; }
        public User? User { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
