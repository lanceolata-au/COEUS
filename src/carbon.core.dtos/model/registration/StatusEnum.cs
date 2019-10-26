namespace carbon.core.dtos.model.registration
{
    public enum StatusEnum : sbyte
    {
        Excluded = 0,
        Canceled = 10,
        Started = 20,
        Saved = 50,
        Submitted = 80,
        Approved = 100,
        
    }
}