using System;
using DAL.ConsoleUi;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums;
using GameSetupUiProviders.Helpers;
using GameSetupUiProviders.Utils;
using Menu;

namespace GameSetupUiProviders
{
    public sealed class BoatPlacementScreenProvider : Menu.Menu
    {
        private readonly ECellState[,] _presentationalBoard;
        private readonly GameSettingsDal _gameSettings;
        private BoatDal? _preliminaryBoat;

        public BoatPlacementScreenProvider(ECellState[,] presentationalBoard, GameSettingsDal gameSettings) : base(
            MenuLevel.InitBoats, "")
        {
            _presentationalBoard = presentationalBoard;
            _gameSettings = gameSettings;
        }

        public BoatDal PlaceBoatScreen(EBoatType boatType)
        {
            _preliminaryBoat = new BoatDal(boatType);

            var response = new BoatLocationValidatorResponse();

            Console.Clear();
            do
            {
                ResetCursorPosition();
                BattleshipUi.DrawBoardWithSingleBoat(_presentationalBoard, _preliminaryBoat);
                MenuUi.ShowMenuLabelInContext(typeof(BoatPlacementScreenProvider));
                MenuUi.ShowPressRKeyMessage();
                MenuUi.ShowPressKeyMessage();

                ConsoleKey? keyPressed = HandleKeyPress();

                if (keyPressed != ConsoleKey.Enter) continue;

                response = BoatLocationValidator.IsBoatLocationOccupied(
                    _presentationalBoard,
                    _preliminaryBoat,
                    _gameSettings.BoatsCanTouch
                );

                if (!response.IsValid)
                {
                    WarningUi.ShowWarningMessage(response.Message);
                }
            } while (!response.IsValid);

            Console.Clear();

            return _preliminaryBoat;
        }

        protected override ConsoleKey HandleKeyPress()
        {
            var keyPressed = Console.ReadKey(true);

            switch (keyPressed.Key)
            {
                case ConsoleKey.UpArrow:
                    BoatLocationChanger.TryDeltaMove(ref _preliminaryBoat!, 0, -1, _gameSettings);
                    break;
                case ConsoleKey.DownArrow:
                    BoatLocationChanger.TryDeltaMove(ref _preliminaryBoat!, 0, +1, _gameSettings);
                    break;
                case ConsoleKey.RightArrow:
                    BoatLocationChanger.TryDeltaMove(ref _preliminaryBoat!, +1, 0, _gameSettings);
                    break;
                case ConsoleKey.LeftArrow:
                    BoatLocationChanger.TryDeltaMove(ref _preliminaryBoat!, -1, 0, _gameSettings);
                    break;
                case ConsoleKey.R:
                    BoatLocationChanger.TryRotate(ref _preliminaryBoat!, _gameSettings);
                    break;
            }

            return keyPressed.Key;
        }
    }
}
