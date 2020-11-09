using System;
using System.Configuration;
using System.Text;
using System.Threading;

namespace GameOfLife
{
    class GameOfLife
    {
        private static void Print(Status[,] future, int rows, int columns, int timeout = 500)
        {
            var stringBuilder = new StringBuilder();
            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    var cell = future[row, column];
                    stringBuilder.Append(cell == Status.Alive ? "1" : "0");
                }
                stringBuilder.Append("\n");
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.Write(stringBuilder.ToString());
            Thread.Sleep(timeout);
        }
        public static void Main()
        {
            int rows = 0; 
            int columns = 0;
            int numberOfGeneration = 0;
            int cnt = 0;

            try
            {
                rows = Int32.Parse(ConfigurationManager.AppSettings["Rows"]);
                columns = Int32.Parse(ConfigurationManager.AppSettings["Columns"]);
                numberOfGeneration = Int32.Parse(ConfigurationManager.AppSettings["GenNumber"]);

                Simulation sim = new Simulation(rows, columns);
                var grid = new Status[rows, columns];
                var random = new Random();

                for (var row = 0; row < rows; row++)
                {
                    for (var column = 0; column < columns; column++)
                    {
                        grid[row, column] = (Status) random.Next(0, 2);
                    }
                }

                Console.CancelKeyPress += (sender, args) =>
                {
                    cnt = numberOfGeneration + 1;
                    Console.WriteLine("\nEnding simulation.");
                };

                Console.Clear();

                while (cnt <= numberOfGeneration)
                {
                    Print(grid, rows, columns);
                    grid = sim.NextGeneration(grid);
                    cnt++;
                }
               
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occured {ex}");
            }
        }
    }
}
