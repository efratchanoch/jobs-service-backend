using System;
using System.Collections.Generic;

namespace jobs_service_backend.DTOs.Common
{
    public class PaginatedListDto<T>
    {
        public IEnumerable<T> Items { get; set; } = Array.Empty<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        /// <summary>Effective page size after normalization (same as used for <see cref="TotalPages"/>).</summary>
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public PaginatedListDto(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = pageSize > 0
                ? (int)Math.Ceiling(count / (double)pageSize)
                : 0;
        }
    }
}