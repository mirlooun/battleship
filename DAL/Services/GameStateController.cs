using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums.Rules;
using DAL.DTO;
using DAL.Mappers;
using Menu;

namespace DAL.Services
{
    public static class GameStateController
    {
        private static string _pathRoot = System.IO.Directory.GetCurrentDirectory();

        private static readonly JsonSerializerOptions? JsonOptions = new()
        {
            WriteIndented = true
        };

        private static string FileStandardDirectoryLocation => _pathRoot +
                                                               System.IO.Path.DirectorySeparatorChar +
                                                               "Saves";

        public static void SetInitialPath(string[] args)
        {
            _pathRoot = args.Length == 1 ? args[0] : _pathRoot;

            if (!System.IO.Directory.Exists(FileStandardDirectoryLocation))
            {
                System.IO.Directory.CreateDirectory(FileStandardDirectoryLocation);
            }
        }

        #region Local saving

        public static string SaveGameToLocal(GameEngine gameEngine)
        {
            var gameEngineDto = gameEngine.GetGameStateDto();
            
            var (p1, p2) = gameEngineDto.GetPlayerNamesInOrder();

            var saveFileName = FileStandardDirectoryLocation +
                               System.IO.Path.DirectorySeparatorChar +
                               $"p1={p1}_p2={p2}_save_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.json";
            
            gameEngineDto.SaveFilePath = saveFileName;

            var confJsonStr = JsonSerializer.Serialize(gameEngineDto, JsonOptions);
            
            if (!gameEngine.HasSaveFilePath() || 
                GameSettingsController
                    .GetGameSettings().SavingMethod == ESavingMethod.SaveIsNewSave)
            {
                gameEngine.SetSessionInfo(saveFileName);
                
                System.IO.File.WriteAllText(saveFileName, confJsonStr);
                return "saved";
            }
            
            var previousSaveName = gameEngine.GetGameStateDto().SaveFilePath;
            
            System.IO.File.WriteAllText(previousSaveName!, confJsonStr);
            System.IO.File.Move(previousSaveName!, saveFileName);
            return "updated";
        }
        private static GameEngine LoadGameFromLocal(string saveFilePath)
        {
            var saveText = System.IO.File.ReadAllText(saveFilePath);

            var gameEngineDto = JsonSerializer.Deserialize<GameEngineDto>(saveText)!;
            
            return GameEngineMapper.MapToDal(gameEngineDto);
        }
        public static List<MenuItem> GetGameSavesList(Func<GameEngine, string> delegateRunGame)
        {
            var menuItems = new List<MenuItem>();

            string[] filePaths = Directory.GetFiles(FileStandardDirectoryLocation);

            Array.Sort(filePaths,
                (file1, file2) => DateTime
                    .ParseExact(file1.Split("save_")[1].Split(".")[0], "dd-MM-yyyy_HH-mm-ss", null)
                    .CompareTo(DateTime.ParseExact(file2.Split("save_")[1].Split(".")[0], "dd-MM-yyyy_HH-mm-ss",
                        null)));

            Array.Reverse(filePaths);

            var i = 1;

            foreach (var file in filePaths)
            {
                var path = file.Split(System.IO.Path.DirectorySeparatorChar);
                var saveName = path[^1];
                menuItems.Add(new MenuItem(i, saveName, () =>
                {
                    var gameEngine = LoadGameFromLocal(file);
                    return delegateRunGame(gameEngine);
                }));
                i++;
            }

            return menuItems;
        }

        #endregion

        #region Database savings

        public static string SaveGameToDatabase(GameEngine gameEngine)
        {
            if (!gameEngine.HasSessionId() ||
                GameSettingsController.GetGameSettings().SavingMethod
                    .Equals(ESavingMethod.SaveIsNewSave))
            {
                DatabaseController.SaveGameToDatabase(gameEngine);
                return "saved";
            }

            DatabaseController.UpdateGameStateInDatabase(gameEngine);
            return "updated";
        }
        private static GameEngine LoadGameFromDb(int sessionId)
        {
            return DatabaseController.LoadGameFromDatabase(sessionId).Result;
        }
        public static List<MenuItem> GetGameSavesListFromDatabase(Func<GameEngine, string> delegateRunGame)
        {
            var sessions = DatabaseController.GetGameSessions();
            
            var menuItems = new List<MenuItem>();

            var i = 1;

            foreach (var session in sessions)
            {
                var sessionName = $"{session.Name}-{session.LastUpdate.ToString()}";
                menuItems.Add(
                    new MenuItem(i, sessionName, () =>
                    {
                        var gameEngine = LoadGameFromDb(session.Id);
                        return delegateRunGame(gameEngine);
                    }));
                i++;
            }

            return menuItems;
        }

        #endregion
    }
}