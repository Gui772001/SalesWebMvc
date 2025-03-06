using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SelleService _selleService;


        public SellersController(SelleService selleService)
        {
            _selleService = selleService;
        }

        public IActionResult Index()
        {
            var list = _selleService.FindAll();

          
            return View(list);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            _selleService.Insert(seller);

            return RedirectToAction(nameof(Index));
        }
    }
}
