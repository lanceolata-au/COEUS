using System.Collections.Generic;

namespace carbon.core.dtos.model.registration
{
    public class ApplicationsPackageDto
    {
        public List<ApplicationDto> Applications { get; set; }
        public List<CountryDto> ApplicationCountries { get; set; }
        public List<StateDto> ApplicationStates { get; set; }
    }
}