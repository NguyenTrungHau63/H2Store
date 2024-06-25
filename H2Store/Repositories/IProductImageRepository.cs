using H2Store.Models;

namespace H2Store.Repositories
{
    public interface IProductImageRepository
    {
        Task AddAsync(ProductImage productImage);
        Task UpdateAsync(ProductImage productImage);
        Task<ProductImage> GetByProductIdAsync(string productId);
    }
}
