namespace HealthAxis.API.DTOs.CommonDtos
{
    public class PaginationQueryDto
    {
        private const int MaxPageSize = 50;

        private int _pageNumber = 1;

        private int _pageSize = 5;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1
                ? 1
                : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value < 1)
                {
                    _pageSize = 5;
                    return;
                }

                _pageSize = value > MaxPageSize
                    ? MaxPageSize
                    : value;
            }
        }

        public string? SearchTerm { get; set; }

        public int? Specialisation { get; set; }
    }
}