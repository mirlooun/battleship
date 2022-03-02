using System;
using System.Threading;
using Menu;

namespace DAL.UiProviders
{
    public class HitHistoryScreenProvider : Menu.Menu
    {
        private readonly GameEngine _gameEngine;
        public HitHistoryScreenProvider(GameEngine gameEngine) : base(MenuLevel.Battleship, "Hit history")
        {
            _gameEngine = gameEngine;
        }

        public override string Run()
        {
            ConsoleKey? keyPressed;
            AddHitsToMenuItems();
            
            string userChoice = "";
            
            Console.Clear();
            do
            {
                ResetCursorPosition();
                MenuUi.ShowGameLogo();
                if (_gameEngine.GetHitHistory().Count == 0)
                {
                    Console.WriteLine("Currently there are no moves to which revert a game state\n");
                }
                else
                {
                    Console.WriteLine("Choose a move you want to revert a game state");
                }
                MenuUi.ShowMenuItems(MenuItems, PointerLocation);
                MenuUi.ShowPressKeyMessage();

                keyPressed = HandleKeyPress();
                
                if (keyPressed != ConsoleKey.Enter) continue;
                
                var item = MenuItems[PointerLocation];
                userChoice = item.MethodToExecute();
                
            } while (
                keyPressed != ConsoleKey.Enter
            );
            Console.Clear();

            return userChoice;
        }
        private void AddHitsToMenuItems()
        {
            var i = 1;
         
            foreach (var hitRecord in _gameEngine.GetHitHistory())
            {
                AddMenuItem(new MenuItem(i, hitRecord.Info, () =>
                {
                    _gameEngine.RevertMovesToSpecified(hitRecord);
                    Console.Clear();
                    Console.WriteLine("Reverted");
                    Thread.Sleep(2000);
                    return "";
                }));
                i++;
            }
            AddMenuItem(new(i, "Return", () => "Return"));
        }
    }
}