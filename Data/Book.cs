namespace ct_datenbanken.Data
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PageCount { get; set; }
        public virtual Author Author { get; set; }
        public bool IsAvailable { get; set; }
    }
}