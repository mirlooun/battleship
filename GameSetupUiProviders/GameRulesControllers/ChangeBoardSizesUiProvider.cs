using System;
using System.Collections.Generic;
using DAL.DAL.Entities;
using DAL.Services;
using GameSetupUiProviders.Utils;
using Menu;

namespace GameSetupUiProviders.GameRulesControllers
{
    public class ChangeBoardSizesUiProvider : Menu.Menu
    {
        private readonly GameSettingsDal _gameSettings;

        private (bool IsCurrentlySettingUp, bool IsForHigh) _isSetting;

        private bool _changeSizesInSync = true;

        public ChangeBoardSizesUiProvider() : base(MenuLevel.LevelPlus, "Setup board sizes")
        {
            _gameSettings = GameSettingsController.GetGameSettings();
        }
        
        public override string Run()
        {
            AddGameSettingsToMenuItems();
            AddMenuLevelDefaultActions();

            ConsoleKey? keyPressed;
            var userChoice = "";

            Console.Clear();
            do
            {
                ResetCursorPosition();

                MenuUi.ShowSettingsLogo();
                MenuUi.ShowMenuLabel(Label);
                MenuUi.ShowMenuItems(MenuItems, PointerLocation);

                if (_isSetting.IsCurrentlySettingUp)
                {
                    SettingsUi.ShowPressKeyMessage();
                    SettingsUi.ShowPressEnterKeyMessageToSaveSettings();
                    keyPressed = SizeChanger(_isSetting.IsForHigh);

                    RefreshMenuItems(AddGameSettingsToMenuItems);
                    if (keyPressed == ConsoleKey.Enter)
                    {
                        _isSetting.IsCurrentlySettingUp = false;
                        GameSettingsController.SaveGameSettings();
                        keyPressed = null;
                        Console.Clear();
                        continue;
                    }
                }
                else
                {
                    MenuUi.ShowPressEnterKeyMessage();
                    MenuUi.ShowPressKeyMessage();
                    keyPressed = HandleKeyPress();
                }

                if (keyPressed != ConsoleKey.Enter) continue;
                var item = MenuItems[PointerLocation];
                userChoice = item.MethodToExecute();
            } while (
                keyPressed != ConsoleKey.Enter &&
                NotReturn(userChoice) ||
                MenuLevel == MenuLevel.Root &&
                NotExit(userChoice) ||
                IsDefault(userChoice)
            );

            Console.Clear();

            if (userChoice == "Return") userChoice = "";

            return !NotReturn(userChoice) ? "" : userChoice;
        }

        private void AddGameSettingsToMenuItems()
        {
            AddMenuItems(new List<MenuItem>
            {
                new(1, $"Change sizes in sync - {GetInSyncUiPresentation()}", () =>
                {
                    Console.Clear();
                    _changeSizesInSync = !_changeSizesInSync;
                    RefreshMenuItems(AddGameSettingsToMenuItems);
                    return "";
                }),
                new(2, $"Change height - current: {_gameSettings.FieldHeight}", () =>
                {
                    Console.Clear();
                    _isSetting = (true, true);
                    return "";
                }),
                new(3, $"Change height - current: {_gameSettings.FieldWidth}", () =>
                {
                    Console.Clear();
                    _isSetting = (true, false);
                    return "";
                })
            });
        }

        private string GetInSyncUiPresentation()
        {
            return _changeSizesInSync ? "yes" : "no";
        }

        private ConsoleKey SizeChanger(bool isHeight)
        {
            var keyPressed = Console.ReadKey(true);
            
            switch (keyPressed.Key)
            {
                case ConsoleKey.UpArrow:
                    if (_changeSizesInSync)
                    {
                        _gameSettings.FieldHeight++;
                        _gameSettings.FieldWidth++;
                    }
                    else if (isHeight)
                    {
                        _gameSettings.FieldHeight++;
                    }
                    else
                    {
                        _gameSettings.FieldWidth++;
                    }

                    break;
                case ConsoleKey.DownArrow:
                    if (_changeSizesInSync)
                    {
                        _gameSettings.FieldHeight--;
                        _gameSettings.FieldWidth--;
                    }
                    else if (isHeight)
                    {
                        _gameSettings.FieldHeight--;
                    }
                    else
                    {
                        _gameSettings.FieldWidth--;
                    }

                    break;
            }

            return keyPressed.Key;
        }
    }
}
