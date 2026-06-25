namespace HealthApp.Shared.DTOs;

public class PaginationQueryDto
{
    private const int MaxPageSize = 50;

    private int pageNumber = 1;
    private int pageSize = 5;

    public int PageNumber
    {
        get => pageNumber;
        set => pageNumber = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => pageSize;
        set => pageSize = value > MaxPageSize
            ? MaxPageSize
            : value < 1
                ? 5
                : value;
    }
}

public class PagedResultDto<T>
{
    public List<T> Items { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages =>
        PageSize == 0
            ? 0
            : (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}