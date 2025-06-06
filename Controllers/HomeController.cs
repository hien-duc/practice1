using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice1.Data;
using practice1.Models;

namespace practice1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var carousels = _context.Carousels
    .Where(c => c.IsActive)
    .OrderBy(c => c.Order)
    .ToList();
            var topLowStockProducts = await _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.StockQuantity)
                .Take(5)
                .ToListAsync();
            ViewBag.TopLowStockProducts = topLowStockProducts;
            ViewBag.Carousels = carousels;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
