using Pb304PetShop.DataContext.Entities;

namespace Pb304PetShop.Models
{
    public class ShopViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
