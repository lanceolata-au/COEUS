using System;
using System.Reflection.Metadata;

namespace carbon.api.Services
{
    public static class ExceptionHandler
    {
        public static void Handle(Exception e)
        {
            Console.WriteLine(e);
        }
    }
}