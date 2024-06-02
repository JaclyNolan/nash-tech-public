using EF_Core_Assignment1.Application.DTOs.Common;

namespace EF_Core_Assignment1.Application.DTOs.Book
{
    public class GetAllBookRequest
    {
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
        public BookSortField SortField { get; set; } = BookSortField.Title;
        public SortOrder SortOrder { get; set; } = SortOrder.Asc;
        public string? Search { get; set; }
    }

    public enum BookSortField
    {
        Id,
        Title,
        DateCreated
    }
}
