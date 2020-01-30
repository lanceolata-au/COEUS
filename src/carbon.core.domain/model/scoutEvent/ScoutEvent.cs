using carbon.core.Features;

namespace carbon.core.domain.model.scoutEvent
{
    public class ScoutEvent : Entity<int>
    {
        public string Name { get; private set; }
    }
}