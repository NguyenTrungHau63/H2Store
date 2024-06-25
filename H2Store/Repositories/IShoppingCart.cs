using H2Store.Models;
using Microsoft.AspNetCore.Mvc;

namespace H2Store.Repositories
{
    public interface IShoppingCart
    {
        Task<List<Product>> GetCartItemsAsync(string userId);
        Task UpdateCartItemQuantityAsync(string userId, string productId, int quantity);
        Task<int> RemoveFromCartAsync(string userId, string productId);
    }
}
