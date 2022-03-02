using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Menu;

namespace Menu
{
    public class Menu : BaseMenu, IMenu
    {
        protected readonly MenuLevel MenuLevel;

        protected readonly string Label;
        protected readonly List<MenuItem> MenuItems = new();
        protected int PointerLocation;
        public Menu(MenuLevel level, string label)
        {
            MenuLevel = level;
            Label = label;
        }
        public virtual string Run()
        {
            AddMenuLevelDefaultActions();
            
            ConsoleKey? keyPressed;
            var userChoice = "";
            
            Console.Clear();
            do
            {
                ResetCursorPosition();
                ShowMenuLevelSpecificTitle();
                MenuUi.ShowMenuLabel(Label);
                MenuUi.ShowMenuItems(MenuItems, PointerLocation);
                MenuUi.ShowPressKeyMessage();
                
                keyPressed = HandleKeyPress();
                if (keyPressed != ConsoleKey.Enter) continue;
                var item = MenuItems.ElementAt(PointerLocation);
                userChoice = item!.MethodToExecute();

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

        private void ShowMenuLevelSpecificTitle()
        {
            if (MenuLevel == MenuLevel.Level1)
            {
                MenuUi.ShowSettingsLogo();
            }
            else if (MenuLevel == MenuLevel.LoadGame)
            {
                MenuUi.ShowLoadMenuLogo();
                if (MenuItems.Count == 2)
                {
                    Console.WriteLine("Currently there are no game saves available...");
                }
            }
            else
            {
                MenuUi.ShowGameLogo();
            }
        }
        protected virtual ConsoleKey HandleKeyPress()
        {
            var keyPressed = Console.ReadKey(true);

            switch (keyPressed.Key)
            {
                case ConsoleKey.UpArrow:
                    PointerLocation = PointerLocation == 0
                        ? PointerLocation = MenuItems.Count - 1
                        : --PointerLocation;
                    break;
                case ConsoleKey.DownArrow:
                    PointerLocation = PointerLocation == MenuItems.Count - 1
                        ? PointerLocation = 0
                        : ++PointerLocation;
                    break;
            }

            return keyPressed.Key;
        }
        protected void AddMenuLevelDefaultActions()
        {
            switch (MenuLevel)
            {
                case MenuLevel.Level1 or MenuLevel.LevelPlus or MenuLevel.LoadGame:
                    AddMenuItem(new MenuItem(MenuItems.Count + 1, "Return", () => "Return"));
                    if (MenuLevel == MenuLevel.LevelPlus)
                    {
                        AddMenuItem(new MenuItem(MenuItems.Count + 1, "Return to main", () => "Return to main"));
                    }
                    AddMenuItem(new MenuItem(MenuItems.Count + 1, "Exit", () => "Exit"));
                    break;
                case MenuLevel.Root:
                    AddMenuItem(new MenuItem(MenuItems.Count + 1, "Exit", () => "Exit"));
                    break;
            }
        }
        protected void AddMenuItem(MenuItem menuItem)
        {
            MenuItems.Add(menuItem);
        }
        public void AddMenuItems(List<MenuItem> menuItems)
        {
            foreach (var menuItem in menuItems)
            {
                MenuItems.Add(menuItem);    
            }
        }
        protected static string DefaultMenuAction()
        {
            Console.Clear();
            Console.WriteLine("Choice not implemented yet..");
            Wait();
            return "";
        }
        protected void RefreshMenuItems(Action addCustomMenuItems)
        {
            MenuItems.Clear();
            addCustomMenuItems();
            AddMenuLevelDefaultActions();
            Console.Clear();
        }
    }
}
