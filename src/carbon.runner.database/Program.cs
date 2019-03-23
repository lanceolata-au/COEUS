using System;
using carbon.persistence.transforms;

namespace carbon.runner.database
{
    internal static class Program
    {
        private static void Main(string[] args)
        {

            const string zeryter = @"
            @@@@@@@@  @@@@@@@@  @@@@@@@   @@@ @@@  @@@@@@@  @@@@@@@@  @@@@@@@        @@@  @@@  @@@ @@@  @@@@@@@@  
            @@@@@@@@  @@@@@@@@  @@@@@@@@  @@@ @@@  @@@@@@@  @@@@@@@@  @@@@@@@@       @@@  @@@  @@@ @@@  @@@@@@@@  
                 @@!  @@!       @@!  @@@  @@! !@@    @@!    @@!       @@!  @@@       @@!  !@@  @@! !@@       @@!  
                !@!   !@!       !@!  @!@  !@! @!!    !@!    !@!       !@!  @!@       !@!  @!!  !@! @!!      !@!   
               @!!    @!!!:!    @!@!!@!    !@!@!     @!!    @!!!:!    @!@!!@!         !@@!@!    !@!@!      @!!    
              !!!     !!!!!:    !!@!@!      @!!!     !!!    !!!!!:    !!@!@!           @!!!      @!!!     !!!     
             !!:      !!:       !!: :!!     !!:      !!:    !!:       !!: :!!         !: :!!     !!:     !!:      
            :!:       :!:       :!:  !:!    :!:      :!:    :!:       :!:  !:!  :!:  :!:  !:!    :!:    :!:       
             :: ::::   :: ::::  ::   :::     ::       ::     :: ::::  ::   :::  :::   ::  :::     ::     :: ::::  
            : :: : :  : :: ::    :   : :     :        :     : :: ::    :   : :  :::   :   ::      :     : :: : :  
            ";
            
            Console.Write(zeryter);
            Console.WriteLine();
            try
            {

                var obj = new Runner(@"server=zeryter.xyz;user=owen;password=######", true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
        
    }
    
}
