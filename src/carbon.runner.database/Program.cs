using System;
using System.Collections.Generic;
using carbon.runner.database.transforms;
using NDesk.Options;

namespace carbon.runner.database
{
    internal static class Program
    {
        private static void Main(string[] args)
        {

            var connectionString = "";
            var dropAll = false;
            var startingData = false;
            
            string data = null;
            var help   = false;
            var verbose = 0;
            var p = new OptionSet () {
                { "connectionString=",  v => data = v },
                { "dropAll=",           v => data = v },
                { "startingData=",      v => data = v },
                { "h|?|help",   v => help = v != null },
            };
            
            var extra = p.Parse (args);

            foreach (var arg in extra)
            {
                
                if (arg.Contains("connectionString="))
                {
                    connectionString = arg.Replace("connectionString=", "");
                }

                if (arg.Contains("dropAll="))
                {
                    dropAll = bool.Parse(arg.Replace("dropAll=",""));
                }
                
                if (arg.Contains("startingData="))
                {
                    startingData = bool.Parse(arg.Replace("startingData=",""));
                }
                
            }

            const string zeryter = @"
__________________________________________________________________________________________________________________________________________________
_%\%\%\%\%\%\__%\%\%\%\%\%\__%\%\%\%\%\____%\%\____%\%\__%\%\%\%\%\%\__%\%\%\%\%\%\__%\%\%\%\%\__________%\%\____%\%\__%\%\____%\%\__%\%\%\%\%\%\_
_______%\%\____%\____________%\%\____%\%\__%\%\____%\%\______%\%\______%\____________%\%\____%\%\__________%\%\%\%\____%\%\____%\%\________%\%\___
_____%\%\______%\%\%\%\%\____%\%\%\%\%\______%\%\%\%\________%\%\______%\%\%\%\%\____%\%\%\%\%\______________%\%\________%\%\%\%\________%\%\_____
___%\%\________%\%\__________%\%\__%\%\________%\%\__________%\%\______%\%\__________%\%\__%\%\____%\%\____%\%\%\%\________%\%\________%\%\_______
_%\%\%\%\%\%\__%\%\%\%\%\%\__%\%\____%\%\______%\%\__________%\%\______%\%\%\%\%\%\__%\%\____%\%\__%\%\__%\%\____%\%\______%\%\______%\%\%\%\%\%\_
__________________________________________________________________________________________________________________________________________________        
            ";
             
            Console.Write(zeryter);
            Console.WriteLine();
            try
            {

                var obj = new Runner(connectionString, dropAll, startingData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
        
    }
    
}
