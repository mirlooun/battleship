using System.Collections.Generic;
using DAL.DAL.Entities;
using DAL.Services;
using DAL.Utils;
using GameSetupUiProviders.GameRulesControllers;
using Menu;

namespace GameSetupUiProviders
{
    public class SettingsUiProvider : Menu.Menu
    {
        private readonly GameSettingsDal _gameSettings;
        public SettingsUiProvider() : base(MenuLevel.Level1, "Game settings")
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
            AddMenuItems(new List<MenuItem>
            {
                new (1, $"Change board sizes - height:{_gameSettings.FieldHeight} width:{_gameSettings.FieldWidth}", ChangeBoardSizes),
                new (2, $"Change boat touch rules - {GameRuleProvider.GetUiName(_gameSettings.BoatsCanTouch)}", ChangeTouchingRules),
                new (3, $"Change hit making rules - {GameRuleProvider.GetUiName(_gameSettings.HitContinuousMove)}", ChangeHitMakingRules),
                new (4, $"Change saving rules - {GameRuleProvider.GetUiName(_gameSettings.SavingMethod)}", ChangeSavingRules),
                new (5, "About", DefaultMenuAction)
            });
        }
        private string ChangeBoardSizes()
        {
            var menu = new ChangeBoardSizesUiProvider();
            var userChoice = menu.Run();
            RefreshMenuItems(AddGameSettingsToMenuItems);
            return userChoice;
        }
        private string ChangeTouchingRules()
        {
            var menu = new ChangeGameTouchRulesUiProvider();
            var userChoice = menu.Run();
            RefreshMenuItems(AddGameSettingsToMenuItems);
            return userChoice;
        }

        private string ChangeHitMakingRules()
        {
            var menu = new ChangeGameRulesHitUiProvider();
            var userChoice = menu.Run();
            RefreshMenuItems(AddGameSettingsToMenuItems);
            return userChoice;
        }
        
        private string ChangeSavingRules()
        {
            var menu = new ChangeSavingMethodUiProvider();
            var userChoice = menu.Run();
            RefreshMenuItems(AddGameSettingsToMenuItems);
            return userChoice;
        }
    }
}
