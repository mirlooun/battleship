using System.Collections.Generic;
using System.Text.Json;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums;
using DAL.DAL.Entities.Enums.Rules;
using DAL.DTO;

namespace DAL.Services
{
    public static class GameSettingsController
    {
        private static string _pathRoot = System.IO.Directory.GetCurrentDirectory();

        private static GameSettingsDal? _gameSettings;

        public static bool IsOnlineMode { get; private set; }

        private static readonly JsonSerializerOptions? JsonOptions = new()
        {
            WriteIndented = true
        };

        private static string FileStandardPath => FileStandardDirectoryLocation +
                                                  System.IO.Path.DirectorySeparatorChar +
                                                  "gameConfig.json";

        private static string FileStandardDirectoryLocation => _pathRoot +
                                                               System.IO.Path.DirectorySeparatorChar +
                                                               "Configs";

        public static GameSettingsDal GetGameSettings()
        {
            if (_gameSettings != null) return _gameSettings;

            if (System.IO.File.Exists(FileStandardPath))
            {
                var confText = System.IO.File.ReadAllText(FileStandardPath);
                _gameSettings = JsonSerializer.Deserialize<GameSettingsDal>(confText);
            }
            else
            {
                var defaultSettings = GetDefaultGameSettings();

                var confJsonStr = JsonSerializer.Serialize(defaultSettings, JsonOptions);

                System.IO.File.WriteAllText(FileStandardPath, confJsonStr);

                _gameSettings = defaultSettings;
            }

            return _gameSettings!;
        }

        public static void SaveGameSettings()
        {
            var confJsonStr = JsonSerializer.Serialize(_gameSettings, JsonOptions);
            System.IO.File.WriteAllText(FileStandardPath, confJsonStr);
        }

        public static void SetInitialPath(string[] args)
        {
            _pathRoot = args.Length == 1 ? args[0] : _pathRoot;

            if (!System.IO.Directory.Exists(FileStandardDirectoryLocation))
            {
                System.IO.Directory.CreateDirectory(FileStandardDirectoryLocation);
            }
        }

        private static GameSettingsDal GetDefaultGameSettings()
        {
            return new GameSettingsDal
            {
                FieldHeight = 10,
                FieldWidth = 10,
                BoatsCanTouch = EBoatsCanTouch.BoatsCanTouch,
                HitContinuousMove = EHitContinuousMove.HitContinuousMove,
                BoatsConfig = GetDefaultBoatConfiguration()
            };
        }

        public static GameSettingsDto GetDefaultGameSettingsDto()
        {
            return new GameSettingsDto()
            {
                FieldHeight = 10,
                FieldWidth = 10,
                BoatsCanTouch = EBoatsCanTouch.BoatsCanTouch,
                HitContinuousMove = EHitContinuousMove.HitContinuousMove
            };
        }

        public static List<BoatConfigurationDto> GetDefaultBoatConfiguration()
        {
            return new List<BoatConfigurationDto>
            {
                new() { BoatType = EBoatType.Carrier, BoatCount = 1 },
                new() { BoatType = EBoatType.Battleship, BoatCount = 1 },
                new() { BoatType = EBoatType.Cruiser, BoatCount = 1 },
                new() { BoatType = EBoatType.Submarine, BoatCount = 1 },
                new() { BoatType = EBoatType.Patrol, BoatCount = 1 }
            };
        }

        public static void SetInitialGameSettings()
        {
            _gameSettings = GetGameSettings();
        }

        public static void SetLocalMode()
        {
            IsOnlineMode = false;
        }

        public static void SetOnlineMode()
        {
            IsOnlineMode = true;
        }
    }
}
