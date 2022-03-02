using System;
using System.Collections.Generic;
using DAL.ConsoleUi;
using DAL.Services;
using Menu;

namespace DAL.UiProviders
{
    public sealed class BattleshipGameUiProvider : Menu.Menu
    {
        private readonly GameEngine _gameEngine;

        public BattleshipGameUiProvider(GameEngine gameEngine) : base(MenuLevel.Battleship, "Battleship")
        {
            _gameEngine = gameEngine;
        }

        public override string Run()
        {
            if (_gameEngine.HasWinner())
            {
                BattleshipUi.ShowWinnerAndLooser(_gameEngine.GetWinnerAndLoser());
                BattleshipUi.ShowQuitMessage();
                
                return "Return";
            }
            
            AddMenuActions();

            var userChoice = "";

            Console.Clear();
            do
            {
                ResetCursorPosition();
                BattleshipUi.DrawBoards(_gameEngine.GetBoards());
                BattleshipUi.ShowEnemyBoardMessage(_gameEngine.GetCurrentEnemyName());
                BattleshipUi.ShowCurrentPlayerName(_gameEngine.GetCurrentAttackerName());
                MenuUi.ShowMenuItems(MenuItems, PointerLocation);
                MenuUi.ShowPressKeyMessage();

                ConsoleKey? keyPressed = HandleKeyPress();

                if (keyPressed != ConsoleKey.Enter) continue;

                var item = MenuItems[PointerLocation];
                userChoice = item.MethodToExecute();

                if (userChoice.Equals("Return"))
                {
                    continue;
                }

                switch (userChoice)
                {
                    case "HitResponse":
                        BattleshipUi.ShowNextMoveByMessage(_gameEngine.GetCurrentAttackerName());
                        break;
                    case "Winner":
                        BattleshipUi.ShowWinnerAndLooser(_gameEngine.GetWinnerAndLoser());
                        BattleshipUi.ShowQuitMessage();
                        
                        if (GameSettingsController.IsOnlineMode)
                        {
                            GameStateController.SaveGameToDatabase(_gameEngine);
                        }
                        else
                        {
                            GameStateController.SaveGameToLocal(_gameEngine);
                        }
                        break;
                }
            } while (
                NotReturnToMain(userChoice) && !_gameEngine.HasWinner()
            );

            Console.Clear();

            return userChoice;
        }

        private string MakeAHit()
        {
            var hitMenu = new HitScreenProvider(_gameEngine);
            return hitMenu.Run();
        }

        private string ShowHitHistory()
        {
            var hitMenu = new HitHistoryScreenProvider(_gameEngine);
            return hitMenu.Run();
        }

        private string SaveGameStateToLocal()
        {
            var response = GameStateController.SaveGameToLocal(_gameEngine);
            Console.Clear();
            Console.WriteLine($"Game is {response}");
            Wait();
            return response;
        }

        private string SaveGameStateToDb()
        {
            var response = GameStateController.SaveGameToDatabase(_gameEngine);
            Console.Clear();
            Console.WriteLine($"Game is {response}");
            Wait();
            return response;
        }

        private void AddMenuActions()
        {
            AddMenuItems(new List<MenuItem>
            {
                new(1, "Make a move", MakeAHit),
                new(2, "Revert a move", ShowHitHistory),
                GameSettingsController.IsOnlineMode
                    ? new MenuItem(3, "Save a game to database", SaveGameStateToDb)
                    : new MenuItem(3, "Save a game", SaveGameStateToLocal),
                new(4, "Return to main menu", () => "Return to main")
            });
        }
    }
}