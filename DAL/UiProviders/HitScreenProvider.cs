using System;
using DAL.ConsoleUi;
using DAL.DAL.Entities;
using DAL.Helpers;
using Menu;

namespace DAL.UiProviders
{
    public sealed class HitScreenProvider : Menu.Menu
    {
        private readonly GameEngine _gameEngine;

        private LocationDal _hitLocation = new(0, 0);
        public HitScreenProvider(GameEngine gameEngine) : base(MenuLevel.LevelPlus, "")
        {
            _gameEngine = gameEngine;
        }

        public override string Run()
        {
            ConsoleKey? keyPressed;

            Console.Clear();
            do
            {
                ResetCursorPosition();
                HitMenuUi.DrawSingleBoard(_gameEngine.GetCurrentEnemyBoardState(), _hitLocation);
                MenuUi.ShowMenuItems(MenuItems, PointerLocation);
                HitMenuUi.ShowLegend();
                MenuUi.ShowPressKeyMessage();

                keyPressed = HandleKeyPress();

                if (keyPressed != ConsoleKey.Enter) continue;

                var response = _gameEngine.MakeAHit(new LocationDal
                (
                    _hitLocation.X,
                    _hitLocation.Y
                ));

                if (response.IsSamePlayerMove(_gameEngine.GetGameSettings().HitContinuousMove))
                {
                    BattleshipUi.ShowHitResponseMessage(response.GetMessage());
                    keyPressed = null;
                }
                else
                {
                    BattleshipUi.ShowHitResponseMessage(response.GetMessage());
                }

                if (_gameEngine.HasWinner()) return "Winner";
            } while (
                keyPressed != ConsoleKey.Enter
            );

            Console.Clear();

            return "HitResponse";
        }

        protected override ConsoleKey HandleKeyPress()
        {
            var keyPressed = Console.ReadKey(true);

            switch (keyPressed.Key)
            {
                case ConsoleKey.UpArrow:
                    HitLocationChanger.TryDeltaMove(ref _hitLocation, 0, -1, _gameEngine.GetGameSettings());
                    break;
                case ConsoleKey.DownArrow:
                    HitLocationChanger.TryDeltaMove(ref _hitLocation, 0, +1, _gameEngine.GetGameSettings());
                    break;
                case ConsoleKey.RightArrow:
                    HitLocationChanger.TryDeltaMove(ref _hitLocation, +1, 0, _gameEngine.GetGameSettings());
                    break;
                case ConsoleKey.LeftArrow:
                    HitLocationChanger.TryDeltaMove(ref _hitLocation, -1, 0, _gameEngine.GetGameSettings());
                    break;
            }

            return keyPressed.Key;
        }
    }
}
