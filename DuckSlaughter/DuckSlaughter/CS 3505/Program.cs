using System;
using CS_3505_Project_06;

namespace Project7
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game06 game = new Game06())
            {
                game.Run();
            }
        }
    }
}

