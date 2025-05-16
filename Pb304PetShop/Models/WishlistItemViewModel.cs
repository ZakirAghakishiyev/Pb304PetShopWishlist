namespace Pb304PetShop.Models
{
    public class WishlistItemViewModel
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        //public bool InStock{ get; set; }
    }

    public class wishlistViewModel
    {
        public List<WishlistItemViewModel> wishlistItemViewModels { get; set; }
    }
}
