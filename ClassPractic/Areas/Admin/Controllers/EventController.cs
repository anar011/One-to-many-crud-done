using ClassPractic.Data;
using ClassPractic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassPractic.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class EventController : Controller
    {
        private readonly AppDbContext _context;

        public EventController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Event> courses = await _context.Events.Where(m => !m.SoftDelete).ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null) return BadRequest();
            Event? events = await _context.Events.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (events is null) return NotFound();

            return View(events);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}

