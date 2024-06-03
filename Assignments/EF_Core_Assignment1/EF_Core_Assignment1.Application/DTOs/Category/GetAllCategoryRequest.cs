using EF_Core_Assignment1.Application.DTOs.Common;

namespace EF_Core_Assignment1.Application.DTOs.Category
{
    public class GetAllCategoryRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public CategorySortField SortField { get; set; } = CategorySortField.Name;
        public SortOrder SortOrder { get; set; } = SortOrder.Asc;
        public string? Search { get; set; }
    }

    public enum CategorySortField
    {
        Id,
        Name,
        DateCreated
    }
}
