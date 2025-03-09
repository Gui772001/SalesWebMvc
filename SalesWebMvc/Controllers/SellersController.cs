using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SelleService _sellerService;
        private readonly DepartmentService _departmentService;
        private readonly SalesWebMvcContext _context;

        public SellersController(SelleService sellerService, DepartmentService departmentService, SalesWebMvcContext context)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Verificar se o Departamento existe
                var department = await _context.Department
                    .FirstOrDefaultAsync(d => d.Id == seller.DepartmentId);

                if (department == null)
                {
                    ModelState.AddModelError("DepartmentId", "Departamento não encontrado.");
                    return View(seller);
                }

                // Associar o departamento ao seller
                seller.Department = department;

                // Adicionar o Seller
                _context.Add(seller);
                await _context.SaveChangesAsync();

                // Confirmar a transação
                await transaction.CommitAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Se algo der errado, fazer o rollback da transação
                await transaction.RollbackAsync();
                Console.WriteLine($"Error: {ex.Message}");
                return View(seller);
            }
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch(IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        // GET: Edit
        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var seller = await _context.Seller
                .Include(s => s.Department) // Inclui o departamento para editar
                .FirstOrDefaultAsync(s => s.Id == id);

            if (seller == null)
            {
                return NotFound();
            }

            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel
            {
                Seller = seller,
                Departments = departments
            };

            return View(viewModel);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (id != seller.Id)
            {
                return NotFound();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Verificar se o Departamento existe
                var department = await _context.Department
                    .FirstOrDefaultAsync(d => d.Id == seller.DepartmentId);

                if (department == null)
                {
                    ModelState.AddModelError("Seller.DepartmentId", "Departamento não encontrado.");
                    return View(seller); // Retorna a view com erro
                }

                // Atualizar o departamento associado ao seller
                seller.Department = department;

                // Atualizar o seller
                _context.Update(seller);
                await _context.SaveChangesAsync();

                // Confirmar a transação
                await transaction.CommitAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Se algo der errado, fazer o rollback da transação
                await transaction.RollbackAsync();
                Console.WriteLine($"Error: {ex.Message}");
                return View(seller); // Retorna a view com o modelo contendo os erros
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