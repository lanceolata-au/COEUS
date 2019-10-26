using System;

namespace carbon.core.dtos.account
{
    public class CoreUserDto
    {
        public Guid UserId { get; set; }
        public AccessEnum Access { get; set; } = AccessEnum.Standard;
        public string Picture { get; set; } = null;
    }
}