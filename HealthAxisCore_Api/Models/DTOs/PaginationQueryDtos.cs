using System.Text.Json.Serialization;

namespace HealthAxisCore_Api.Models.Dtos
{
    public class PaginationQueryDto
    {
        private const int DefaultPageNumber = 1;

        private const int DefaultPageSize = 10;

        private const int MaxPageSize = 100;

        private int _pageNumber = DefaultPageNumber;

        private int _pageSize = DefaultPageSize;

        [JsonRequired]
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? DefaultPageNumber : value;
        }

        [JsonRequired]
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value < 1)
                {
                    _pageSize = DefaultPageSize;
                    return;
                }

                _pageSize = value > MaxPageSize
                    ? MaxPageSize
                    : value;
            }
        }
    }

    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = [];

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}