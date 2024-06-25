using H2Store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace H2Store.Controllers
{
    public class WishLishController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public WishLishController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            var wishlish = await _context.ProductLike
                .Include(x => x.Product)
                .Where(x => x.UserId == user.Id).ToListAsync();
            return View(wishlish);
        }

        public async Task<IActionResult> RemoveWishLish(string id)
        {
            var wish = await _context.ProductLike.FirstOrDefaultAsync(x => x.ProductId == id);
            if (wish != null)
            {
                _context.ProductLike.Remove(wish);
                await _context.SaveChangesAsync();
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> AddWishList(string id)
        {
            // Ensure the product exists
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (product == null)
            {
                // Handle the case where the product does not exist
                return NotFound();
            }

            // Ensure the user is authenticated and exists
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Check if the product is already in the wishlist
            var existingWish = await _context.ProductLike
                .FirstOrDefaultAsync(x => x.ProductId == id && x.UserId == user.Id);

            if (existingWish != null)
            {
                // Handle the case where the product is already in the wishlist
                // Optionally, you could inform the user that the product is already in their wishlist
                return RedirectToAction("Index");
            }

            // Add the product to the wishlist
            var wishlish = new ProductLike()
            {
                Product = product,
                ProductId = product.ProductId,
                User = user,
                UserId = user.Id
            };
            _context.ProductLike.Add(wishlish);
            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());
        }


    }
}
