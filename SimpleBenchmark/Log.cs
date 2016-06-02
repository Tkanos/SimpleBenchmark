using System;

namespace SimpleBenchmark
{
    public static class Log
    {
        /// <summary>
        /// Writes the colored line.
        /// </summary>
        /// <param name="colour">The colour.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void WriteColoredLine(ConsoleColor colour, string format, params object[] args)
        {
            WriteColoredLine(colour, String.Format(format, args));
        }

        /// <summary>
        /// Writes the colored line.
        /// </summary>
        /// <param name="colour">The colour.</param>
        /// <param name="object">The object.</param>
        public static void WriteColoredLine(ConsoleColor colour, object @object)
        {
            WriteColoredLine(colour, @object.ToString());
        }

        /// <summary>
        /// Writes the colored line.
        /// </summary>
        /// <param name="colour">The colour.</param>
        /// <param name="text">The text.</param>
        public static void WriteColoredLine(ConsoleColor colour, string text)
        {
            var origColour = Console.ForegroundColor;
            Console.ForegroundColor = colour;
            Console.WriteLine(text);
            Console.ForegroundColor = origColour;
        }
    }
}
