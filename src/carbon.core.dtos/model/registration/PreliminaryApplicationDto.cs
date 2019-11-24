using System;

namespace carbon.core.dtos.model.registration
{
    public class PreliminaryApplicationDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int State { get; set; }
        public int Country { get; set; }
    }
}