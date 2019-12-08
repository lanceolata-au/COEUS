using carbon.core.Features;

namespace carbon.core.domain.model.registration
{
    public class State : Entity<int>
    {
        public int CountryId { get; private set; }
        public string ShortCode { get; private set; }
        public string FullName { get; private set; }
    }
}