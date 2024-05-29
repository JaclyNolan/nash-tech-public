using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.DTOs.Book
{
    public class GetAllBookRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public BookSortField SortField { get; set; } = BookSortField.Name;
        public SortOrder SortOrder { get; set; } = SortOrder.Asc;
        public string Search { get; set; } = string.Empty;
    }

    public enum BookSortField
    {
        Id,
        Name,
        DateCreated
    }

    public enum SortOrder
    {
        Asc,
        Desc
    }

}
