using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Wrappers
{
    public class PaginatedResult<T>
    {
        public List<T> Result { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        public PaginatedResult(List<T> result, int total, int pageNumber, int pageSize)
        {
            Result = result;
            TotalCount = total;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        //public int PageIndex { get; set; }
        //public int PageSize { get; set; }
        //public int Count { get; set; }
        //public List<T> Data { get; set; } = new();

        //public PaginatedResult(List<T> data, int count, int pageIndex, int pageSize)
        //{
        //    Data = data;
        //    Count = count;
        //    PageIndex = pageIndex;
        //    PageSize = pageSize;
        //}
    }
}
