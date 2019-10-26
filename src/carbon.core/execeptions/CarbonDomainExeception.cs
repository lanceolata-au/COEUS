using System;

namespace carbon.core.execeptions
{
    public class CarbonDomainException : Exception
    {
        public CarbonDomainException(string message) : base(message) {}
    }
}