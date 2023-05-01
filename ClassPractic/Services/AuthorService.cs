using ClassPractic.Data;
using ClassPractic.Models;
using ClassPractic.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ClassPractic.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly AppDbContext _context;


        public AuthorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseAuthor>> GetAll()
        {
            return await _context.CourseAuthors.OrderByDescending(m => m.Id).ToListAsync();
        }
    }
}
