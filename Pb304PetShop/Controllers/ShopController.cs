using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pb304PetShop.DataContext;


namespace Pb304PetShop.Controllers;

public class ShopController : Controller
{
    private readonly AppDbContext _context;

    public ShopController(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }
    public async Task<IActionResult> Index()
    {
        ViewBag.ProductCount=await _context.Products.CountAsync();
        var products =await _context.Products.Take(6).ToListAsync();
        return View(products);
    }

    [HttpPost]
    public async Task<IActionResult> Partial([FromBody]RequestModel reqModel)
    {
        var products=await _context.Products.Skip(reqModel.From).Take(6).ToListAsync();
        return PartialView("ShopProductPartialView",products);
    }
}


public class RequestModel
{
    public int From { get; set; }
}