using ClassPractic.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassPractic.Data
{
    public  class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Slider> Sliders { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseImage> CourseImages { get; set; }

        public DbSet<CourseAuthor> CourseAuthors { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<NewsAuthor> NewsAuthors { get; set; }
    }
    
}
