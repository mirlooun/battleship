using System;
using System.Threading;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums;

namespace DAL.ConsoleUi
{
    public static class BattleshipUi
    {
        public static void DrawBoards((ECellState[,] enemyBoard, ECellState[,] attackerBoard) boards)
        {
            var (enemyBoard, attackerBoard) = boards;
            
            Console.Title = "Battleship";
            
            var width = enemyBoard.GetUpperBound(0) + 1; //x-axis
            var height = enemyBoard.GetUpperBound(1) + 1; //y-axis
            
            Console.ForegroundColor = ConsoleColor.White;
            
            DrawUpperAlphabeticRow(width);
            DrawUpperAlphabeticRow(width);

            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.Blue;
            
            WriteUpperDashLine(width);
            WriteUpperDashLine(width);

            Console.WriteLine();

            for (var rowIndex = 0; rowIndex < height; rowIndex++)
            {
                DrawBoardBody(rowIndex, width, enemyBoard, false);
                DrawBoardBody(rowIndex, width, attackerBoard, true);

                Console.WriteLine();
                
                DrawLowerDashLine(width);
                DrawLowerDashLine(width);

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.ResetColor();
        }

        private static void DrawLowerDashLine(int width)
        {
            for (var colIndex = 0; colIndex < width; colIndex++)
            {
                if (colIndex == 0) Console.Write("    ");
                Console.Write("+---+");
            }
        }

        private static void DrawBoardBody(int rowIndex, int width, ECellState[,] enemyBoard, bool showBoats)
        {
            if (rowIndex + 1 < 10) Console.Write("  ");
            else if (rowIndex + 1 > 9 && rowIndex + 1 < 100) Console.Write(" ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{rowIndex + 1} ");

            Console.ForegroundColor = ConsoleColor.Blue;
            for (var colIndex = 0; colIndex < width; colIndex++)
            {
                Console.Write("| ");
                var cellState = CellString(enemyBoard[colIndex, rowIndex], showBoats);
                if (cellState.Equals("*")) Console.ForegroundColor = ConsoleColor.Gray;
                else if (cellState.Equals("X")) Console.ForegroundColor = ConsoleColor.White;
                else if (cellState.Equals("@")) Console.ForegroundColor = ConsoleColor.Red;
                else if (cellState.Equals("S")) Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(cellState);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(" |");
            }
        }

        private static void WriteUpperDashLine(int width)
        {
            for (var colIndex = 0; colIndex < width; colIndex++)
            {
                if (colIndex == 0) Console.Write("    ");
                Console.Write("+---+");
            }
        }

        private static void DrawUpperAlphabeticRow(int width)
        {
            for (var colIndex = 0; colIndex < width; colIndex++)
            {
                var c = (char)(65 + colIndex);
                if (colIndex == 0) Console.Write("    ");
                if (colIndex + 1 < 10) Console.Write($"  {c}  ");
                else if (colIndex + 1 < 100 && colIndex + 1 > 9) Console.Write($"  {c}  ");
                else Console.Write($"  {c}  ");
            }
        }

        public static void DrawSingleBoardWithPlacedBoats(ECellState[,] board)
        {
            var width = board.GetUpperBound(0) + 1; //x-axis
            var height = board.GetUpperBound(1) + 1; //y-axis
            
            Console.ForegroundColor = ConsoleColor.White;
            DrawUpperAlphabeticRow(width);
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.Blue;
            WriteUpperDashLine(width);
            Console.WriteLine();
            
            for (var rowIndex = 0; rowIndex < height; rowIndex++)
            {
                DrawBoardBody(rowIndex, width, board, true);
                Console.WriteLine();
                DrawLowerDashLine(width);
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.ResetColor();
        }
        
        public static void DrawBoardWithSingleBoat(ECellState[,] board, BoatDal boat)
        {
            
            var width = board.GetUpperBound(0) + 1; //x-axis
            var height = board.GetUpperBound(1) + 1; //y-axis
            
            Console.ForegroundColor = ConsoleColor.White;
            DrawUpperAlphabeticRow(width);
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.Blue;
            WriteUpperDashLine(width);
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
                    if (boat.IsLocatedHere(colIndex, rowIndex))
                    {
                        var cellState = "S";
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(cellState);
                        Console.ForegroundColor = ConsoleColor.Blue; 
                    }
                    else
                    {
                        var cellState = CellString(board[colIndex, rowIndex], true);
                        if (cellState.Equals("*")) Console.ForegroundColor = ConsoleColor.Gray;
                        else if (cellState.Equals("S")) Console.ForegroundColor = ConsoleColor.Green;
                        else if (cellState.Equals("X")) Console.ForegroundColor = ConsoleColor.Red;
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

        public static void ShowCurrentPlayerName(string playerName)
        {
            Console.Write(" Player ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{playerName}");
            Console.ResetColor();
            Console.Write(" move");
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void ShowEnemyBoardMessage(string playerName)
        {
            Console.Write(" Player ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{playerName}");
            Console.ResetColor();
            Console.Write(" board");
            Console.Write("\t\t\t\t\t  Your board");
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void ShowHitResponseMessage(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Wait();
        }

        public static void ShowNextMoveByMessage(string playerName)
        {
            Console.Clear();
            Console.WriteLine($"Next move by {playerName}");
            Wait();
        }
        
        public static void ShowWinnerAndLooser((string winner, string loser) winnerAndLooser)
        {
            Console.Clear();
            Console.WriteLine($"The winner is {winnerAndLooser.winner} and looser is {winnerAndLooser.loser}");
        }
        
        private static void Wait()
        {
            Thread.Sleep(2000);
        }

        public static void ShowQuitMessage()
        {
            Console.WriteLine("Press any key to quit..");
            Console.ReadKey();
        }
    }
}
