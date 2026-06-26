namespace HealthAxisCore_Api.Models.Dtos
{
    public class PaginationQueryDto
    {
        private const int MaxPageSize = 100;

        private int _pageNumber = 1;

        private int _pageSize = 10;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value < 1)
                {
                    _pageSize = 10;
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
        public List<T> Items { get; set; } = new();

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}