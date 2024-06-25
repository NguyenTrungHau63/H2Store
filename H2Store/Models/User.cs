using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace H2Store.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }

        public List<CartItem>? CartItem { get; set; }
        public List<Order>? Order { get; set; }

        public IEnumerable<ProductLike> WishLish { get; set; }
    }
}
