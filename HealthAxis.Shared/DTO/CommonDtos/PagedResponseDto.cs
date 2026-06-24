namespace HealthAxis.Shared.DTO.CommonDtos
{
    public sealed class PagedResponseDto<T>
    {
        public List<T> Items { get; set; } = new();

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages =>
            PageSize == 0
                ? 0
                : (int)Math.Ceiling((double)TotalRecords / PageSize);

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}