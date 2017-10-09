using System;
using System.Text;

namespace BoggleSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Use Ctrl-C to exit program
                Console.CancelKeyPress += delegate
                {
                    Console.WriteLine("\n\nExiting boggle solver.");
                    Environment.Exit(0);
                };

                var dictionary = new BoggleDictionary("TWL06.txt");
                var solver = new GridSolver(dictionary, 4, 4);
                Intro();
                var grid = GetGrid();
                while (!string.IsNullOrWhiteSpace(grid))
                {
                    try
                    {
                        PrintWords(solver.FindWords(grid));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    grid = GetGrid();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private static string GetGrid()
        {
            var sb = new StringBuilder();
            Console.WriteLine("Enter boggle grid one line at a time. Ctrl-C to quit.");
            for (var i = 1; i < 5; ++i)
            {
                sb.Append(GetLine(i));
            }
            return sb.ToString();
        }

        private static string GetLine(int lineNumber)
        {
            Console.Write($"Line {lineNumber}: ");
            var b = new StringBuilder();
            while (b.Length < 4)
            {
                ConsoleKeyInfo c = Console.ReadKey(true);
                if (!char.IsLetter(c.KeyChar)) continue;
                char uc = char.ToUpper(c.KeyChar);
                if (uc == 'Q')
                {
                    Console.Write("Qu");
                }
                else
                {
                    Console.Write(uc);
                }
                b.Append(uc);
            }
            Console.WriteLine();
            return b.ToString();
        }

        private const string _INTRO =
                "Boggle is a word game played using a 4 x 4 grid of letters,\nin which players attempt to find words in sequences of adjacent letters.\n\n'Qu' is combined into a single letter, but should be entered as 'Q'\n\n"
            ;
            
        private static void Intro()
        {
            Console.WriteLine(_INTRO);
        }

        private static void PrintWords(string[] words)
        {
            Console.WriteLine($"\nWords Found: {words.Length}");
            var c = 0;
            foreach (var word in words)
            {
                Console.WriteLine($"{word} [{BoggleDictionary.ScoreWord(word)}]");
                c++;
                if (c % 10 != 0) continue;
                Console.WriteLine("[Hit Enter]");
                Console.ReadKey(true);
            }
            Console.WriteLine("**End of word list***\n");
        }

    }
}
