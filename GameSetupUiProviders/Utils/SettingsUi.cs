using System;

namespace GameSetupUiProviders.Utils
{
    public static class SettingsUi
    {
        public static void ShowPressKeyMessage()
        {
            Console.WriteLine();
            Console.WriteLine("Use up and down arrow keys to change sizes");
        }
        
        public static void ShowPressEnterKeyMessageToSaveSettings()
        {
            Console.WriteLine();
            Console.WriteLine("Press enter to save settings");
        }
    }
}
