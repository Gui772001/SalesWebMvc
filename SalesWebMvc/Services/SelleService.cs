using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
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

        public List<Seller> FindAllA()
        {
            return _context.Seller.ToList(); // Versão síncrona
        }

        public void Insert(Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges(); // Chamada síncrona
        }

        public Seller FindById(int id)
        {
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id); // Chamada síncrona
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            if (obj != null)
            {
                _context.Seller.Remove(obj);
                _context.SaveChanges(); // Chamada síncrona
            }
        }

        public void Uptade(Seller obj)
        {
            if (!_context.Seller.Any(x => x.Id == obj.Id))
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(obj);
                _context.SaveChanges(); // Chamada síncrona
            }
            catch (DbConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
