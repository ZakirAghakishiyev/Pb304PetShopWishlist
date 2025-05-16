using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pb304PetShop.DataContext;
using Pb304PetShop.Models;

namespace Pb304PetShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _dbContext.Sliders.ToListAsync();
            var products = await _dbContext.Products.Take(6).ToListAsync();
            var categories = await _dbContext.Categories.ToListAsync();

            var model = new HomeViewModel
            {
                Sliders = sliders,
                Products = products,
                Categories = categories
            };

            return View(model);
        }

        public IActionResult Test()
        {
            Response.Cookies.Append("Test", "Pb304-New",new CookieOptions { Expires = DateTimeOffset.Now.AddHours(1)});

            HttpContext.Session.SetString("Test-Session", "Pb304-Session");

            return NoContent();
        }

        public IActionResult Get()
        {
            var test = Request.Cookies["Test"];
            var testSession = HttpContext.Session.GetString("Test-Session");
            return Json(new { test, testSession });
        }

        [HttpPost]
        public IActionResult ToggleWishlistItem(int id)
        {
            var product = _dbContext.Products.Find(id);

            var wishlistKey = "Wishlist";
            var cookie = Request.Cookies[wishlistKey];

            var wishlist = string.IsNullOrEmpty(cookie)
                ? new List<WishlistCookieItem>()
                : JsonConvert.DeserializeObject<List<WishlistCookieItem>>(cookie);

            var existingItem = wishlist.FirstOrDefault(x => x.ProductId == id);

            if (existingItem != null)
            {
                wishlist.Remove(existingItem);
            }
            else
            {
                wishlist.Add(new WishlistCookieItem { ProductId = id });
            }

            var updatedCookie = JsonConvert.SerializeObject(wishlist);
            Response.Cookies.Append(wishlistKey, updatedCookie, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMonths(900)
            });

            return Ok(new { inWishlist = existingItem == null });
        }

    }
}
