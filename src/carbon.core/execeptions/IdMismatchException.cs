using System;

namespace carbon.core.execeptions
{
    public class IdMismatchException : Exception
    {
        public IdMismatchException(string message) : base(message) {}
    }
}