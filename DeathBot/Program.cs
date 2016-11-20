using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DeathBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "DeathBot";
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Enter the bot token.");
            Console.ForegroundColor = ConsoleColor.Green;
            string botToken = Console.ReadLine();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Enter the bot token.");
            Console.ForegroundColor = ConsoleColor.Green;
            foreach(char c in botToken)
            {
                Console.Write("*");
            }
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Enter the owner ID.");
            Console.ForegroundColor = ConsoleColor.Green;
            string ownerId = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Starting up DeathBot.");
            Bot bot = new Bot(botToken, ownerId);
        }
    }
}
