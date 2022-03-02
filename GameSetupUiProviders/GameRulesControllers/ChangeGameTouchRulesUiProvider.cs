using System;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums.Rules;
using DAL.Services;
using DAL.Utils;
using Menu;

namespace GameSetupUiProviders.GameRulesControllers
{
    public class ChangeGameTouchRulesUiProvider : Menu.Menu
    {
        private readonly GameSettingsDal _gameSettings;
        public ChangeGameTouchRulesUiProvider() : base(MenuLevel.LevelPlus, "Choose a game rule")
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
            foreach (EBoatsCanTouch rule in Enum.GetValues(typeof(EBoatsCanTouch)))
            {
                var uiName = _gameSettings.BoatsCanTouch == rule ?
                    GameRuleProvider.GetUiName(rule) + " *" : 
                    GameRuleProvider.GetUiName(rule);
                AddMenuItem(new MenuItem(i, uiName, () =>
                {
                    _gameSettings.BoatsCanTouch = rule;
                    GameSettingsController.SaveGameSettings();
                    RefreshMenuItems(AddGameSettingsToMenuItems);
                    return "";
                }));
                i++;
            }
        }
    }
}
