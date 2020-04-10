using System;

namespace carbon.core.guards
{
    public class GuardException : Exception
    {
        public GuardException(GuardType type, string message) : base(message + " | | " + type.ToString())
        {
            
        }
    }
}