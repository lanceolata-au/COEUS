using System;
using carbon.core.dtos.model.registration;
using carbon.core.execeptions;
using carbon.core.Features;

namespace carbon.core.domain.model.registration
{
    public class Application : Entity<int>
    {
        
        /*
         * This is the application for an event.
         *
         * Note that the defaults are set on initialisation.
         *
         * Where internal is used a test case exists and this must be left as is
         */

        public Guid UserId { get; internal set; }
        public StatusEnum Status { get; private set; } = StatusEnum.Started;
        public DateTime DateOfBirth { get; private set; } = DateTime.Now;
        public string PhoneNo { get; private set; } = null;
        public string RegistrationNo { get; private set; } = null;
        public int State { get; private set; } = 0;
        public int Country { get; private set; } = 0;
        public int Formation { get; private set; } = 0;

        public bool Locked { get; internal set; } = false;

        public static Application Create(Guid userId)
        {
            //TODO Guards

            var obj = new Application();

            obj.UserId = userId;

            return obj;
        }
        
        public void Update(ApplicationDto dto)
        {
            if (Locked) throw new CarbonDomainException("Application is Locked");
            if (!dto.UserId.Equals(UserId)) throw new IdMismatchException(
                "Saved id:\n" + UserId + "\nDoes not match update id:\n" + dto.UserId);
            
            Status = dto.Status;
            DateOfBirth = dto.DateOfBirth;
            PhoneNo = dto.PhoneNo;
            RegistrationNo = dto.RegistrationNo;
            State = dto.State;
            Country = dto.Country;
            Formation = dto.Formation;
            
        }
        
    }

}