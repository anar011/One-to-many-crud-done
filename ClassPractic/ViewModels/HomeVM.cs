using ClassPractic.Models;

namespace ClassPractic.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }

        public IEnumerable<Course> Courses { get; set; }

        public IEnumerable<Event> Events { get; set; }

        public Course LatestNews { get; set; }

        public IEnumerable<News> News { get; set; }

        public IEnumerable<NewsAuthor> NewsAuthors { get; set; }

    }
}
