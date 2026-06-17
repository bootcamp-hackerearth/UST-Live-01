using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.API.Helpers
{
    [ExcludeFromCodeCoverage]
    public class PaginationParams
    {
        private const int MaxPageSize = 100;

        public int? PageNumber { get; set; }

        private int? _pageSize = 10;

        public int? PageSize
        {
            get => _pageSize;
            set => _pageSize = value.HasValue
                ? Math.Min(value.Value, MaxPageSize)
                : 10;
        }

        public string? Search { get; set; }
    }
}