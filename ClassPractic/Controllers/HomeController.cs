using ClassPractic.Data;
using ClassPractic.Models;
using ClassPractic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ClassPractic.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            IEnumerable<Slider> sliders = await _context.Sliders.Where(m => !m.SoftDelete).ToListAsync();
            IEnumerable<Course> courses = await _context.Courses.Include(m=>m.CourseAuthor).Include(m=>m.CourseImages).Where(m => !m.SoftDelete).ToListAsync();
            Course? latestNews = await _context.Courses.Include(m => m.CourseAuthor).Include(m => m.CourseImages).Where(m => !m.SoftDelete).OrderByDescending(m => m.Id).FirstOrDefaultAsync();
            IEnumerable<Event> events = await _context.Events.Where(m => !m.SoftDelete).ToListAsync();
            IEnumerable<News> news = await _context.News.Where(m => !m.SoftDelete).ToListAsync();
            IEnumerable<NewsAuthor> newsAuthors = await _context.NewsAuthors.Where(m => !m.SoftDelete).ToListAsync();

            HomeVM homevm = new()
            {
                Sliders = sliders,
                Courses=courses,
                LatestNews=latestNews,
                Events = events,
                News=news,
                NewsAuthors=newsAuthors
            };

            return View(homevm);
        }

    }
}