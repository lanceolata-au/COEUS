namespace carbon.core.dtos.account
{
    public class CoreUserUpdate
    {
        public AccessEnum Access { get; set; } = AccessEnum.Standard;
        public string Picture { get; set; } = null;
    }
}