using System;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums.Rules;
using DAL.Services;
using DAL.Utils;
using Menu;

namespace GameSetupUiProviders.GameRulesControllers
{
    public class ChangeGameRulesHitUiProvider : Menu.Menu
    {
        private readonly GameSettingsDal _gameSettings;
        public ChangeGameRulesHitUiProvider() : base(MenuLevel.LevelPlus, "Choose a game rule")
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
            foreach (EHitContinuousMove rule in Enum.GetValues(typeof(EHitContinuousMove)))
            {
                var uiName = _gameSettings.HitContinuousMove == rule ?
                    GameRuleProvider.GetUiName(rule) + " *" : 
                    GameRuleProvider.GetUiName(rule);
                AddMenuItem(new MenuItem(i, uiName, () =>
                {
                    _gameSettings.HitContinuousMove = rule;
                    GameSettingsController.SaveGameSettings();
                    RefreshMenuItems(AddGameSettingsToMenuItems);
                    return "";
                }));;
                i++;
            }
        }
    }
}
