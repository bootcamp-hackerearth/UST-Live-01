using Healthcare.Shared.DTOs.Doctor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Shared.DTOs.Patient
{
    public class PagedPatientResponse
    {
        public List<PatientListDto> Items { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
