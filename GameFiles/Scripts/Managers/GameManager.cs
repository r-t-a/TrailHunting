using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TrailHunting.Scripts.Enums;
using TrailHunting.Scripts.Models;

namespace TrailHunting.Scripts.Managers
{
    public class GameManager
    {
        public static PlayerManager PlayerManager { get; private set; }
        public static Linker Linker { get; private set; }
        public static AcceptDialog ResultsDialog { get; private set; }

        private static int _smallGame = 4;
        private static int _mediumGame = 90;
        private static int _medLargeGame = 200;
        private static int _largeGame = 400;

        public GameManager(Linker linker)
        {
            Linker = linker;
            PlayerManager = new PlayerManager();
        }

        public static void End() 
        {
            Linker.GetTree().ChangeScene(Constants.MainMenu);
            Save();
        }

        public static void BuildTopDownResultsDialog(int smallCounter, int mediumCounter, int medLargeCounter, int largeCounter)
        {
            ResultsDialog = Linker.GetTree().Root.GetNode("TopDownStart").GetNodeOrNull<AcceptDialog>("CanvasLayer/AcceptDialog");
            var label = ResultsDialog.GetNodeOrNull<Label>("ResultsText");
            label.Text = $"Total Meat Hunted:{System.Environment.NewLine}{GetTotal(smallCounter, mediumCounter, medLargeCounter, largeCounter)}";
        }

        public static void BuildFirstPersonResultsDialog(int smallCounter, int mediumCounter, int medLargeCounter, int largeCounter)
        {
            ResultsDialog = Linker.GetTree().Root.GetNode("FirstPersonStart").GetNodeOrNull<AcceptDialog>("CanvasLayer/AcceptDialog");
            var label = ResultsDialog.GetNodeOrNull<Label>("ResultsText");
            label.Text = $"Total Meat Hunted:{System.Environment.NewLine}{GetTotal(smallCounter, mediumCounter, medLargeCounter, largeCounter)}";
        }

        private static int GetTotal(int smallCounter, int mediumCounter, int medLargeCounter, int largeCounter)
        {
            return (smallCounter * _smallGame) + (mediumCounter * _mediumGame) + (medLargeCounter * _medLargeGame) + (largeCounter * _largeGame);
        }

        public static void SetGameOption(bool isFirstPerson)
        {
            PlayerManager.IsFirstPersonStyle = isFirstPerson;
        }

        public static void SetGameType(bool isEndless)
        {
            PlayerManager.IsEndless = isEndless;
        }

        public static (MapType, List<int>) BuildTopDownLevel()
        {
            var getMapType = (MapType)new Random().Next(0, 4);
            var terrainList = new List<int>();
            var randomTerrainAmount = new Random().Next(0, Constants.MaxTerrainSpawn + 1);

            for (int i = 0; i <= randomTerrainAmount; i++)
            {
                switch (getMapType)
                {
                    default:
                    case MapType.Woods:
                        var randomWoodObj = new Random().Next(0, 4);
                        terrainList.Add(randomWoodObj);
                        break;
                    case MapType.Desert:
                        var randomDesertObj = new Random().Next(0, 2);
                        terrainList.Add(randomDesertObj);
                        break;
                    case MapType.Plains:
                        var randomPlainsObj = new Random().Next(0, 3);
                        terrainList.Add(randomPlainsObj);
                        break;
                    case MapType.Mountains:
                        var randomMountainObj = new Random().Next(0, 4);
                        terrainList.Add(randomMountainObj);
                        break;
                }
            }
            return (getMapType, terrainList);
        }

        public static string BuildFirstPersonLevel()
        {
            var getMapType = (MapType)new Random().Next(0, 5);
            switch(getMapType)
            {
                default:
                case MapType.Woods:
                    return "Woods";
                case MapType.Desert:
                    return "Desert";
                case MapType.Plains:
                    return "Plains";
                case MapType.Mountains:
                    return "Mountains";
            }
        }

        public static string GetSpawnPoint()
        {
            var spawnPoint = new Random().Next(1, 13);
            return $"{Constants.Spawn}{spawnPoint}";
        }

        public static string GetAirSpawnPoint()
        {
            var spawnPoint = new Random().Next(1, 7);
            return $"{Constants.AirSpawn}{spawnPoint}";
        }

        public static string GetGroundSpawnPoint()
        {
            var spawnPoint = new Random().Next(1, 10);
            return $"{Constants.GroundSpawn}{spawnPoint}";
        }

        public static string GetTimedSpawnPoint()
        {
            var spawnPoint = new Random().Next(1, 4);
            return $"{Constants.TimedSpawn}{spawnPoint}";
        }

        public static void Load()
        {
            try
            {
                var file = new File();
                if (file.FileExists(Constants.SavedPlayerFileName))
                {
                    file.Open(Constants.SavedPlayerFileName, File.ModeFlags.Read);
                    var data = JsonConvert.DeserializeObject<PlayerData>(file.GetAsText());
                    file.Close();
                    if (data != null && data is PlayerData player)
                    {
                        PlayerManager = new PlayerManager()
                        {
                            IsFirstPersonStyle = player.IsFirstPersonStyle,
                            IsEndless = player.IsEndless,
                            FirstPersonHighScore = player.FirstPersonHighScore,
                            TopDownHighScore = player.TopDownHighScore,
                        };
                    }
                }
                else
                {
                    GD.Print("No Saved Data");
                }
            }
            catch (Exception e)
            {
                GD.Print($"Exception: {e.InnerException.Message}");
            }
        }

        public static void Save()
        {
            try
            {
                var file = new File();
                var toJSON = JsonConvert.SerializeObject(PlayerData.ToPlayerData(PlayerManager), Formatting.Indented);
                file.Open(Constants.SavedPlayerFileName, File.ModeFlags.Write);
                file.StoreString(toJSON);
                file.Close();
            }
            catch (Exception e)
            {
                GD.Print($"Failed saving {e.InnerException.Message}");
            }
        }
    }
}