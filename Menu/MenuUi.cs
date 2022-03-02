using System;
using System.Collections.Generic;
using System.Threading;

namespace Menu
{
    public class MenuUi
    {
        public static void ShowMenuLabel(string label)
        {
            Console.WriteLine($"\n {label}\n");
        }

        public static void ShowMenuItems(List<MenuItem> menuItems, int pointerLocation)
        {
            foreach (var item in menuItems)
            {
                if (item.ItemIndex - 1 == pointerLocation)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(item);
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(item);
                }
            }

            Console.ResetColor();
        }

        public static void ShowPressKeyMessage()
        {
            Console.WriteLine("\n>> Use arrow keys to navigate");
        }

        

        public static void ShowInitPlayerMessage(bool isFirst)
        {
            var playerOrder = isFirst ? "first" : "second";
            Console.WriteLine($" Write down {playerOrder} player name");
            Console.WriteLine();
            Console.Write(">> ");
        }

        public static void ShowPlayerNameInContext(string playerName, Type type)
        {
            if (type.Name.Equals("NewPlayerBoatsUiProvider"))
            {
                Console.Write(" Player ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{playerName}");
                Console.ResetColor();
                Console.Write(" places boats");
            }

            Console.WriteLine();
        }

        public static void ShowMenuLabelInContext(Type type)
        {
            var showMessage = type.Name switch
            {
                "InitBoats" => " Choose a boat you want to place",
                "InitSingleBoat" => " Use arrows to place a boat",
                _ => ""
            };

            Console.WriteLine(showMessage);
        }
        public static void ShowBoatCountWarningMessage(string type)
        {
            Console.Clear();
            Console.WriteLine($"All boats of type {type} have been placed to board!");
            Wait();
        }
        public static void ShowGameLogo()
        {
            Console.Title = "Battleship Primordial";
            const string title = @"
__________         __    __  .__                .__    .__        
\______   \_____ _/  |__/  |_|  |   ____   _____|  |__ |__|_____  
 |    |  _/\__  \\   __\   __\  | _/ __ \ /  ___/  |  \|  \____ \ 
 |    |   \ / __ \|  |  |  | |  |_\  ___/ \___ \|   Y  \  |  |_> >
 |______  /(____  /__|  |__| |____/\___  >____  >___|  /__|   __/ 
        \/      \/                     \/     \/     \/   |__|    
";
            Console.WriteLine(title);
        }

        public static void ShowPlayerOrder(bool isFirst)
        {
            Console.Title = "Setup players";
            var player = isFirst
                ? @"
______ _                         __  
| ___ \ |                       /  | 
| |_/ / | __ _ _   _  ___ _ __  `| | 
|  __/| |/ _` | | | |/ _ \ '__|  | | 
| |   | | (_| | |_| |  __/ |    _| |_
\_|   |_|\__,_|\__, |\___|_|    \___/
                __/ |                
               |___/                 
"
                : @"
______ _                         _____ 
| ___ \ |                       / __  \
| |_/ / | __ _ _   _  ___ _ __  `' / /'
|  __/| |/ _` | | | |/ _ \ '__|   / /  
| |   | | (_| | |_| |  __/ |    ./ /___
\_|   |_|\__,_|\__, |\___|_|    \_____/
                __/ |                  
               |___/                   
";
            Console.WriteLine(player);
        }

        public static void ShowSettingsLogo()
        {
            Console.Title = "Settings";
            const string title = @"
 _____      _   _   _                 
/  ___|    | | | | (_)                
\ `--.  ___| |_| |_ _ _ __   __ _ ___ 
 `--. \/ _ \ __| __| | '_ \ / _` / __|
/\__/ /  __/ |_| |_| | | | | (_| \__ \
\____/ \___|\__|\__|_|_| |_|\__, |___/
                             __/ |    
                            |___/     
";
            Console.WriteLine(title);
        }

        public static void ShowPressRKeyMessage()
        {
            Console.WriteLine(" Press 'R' to rotate a boat");
        }

        public static void ShowLoadMenuLogo()
        {
            Console.Title = "Load games";
            const string title = @"
 _                     _                                   
| |                   | |                                  
| |     ___   __ _  __| |   __ _  __ _ _ __ ___   ___  ___ 
| |    / _ \ / _` |/ _` |  / _` |/ _` | '_ ` _ \ / _ \/ __|
| |___| (_) | (_| | (_| | | (_| | (_| | | | | | |  __/\__ \
\_____/\___/ \__,_|\__,_|  \__, |\__,_|_| |_| |_|\___||___/
                            __/ |                          
                           |___/                           
";
            Console.WriteLine(title);
        }

        public static void ShowPressEnterKeyMessage()
        {
            Console.WriteLine();
            Console.WriteLine("Press enter key to change a parameter");
        }
        
        private static void Wait(){
            Thread.Sleep(2000);
        }

        public static void ShowNoGameSavesAvailableMessage()
        {
            
        }
    }
    class MenuUiImpl : MenuUi
    {
    }
}
