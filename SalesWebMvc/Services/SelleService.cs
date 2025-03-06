using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SelleService
    {

        private readonly SalesWebMvcContext _context; 

        public SelleService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public void  Insert(Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }
    }
}
