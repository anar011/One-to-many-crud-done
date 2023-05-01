using ClassPractic.Data;
using ClassPractic.Models;
using ClassPractic.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ClassPractic.Services
{
    public class CouseService : ICourseService
    {
        private readonly AppDbContext _context;
        public CouseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAll() => await _context.Courses.Include(m => m.CourseImages).ToListAsync();

        public async Task<Course> GetById(int id) => await _context.Courses.FindAsync(id);



        public async Task<Course> GetFullDataById(int id) => await _context.Courses.Include(m => m.CourseImages).Include(m => m.CourseAuthor).FirstOrDefaultAsync(m => m.Id == id);

        public async Task<List<Course>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Courses.Include(m => m.CourseAuthor).Include(m => m.CourseImages).Skip((page * take) - take).Take(take).ToListAsync();
        }
    }
}
