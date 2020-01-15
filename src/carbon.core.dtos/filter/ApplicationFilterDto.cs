using System;
using System.Collections.Generic;

namespace carbon.core.dtos.filter
{
    public class ApplicationFilterDto
    {
        public List<int> Countries { get; set; }
        public List<int> States { get; set; }
        public DateTime AgeDate { get; set; }
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }
    }
}