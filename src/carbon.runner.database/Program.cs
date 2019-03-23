using System;
using carbon.persistence.transforms;

namespace carbon.runner.database
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World!");

                var obj = new Runner(@"server=localhost;database=carbon;user=carbon;password=#####");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
