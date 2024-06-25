using H2Store.Areas.Admin.Models;
using H2Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace H2Store.Areas.Admin.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueCategoryAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RevenueCategoryAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("order-count")]
        public async Task<ActionResult<IEnumerable<CategoryOrderCount>>> GetOrderCountByCategory()
        {
            var orderCounts = await _context.Categories
                .Select(category => new CategoryOrderCount
                {
                    CategoryName = category.Name,
                    OrderCount = _context.Orders
                        .Where(order => order.OrderDetails.Any(detail => detail.Product.CategoryId == category.Id))
                        .Count()
                })
                .ToListAsync();

            return orderCounts;
        }

    }
}
