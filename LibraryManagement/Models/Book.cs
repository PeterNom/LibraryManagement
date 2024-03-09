namespace LibraryManagement.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty ;
        public DateTime PublishedDate { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;
    }
}
