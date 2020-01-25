using System;

namespace carbon.core.dtos.model.registration
{
    public class ApplicationDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public StatusEnum Status { get; set; }
        public string DateOfBirth { get; set; }
        public string PhoneNo { get; set; }
        public string RegistrationNo { get; set; }
        public int State { get; set; }
        public int Country { get; set; }
        public int Formation { get; set; }
    }
}