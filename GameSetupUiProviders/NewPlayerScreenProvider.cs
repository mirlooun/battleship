using System;
using DAL.DAL.Entities;
using GameSetupUiProviders.Helpers;
using GameSetupUiProviders.Utils;
using Menu;

namespace GameSetupUiProviders
{
    public static class NewPlayerScreenProvider
    {
        public static PlayerDal NewPlayerScreen(bool isFirst)
        {
            string playerName;
            
            PlayerValidatorResponse playerValidatorResponse;
            
            do
            {
                Console.Clear();
                MenuUi.ShowPlayerOrder(isFirst);
                MenuUi.ShowInitPlayerMessage(isFirst);
                playerName = Console.ReadLine()?.Trim()!;
                
                playerValidatorResponse = PlayerValidator.IsNameValid(playerName);

                if (!playerValidatorResponse.IsValid)
                {
                    WarningUi.ShowValidatorResponse(playerValidatorResponse.Message);
                }
                
            } while (!playerValidatorResponse.IsValid);

            var player = new PlayerDal(playerName);

            return player;
        }
    }
}
