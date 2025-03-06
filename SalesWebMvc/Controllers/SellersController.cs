using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SelleService _selleService;
        private readonly DepartmentService _departmentService;


        public SellersController(SelleService selleService, DepartmentService departmentService)
        {
            _selleService = selleService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var list = _selleService.FindAll();

          
            return View(list);
        }
        public IActionResult Create()
        {
            var departmens = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departmens };
            return View(viewModel);

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
