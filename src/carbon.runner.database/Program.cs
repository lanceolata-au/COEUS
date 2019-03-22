using System;
using carbon.persistence.transforms;

namespace carbon.runner.database
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World!");

                var obj = new Runner(@"server=localhost;database=carbon;user=carbon;password=the_game");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}