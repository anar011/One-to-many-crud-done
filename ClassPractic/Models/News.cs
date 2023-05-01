namespace ClassPractic.Models
{
    public class News : BaseEntity
    {
        public DateTime Date { get; set; }

        public string? Question { get; set; }

        public string? Image { get; set; }

        public int NewsAuthorId { get; set; }

        public NewsAuthor? NewsAuthor { get; set; }

      

    }
}
