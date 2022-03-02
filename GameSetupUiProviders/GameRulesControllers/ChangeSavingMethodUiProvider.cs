using System;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums.Rules;
using DAL.Services;
using DAL.Utils;
using Menu;

namespace GameSetupUiProviders.GameRulesControllers
{
    public class ChangeSavingMethodUiProvider : Menu.Menu
    {
        private readonly GameSettingsDal _gameSettings;

        public ChangeSavingMethodUiProvider() : base(MenuLevel.LevelPlus, "Choose a game rule")
        {
            _gameSettings = GameSettingsController.GetGameSettings();
        }

        public override string Run()
        {
            AddGameSettingsToMenuItems();
            return base.Run();
        }

        private void AddGameSettingsToMenuItems()
        {
            var i = 1;
            foreach (ESavingMethod rule in Enum.GetValues(typeof(ESavingMethod)))
            {
                var uiName = _gameSettings.SavingMethod == rule
                    ? GameRuleProvider.GetUiName(rule) + " *"
                    : GameRuleProvider.GetUiName(rule);
                AddMenuItem(new MenuItem(i, uiName, () =>
                {
                    _gameSettings.SavingMethod = rule;
                    GameSettingsController.SaveGameSettings();
                    RefreshMenuItems(AddGameSettingsToMenuItems);
                    return "";
                }));
                ;
                i++;
            }
        }
    }
}
