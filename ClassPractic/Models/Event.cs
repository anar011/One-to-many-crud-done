namespace ClassPractic.Models
{
    public class Event:BaseEntity
    {


        public string? Name { get; set; }

        public string? Location { get; set; }

        public DateTime Date { get; set; }
    }
}
