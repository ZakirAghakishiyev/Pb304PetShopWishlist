using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pb304PetShop.DataContext;
using Pb304PetShop.Models;

namespace Pb304PetShop.Controllers
{
    public class WishlistController : Controller
    {

        private readonly AppDbContext _dbContext;

        public WishlistController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var wishlist = GetWishlist();
            var wishlistItemList = new List<WishlistItemViewModel>();

            foreach (var item in wishlist)
            {
                var product = _dbContext.Products.Find(item.ProductId);
                if (product == null) continue;

                wishlistItemList.Add(new WishlistItemViewModel
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    ImageUrl = product.CoverImageUrl,
                    //InStock = product.Stock > 0
                });
            }

            var model = new wishlistViewModel
            {
                wishlistItemViewModels = wishlistItemList
            };

            return View(model);
        }
        private List<WishlistCookieItem> GetWishlist()
        {
            var wishlist = Request.Cookies["Wishlist"];

            if (string.IsNullOrEmpty(wishlist))
                return new List<WishlistCookieItem>();

            var wishlistItems = JsonConvert.DeserializeObject<List<WishlistCookieItem>>(wishlist);

            return wishlistItems ?? new List<WishlistCookieItem>();
        }

        public IActionResult AddToWishlist(int id)
        {
            var product = _dbContext.Products.Find(id);

            var wishlist = GetWishlist();

            if (!wishlist.Any(x => x.ProductId == id))
            {
                wishlist.Add(new WishlistCookieItem { ProductId = id });
            }

            var wishlistJson = JsonConvert.SerializeObject(wishlist);
            Response.Cookies.Append("Wishlist", wishlistJson, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMonths(900)
            });

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromWishlist(int id)
        {
            var wishlist = GetWishlist();

            var item = wishlist.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                wishlist.Remove(item);
                var wishlistJson = JsonConvert.SerializeObject(wishlist);

                Response.Cookies.Append("Wishlist", wishlistJson, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddMonths(900)
                });
            }

            return RedirectToAction("Index");
        }


    }
}
