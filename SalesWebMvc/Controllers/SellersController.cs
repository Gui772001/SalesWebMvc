using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;

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
        public IActionResult Delete(int? id)
        {
           if( id == null)
            {
                return NotFound();
            }
            var obj = _selleService.FindById(id.Value);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _selleService.Remove(id);
            return RedirectToAction(nameof(Index));

        }
           
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _selleService.FindById(id.Value);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _selleService.FindById(id.Value);

            if (obj == null)
            {
                return NotFound();
            }
            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj,Departments = departments };
            return View(viewModel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,Seller seller)
        {
          if(id != seller.Id)
            {
                return BadRequest();
            }
            try
            {
                _selleService.Uptade(seller);
                return RedirectToAction(nameof(Index));
            }catch(NotFoundException)
            {
                return NotFound();
            }
            catch (DbConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}
