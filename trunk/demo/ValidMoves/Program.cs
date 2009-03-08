using System;

namespace ValidMoves
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ValidMoves game = new ValidMoves())
            {
                game.Run();
            }
        }
    }
}

