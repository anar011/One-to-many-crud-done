namespace ClassPractic.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public bool SoftDelete { get; set; } 
    }
}
