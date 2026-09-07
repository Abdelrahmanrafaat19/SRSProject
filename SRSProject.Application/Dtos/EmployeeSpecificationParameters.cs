using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Dtos
{
    public class EmployeeSpecificationParameters
    {
        public string? Search { get; set; }

        public bool? IsActive { get; set; }

        public decimal? MinimumSalary { get; set; }

        public decimal? MaximumSalary { get; set; }

        public string? Sort { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
