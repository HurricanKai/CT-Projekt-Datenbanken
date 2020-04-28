using System.ComponentModel.DataAnnotations;

namespace ct_datenbanken.Data
{
    public class BookDTO
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        [Range(0, int.MaxValue)]
        public int PageCount { get; set; }
        [Required]
        public string AuthorName { get; set; }
        public bool IsAvailable { get; set; }
    }
}