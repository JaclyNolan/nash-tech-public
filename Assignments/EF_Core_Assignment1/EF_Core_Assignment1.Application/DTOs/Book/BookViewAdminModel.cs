using EF_Core_Assignment1.Application.DTOs.Category;

namespace EF_Core_Assignment1.Application.DTOs.Book
{
    public class BookViewAdminModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryAdminViewModel Category { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }

    public class BookUserViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryUserViewModel Category { get; set; }
    }
}
