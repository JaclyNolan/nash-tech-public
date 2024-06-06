using EF_Core_Assignment1.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.DTOs.BookBorrowingRequest
{
    public class GetAllBorrowingRequest
    {
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
        public BorrowingRequestSortField SortField { get; set; } = BorrowingRequestSortField.RequestorName;
        public SortOrder SortOrder { get; set; } = SortOrder.Asc;
    }

    public enum BorrowingRequestSortField
    {
        RequestorName = 2,
        ActionerName = 3,
        Status = 1,
        RequestDate = 0
    }
}
