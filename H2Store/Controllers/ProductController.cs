﻿using Azure;
using H2Store.Models;
using H2Store.ViewsModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace H2Store.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly object _productRepository;
        private readonly UserManager<User> _userManager;

        public ProductController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(int? id, int page = 1)
        {
            var currentUser = await _userManager
                 .GetUserAsync(User);
            if(currentUser != null)
            {
                var lish = await _context.ProductLike
                .Include(x => x.Product)
                .Where(x => x.UserId == currentUser.Id).ToListAsync();
                currentUser.WishLish = lish;
            }
            
            page = page < 1 ? 1 : page;
            int pagesize = 20;
            var productsQuery = _context.Products
                .Include(x => x.Comments)
                .Include(x => x.Images).AsQueryable();

            if (id.HasValue)
            {
                productsQuery = productsQuery.Where(x => x.CategoryId == id);
            }

            var products = await productsQuery.Include(x => x.Comments).ToPagedListAsync(page, pagesize);
            var categories = await _context.Categories.ToListAsync();

            var model = new VMHomeP()
            {
                Products = products,
                Categories = categories,
                User = currentUser
            };

            return View(model);
        }


        public async Task<IActionResult> Detail(string id)
        {
            var product = await _context.Products
                .Include(x => x.Images)
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.ProductId == id);

            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> SearchBox(string value, int page = 1)
        {
            var currentUser = await _userManager
                .GetUserAsync(User);
            var lish = await _context.ProductLike
                .Include(x => x.Product)
                .Where(x => x.UserId == currentUser.Id).ToListAsync();
            currentUser.WishLish = lish;
            page = page < 1 ? 1 : page;
            int pageSize = 20;

            // Get the initial paged list of products
            var products = await _context.Products.Include(p => p.Images).ToPagedListAsync(page, pageSize);

            // Apply search filter if searchTerm is provided
            if (!string.IsNullOrEmpty(value))
            {
                var name = value.ToUpper().Trim();
                // Filter products based on the search term
                var filteredProducts = _context.Products
                                               .Include(p => p.Images)
                                               .Where(p => (p.Name.ToUpper().Contains(name)) || (p.Description.ToUpper().Contains(name)));

                // Get the paged list of the filtered products
                products = await filteredProducts.ToPagedListAsync(page, pageSize);
            }
            var categories = await _context.Categories.ToListAsync();
            var model = new VMHomeP()
            {
                Products = products,
                User = currentUser,
                Categories = categories
            };
            // Return the view with the paged list of products
            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReview(int? rating, string? message, string? name, string productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (message == null || name == null) return NotFound();

            var comment = new Comment()
            {
                Email = user.Email,
                Message = message,
                FullName = name,
                Rate = rating,
                CommentDate = DateTime.Now,
                User = user,
                UserID = user.Id,
                Product = product,
                ProductID = productId,
                ParentID = 0
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());

        }

        public async Task<IActionResult> Sorting(int type, int page = 1)
        {
            var currentUser = await _userManager
                .GetUserAsync(User);
            var lish = await _context.ProductLike
                .Include(x => x.Product)
                .Where(x => x.UserId == currentUser.Id).ToListAsync();
            currentUser.WishLish = lish;
            page = page < 1 ? 1 : page;
            int pageSize = 20;
            var productsQuery = _context.Products.Include(p => p.Images).AsQueryable();
            switch (type)
            {
                case 2:
                    productsQuery = productsQuery.OrderBy(p => p.Price);
                    break;
                case 3:
                    productsQuery = productsQuery.OrderByDescending(p => p.Price);
                    break;
                case 4:
                    productsQuery = productsQuery.Where(p => p.Price > 0 && p.Price <= 1000000);
                    break;
                case 5:
                    productsQuery = productsQuery.Where(p => p.Price > 1000000 && p.Price <= 10000000);
                    break;
                case 6:
                    productsQuery = productsQuery.Where(p => p.Price > 10000000 && p.Price <= 20000000);
                    break;
                case 7:
                    productsQuery = productsQuery.Where(p => p.Price > 20000000 && p.Price <= 30000000);
                    break;
                case 8:
                    productsQuery = productsQuery.Where(p => p.Price >= 30000000);
                    break;

                default:
                    // Sắp xếp theo một thứ tự mặc định ở đây nếu cần
                    break;
            }

            var products = await productsQuery.ToPagedListAsync(page, pageSize);
            var categories = await _context.Categories.ToListAsync();

            var model = new VMHomeP()
            {
                Products = products,
                Categories = categories,
                User = currentUser
            };

            return View("Index", model);
        }


    }


}