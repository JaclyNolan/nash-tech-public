namespace EF_Core_Assignment1.Application.DTOs.Book
{
    public class BookViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}
