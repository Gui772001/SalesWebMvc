using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class SelleService
    {
        private readonly SalesWebMvcContext _context;

        public SelleService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller obj)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Adiciona o objeto (Seller) à base de dados
                _context.Add(obj);

                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();

                // Confirma a transação se tudo ocorrer bem
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // Se ocorrer um erro, desfaz as alterações
                await transaction.RollbackAsync();

                // Relança a exceção para que ela seja tratada mais acima
                throw;
            }
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }catch(DbUpdateException e)
            {
                throw new IntegrityException(e.Message);

            }


        }
        public async Task UpdateAsync(Seller seller)
        {
            bool exists = await _context.Seller.AnyAsync(s => s.Id == seller.Id);
            if (!exists)
            {
                throw new NotFoundException("Seller not found");
            }

            try
            {
                _context.Update(seller);  // Atualiza o seller
                await _context.SaveChangesAsync();  // Salva as mudanças
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }



    }
}
