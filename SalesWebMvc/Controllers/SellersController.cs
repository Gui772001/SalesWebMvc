using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;

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
            var list = _selleService.FindAllA(); // Chamada síncrona

            return View(list);
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll(); // Chamada síncrona
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            _selleService.Insert(seller); // Chamada síncrona

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _selleService.FindById(id.Value); // Chamada síncrona

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
            _selleService.Remove(id); // Chamada síncrona
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _selleService.FindById(id.Value); // Chamada síncrona

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
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = _selleService.FindById(id.Value); // Chamada síncrona
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            List<Department> departments = _departmentService.FindAll(); // Chamada síncrona
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };

            // Aqui você pode colocar o TempData para registrar a ação
            TempData["ConsoleMessage"] = $"Editando vendedor: {obj.Id} - {obj.Name}";

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll(); // Chamada síncrona
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            if (id != seller.Id)
            {
                return BadRequest();
            }
            try
            {
                _selleService.Uptade(seller); // Chamada síncrona
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (DbConcurrencyException)
            {
                return BadRequest();
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
