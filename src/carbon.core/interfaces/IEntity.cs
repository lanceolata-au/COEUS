namespace carbon.core.Interfaces
{
    public interface IEntity<out TLd>
    {
        TLd Id { get; }
    }
}