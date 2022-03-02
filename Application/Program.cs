using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using DAL;
using DAL.DAL.Entities;
using DAL.Services;
using DAL.UiProviders;
using GameSetupUiProviders;
using Menu;

namespace Application
{
    internal static class Program
    {
        private static GameSettingsDal GameSettings => GameSettingsController.GetGameSettings();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        static void Main(string[] args)
        {
            SetConsoleWindowPosition();
            RunPreMenu(args);
            Run();
            Exit();
        }

        private static void RunPreMenu(string[] args)
        {
            var menu = new Menu.Menu(MenuLevel.PreInitialization, "Choose playing mode");
            menu.AddMenuItems(new List<MenuItem>
            {
                new(1, "Play local", () =>
                {
                    InitializeGameStateControllers(args);
                    GameSettingsController.SetInitialGameSettings();
                    GameSettingsController.SetLocalMode();
                    return "Return";
                }),
                new(2, "Play online", () =>
                {
                    InitializeGameStateControllers(args);
                    GameSettingsController.SetInitialGameSettings();
                    GameSettingsController.SetOnlineMode();
                    return "Return";
                }),
            });
            menu.Run();
        }

        private static void SetConsoleWindowPosition()
        {
            var ptr = GetConsoleWindow();
            MoveWindow(ptr, 300, 100, 900, 600, true);
        }

        private static void InitializeGameStateControllers(string[] path)
        {
            GameSettingsController.SetInitialPath(path);
            GameStateController.SetInitialPath(path);
        }

        private static void Run()
        {
            var menu = new Menu.Menu(MenuLevel.Root, "Main menu");
            menu.AddMenuItems(new()
            {
                new MenuItem(1, "Start new game", RunBattleship),
                new MenuItem(2, "Load game", LoadGame),
                new MenuItem(3, "Settings", RunSettingsMenu)
            });
            menu.Run();
        }

        private static string LoadGame()
        {
            var menu = new Menu.Menu(MenuLevel.LoadGame, "Load game menu");

            if (GameSettingsController.IsOnlineMode)
            {
                menu.AddMenuItems(GameStateController.GetGameSavesListFromDatabase(RunBattleship));
            }
            else
            {
                menu.AddMenuItems(GameStateController.GetGameSavesList(RunBattleship));
            }

            return menu.Run();
        }

        private static string RunBattleship()
        {
            // Player creation screens
            var playerA = NewPlayerScreenProvider.NewPlayerScreen(true);

            var playerB = NewPlayerScreenProvider.NewPlayerScreen(false);

            // Init board screens

            var pbProvider = new NewPlayerBoatsUiProvider(GameSettings);

            pbProvider.PlaceBoatsScreen(playerA);

            pbProvider.PlaceBoatsScreen(playerB);

            var gameEngine = new GameEngine(GameSettings, playerA, playerB);

            var game = new BattleshipGameUiProvider(gameEngine);

            return game.Run();
        }

        private static string RunSettingsMenu()
        {
            var menu = new SettingsUiProvider();
            return menu.Run();
        }

        private static string RunBattleship(GameEngine gameEngine)
        {
            var game = new BattleshipGameUiProvider(gameEngine);
            return game.Run();
        }

        private static void Exit()
        {
            Console.Clear();

            const string exitMsg = "Application is closing";
            Console.WriteLine(exitMsg);

            for (var i = 0; i < 3; i++)
            {
                Console.Write(".");
                Thread.Sleep(800);
            }
        }
    }
}