namespace ClassPractic.Models
{
    public class NewsAuthor:BaseEntity
    {

        public string? AuthorName { get; set; }

        public ICollection<News> News { get; set; }
    }
}
