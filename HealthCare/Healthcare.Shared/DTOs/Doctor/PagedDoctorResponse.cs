using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Shared.DTOs.Doctor
{
    public class PagedDoctorResponse
    {

        public List<DoctorListDto> Items { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

    }
}
