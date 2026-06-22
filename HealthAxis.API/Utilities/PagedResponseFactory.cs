using HealthAxis.API.DTO.CommonDtos;

namespace HealthAxis.API.Utilities
{
    public static class PagedResponseFactory
    {
        public static PagedResponseDto<T> Create<T>(
            List<T> data,
            PaginationQueryDto paginationQuery,
            int totalRecords)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(paginationQuery);

            var totalPages = (int)Math.Ceiling(
                totalRecords / (double)paginationQuery.PageSize);

            return new PagedResponseDto<T>
            {
                Data = data,
                PageNumber = paginationQuery.PageNumber,
                PageSize = paginationQuery.PageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }
    }
}