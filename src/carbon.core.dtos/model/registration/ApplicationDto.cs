using System;

namespace carbon.core.dtos.model.registration
{
    public class ApplicationDto
    {
        public Guid UserId { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNo { get; set; }
        public string RegistrationNo { get; set; }
        public int State { get; set; }
        public int Country { get; set; }
        public int Formation { get; set; }
    }
}