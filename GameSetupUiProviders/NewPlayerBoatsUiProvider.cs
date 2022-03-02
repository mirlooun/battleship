using System;
using System.Collections.Generic;
using System.Linq;
using DAL.ConsoleUi;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums;
using DAL.Utils;
using Menu;

namespace GameSetupUiProviders
{
    public sealed class NewPlayerBoatsUiProvider : Menu.Menu
    {
        private Dictionary<EBoatType, int> _remainingBoatCount = default!;

        private List<BoatDal> _placedBoats = default!;

        private ECellState[,] _presentationalBoard = default!;

        private readonly GameSettingsDal _gameSettings;

        public NewPlayerBoatsUiProvider(GameSettingsDal gameSettings) : base(MenuLevel.InitBoats, "")
        {
            _gameSettings = gameSettings;
        }

        public void PlaceBoatsScreen(PlayerDal player)
        {
            _placedBoats = new List<BoatDal>(_gameSettings.BoatsConfig.Count);
            GetPresentationalBoard();
            GetInitialBoatsList();
            AddBoatsToMenuItems();

            Console.Clear();
            do
            {
                ResetCursorPosition();
                BattleshipUi.DrawSingleBoardWithPlacedBoats(_presentationalBoard);
                MenuUi.ShowPlayerNameInContext(player.Name, typeof(NewPlayerBoatsUiProvider));
                MenuUi.ShowMenuLabelInContext(typeof(NewPlayerBoatsUiProvider));
                MenuUi.ShowMenuItems(MenuItems, PointerLocation);
                MenuUi.ShowPressKeyMessage();

                ConsoleKey? keyPressed = HandleKeyPress();

                if (keyPressed != ConsoleKey.Enter) continue;
                var item = MenuItems[PointerLocation];
                item.MethodToExecute();
            } while (!IsAllBoatsPlaced());
            Console.Clear();

            player.SetPlayerBoats(_placedBoats);

            ResetMenu();
        }
        private void ResetMenu()
        {
            MenuItems.Clear();
            PointerLocation = 0;
        }

        private void GetPresentationalBoard()
        {
            _presentationalBoard = new ECellState[_gameSettings.FieldHeight, _gameSettings.FieldWidth];
        }

        private void GetInitialBoatsList()
        {
            _remainingBoatCount = _gameSettings.GetBoatsConfiguration();
        }

        private bool IsAllBoatsPlaced()
        {
            return _remainingBoatCount.Values.Sum() == 0;
        }

        private void AddBoatsToMenuItems()
        {
            var i = 1;
            var items = new List<MenuItem>();
            foreach (EBoatType bt in Enum.GetValues(typeof(EBoatType)))
            {
                if (!_remainingBoatCount.ContainsKey(bt)) continue;

                string toDraw = BoatTypeProvider.GetUiName(bt) + " - x" + _remainingBoatCount[bt];
                items.Add(new MenuItem(i, toDraw, () => AddBoatLocation(bt)));
                i++;
            }

            AddMenuItems(items);
        }

        private string AddBoatLocation(EBoatType boatType)
        {
            if (_remainingBoatCount[boatType] == 0)
            {
                MenuUi.ShowBoatCountWarningMessage(BoatTypeProvider.GetUiName(boatType));
            }
            else
            {
                var bpProvider = new BoatPlacementScreenProvider(_presentationalBoard, _gameSettings);
                var newBoat = bpProvider.PlaceBoatScreen(boatType);
                AddCurrentBoatToBoatsList(newBoat);
                AddCurrentBoatToPresentationalBoard(newBoat);
                RemoveBoatCountOfPlacedBoat(boatType);
                RefreshMenuItems(AddBoatsToMenuItems);
            }

            return "";
        }

        private void RemoveBoatCountOfPlacedBoat(EBoatType boatType)
        {
            _remainingBoatCount[boatType]--;
        }

        private void AddCurrentBoatToBoatsList(BoatDal newBoat)
        {
            newBoat.SetPlaced();
            _placedBoats.Add(newBoat);
        }

        private void AddCurrentBoatToPresentationalBoard(BoatDal boat)
        {
            foreach (var point in boat.Locations)
            {
                _presentationalBoard[point.X, point.Y] = ECellState.Ship;
            }
        }
    }
}
