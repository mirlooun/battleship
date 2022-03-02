using System;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums;

namespace DAL.ConsoleUi
{
    public static class HitMenuUi 
    {
        public static void DrawSingleBoard(ECellState[,] presentationalBoard, LocationDal hit)
        {
            var width = presentationalBoard.GetUpperBound(0) + 1; //x-axis
            var height = presentationalBoard.GetUpperBound(1) + 1; //y-axis
            
            Console.ForegroundColor = ConsoleColor.White;
            for (var colIndex = 0; colIndex < width; colIndex++)
            {
                var c = (char)(65 + colIndex);
                if (colIndex == 0) Console.Write("    ");
                switch (colIndex + 1)
                {
                    case < 10:
                    case < 100 and > 9:
                        Console.Write($"  {c}  ");
                        break;
                    default:
                        Console.Write($"  {c}  ");
                        break;
                }
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            for (var colIndex = 0; colIndex < width; colIndex++)
            {
                if (colIndex == 0) Console.Write("    ");
                Console.Write("+---+");
            }

            Console.WriteLine();

            for (var rowIndex = 0; rowIndex < height; rowIndex++)
            {
                if (rowIndex + 1 < 10) Console.Write("  ");
                else if (rowIndex + 1 > 9 && rowIndex + 1 < 100) Console.Write(" ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{rowIndex + 1} ");
                Console.ForegroundColor = ConsoleColor.Blue;

                for (var colIndex = 0; colIndex < width; colIndex++)
                {
                    Console.Write("| ");
                    if (hit.X == colIndex && hit.Y == rowIndex)
                    {
                        var cellState = "*";
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(cellState);
                        Console.ForegroundColor = ConsoleColor.Blue; 
                    }
                    else
                    {
                        var cellState = CellString(presentationalBoard[colIndex, rowIndex], false);
                        Console.ForegroundColor = cellState switch
                        {
                            "X" => ConsoleColor.White,
                            "@" => ConsoleColor.Red,
                            _ => Console.ForegroundColor
                        };
                        Console.Write(cellState);
                        Console.ForegroundColor = ConsoleColor.Blue; 
                    }
                    
                    Console.Write(" |");
                }
                Console.WriteLine();
                for (var colIndex = 0; colIndex < width; colIndex++)
                {
                    if (colIndex == 0) Console.Write("    ");
                    Console.Write("+---+");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.ResetColor();
        }
        
        private static string CellString(ECellState cellState, bool displayShips)
        {
            return cellState switch
            {
                ECellState.Empty => " ",
                ECellState.Ship => displayShips ? "S" : " ",
                ECellState.Miss => "X",
                ECellState.Hit => "@",
                _ => "-"
            };
        }
        
        public static void ShowLegend()
        {
            // hit
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" *");
            Console.ResetColor();
            Console.WriteLine(" - is your hit");
            // damaged ship
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" @");
            Console.ResetColor();
            Console.WriteLine(" - is damaged ship");
            // miss
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" X");
            Console.ResetColor();
            Console.WriteLine(" - is miss");
        }
    }
}
