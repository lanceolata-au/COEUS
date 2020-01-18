using System;
using System.Collections.Generic;

namespace carbon.core.dtos.filter
{
    public class ApplicationFilterDto
    {
        public List<int> Countries { get; set; } = null;
        public List<int> States { get; set; } = null;
        public DateTime AgeDate { get; set; } = default;
        public int MinimumAge { get; set; } = 0;
        public int MaximumAge { get; set; } = 0;
        public int ResultsPerPage { get; set; } = 50;
        public int Page { get; set; } = 1;
    }
}