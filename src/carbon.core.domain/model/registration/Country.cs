using carbon.core.Features;

namespace carbon.core.domain.model.registration
{
    public class Country : Entity<int>
    {
        public string ShortCode { get; private set; }
        public string FullName { get; private set; }
    }
}