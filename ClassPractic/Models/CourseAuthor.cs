namespace ClassPractic.Models
{
    public class CourseAuthor:BaseEntity
    {
        public string Name  { get; set; }

        public ICollection<Course> Courses { get; set; }

    }
}
