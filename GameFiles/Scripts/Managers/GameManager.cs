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
        public static int TopDownPlayerSpeed { get; private set; } = 10;
        public static int TopDownBulletSpeed { get; private set; }
        public static int MaxAnimalSpawn { get; private set; }
        public static int MaxTerrainSpawn { get; private set; }
        public static int MaxAnimalFirstPersonSpawn { get; private set; }
        public static int FlintlockAmmo { get; private set; }
        public static int RepeaterAmmo { get; private set; }
        public static int PistolAmmo { get; private set; }
        public static int SmallGame { get; private set; }
        public static int MediumGame { get; private set; }
        public static int MediumLargeGame { get; private set; }
        public static int LargeGame { get; private set; }
        public static int ModeAHighScore { get; private set; }
        public static int ModeBHighScore { get; private set; }
        public static int CurrentScore { get; private set; }

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

        public static void ResetGame()
        {
            CurrentScore = 0;
        }

        public static void UpdateScore(int pts)
        {
            CurrentScore += pts;
        }

        public static void UpdatePlayerStats(Animals animalShot)
        {
            switch (animalShot)
            {
                case Animals.Squirrel:
                    PlayerManager.SquirrelTotal += 1;
                    UpdateScore(100);
                    break;
                case Animals.Rabbit:
                    PlayerManager.RabbitTotal += 1;
                    UpdateScore(100);
                    break;
                case Animals.Doe:
                    PlayerManager.DoeTotal += 1;
                    UpdateScore(300);
                    break;
                case Animals.Buck:
                    PlayerManager.BuckTotal += 1;
                    UpdateScore(1000);
                    break;
                case Animals.Bear:
                    PlayerManager.BearTotal += 1;
                    UpdateScore(2000);
                    break;
                case Animals.Buffalo:
                    PlayerManager.BuffaloTotal += 1;
                    UpdateScore(5000);
                    break;
                case Animals.Caribou:
                    PlayerManager.CaribouTotal += 1;
                    UpdateScore(1000);
                    break;
                case Animals.Elk:
                    PlayerManager.ElkTotal += 1;
                    UpdateScore(4000);
                    break;
                case Animals.Duck:
                    PlayerManager.DuckTotal += 1;
                    UpdateScore(100);
                    break;
                case Animals.Goose:
                    PlayerManager.GooseTotal += 1;
                    UpdateScore(100);
                    break;
            }
        }

        public static void BuildFirstPersonResultsDialog(int smallCounter, int mediumCounter, int medLargeCounter, int largeCounter)
        {
            ResultsDialog = Linker.GetTree().Root.GetNode("FirstPersonStart").GetNodeOrNull<AcceptDialog>("CanvasLayer/AcceptDialog");
            var label = ResultsDialog.GetNodeOrNull<Label>("ResultsText");
            label.Text = $"Total Meat Hunted:{System.Environment.NewLine}{GetTotal(smallCounter, mediumCounter, medLargeCounter, largeCounter)}";
        }

        private static int GetTotal(int smallCounter, int mediumCounter, int medLargeCounter, int largeCounter)
        {
            return (smallCounter * SmallGame) + (mediumCounter * MediumGame) + (medLargeCounter * MediumLargeGame) + (largeCounter * LargeGame);
        }

        public static (MapType, List<int>) BuildTopDownLevel()
        {
            var getMapType = (MapType)new Random().Next(0, 4);
            var terrainList = new List<int>();
            var randomTerrainAmount = new Random().Next(0, MaxTerrainSpawn + 1);

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

        public static string GetSpawnPoint()
        {
            var spawnPoint = new Random().Next(1, 11);
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
                            ModeAHighScore = player.ModeAHighScore,
                            ModeBHighScore = player.ModeBHighScore,
                            FirearmType = player.FirearmType,
                            FirstPersonTotal = player.FirstPersonTotal,
                            TopDownTotal = player.TopDownTotal,
                            SquirrelTotal = player.SquirrelTotal,
                            RabbitTotal = player.RabbitTotal,
                            DoeTotal = player.DoeTotal,
                            BuckTotal = player.BuckTotal,
                            CaribouTotal = player.CaribouTotal,
                            ElkTotal = player.ElkTotal,
                            BearTotal = player.BearTotal,
                            BuffaloTotal = player.BuffaloTotal,
                            DuckTotal = player.DuckTotal,
                            GooseTotal = player.GooseTotal,
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

        public static void LoadVariables()
        {
            try
            {
                var file = new File();
                if (file.FileExists(Constants.GameDataFileName))
                {
                    file.Open(Constants.GameDataFileName, File.ModeFlags.Read);
                    var data = JsonConvert.DeserializeObject<GameData>(file.GetAsText());
                    file.Close();
                    if (data != null && data is GameData gameData)
                    {
                        TopDownBulletSpeed = gameData.TopDownBulletSpeed;
                        TopDownPlayerSpeed = gameData.TopDownPlayerSpeed;
                        MaxAnimalSpawn = gameData.MaxAnimalSpawn;
                        MaxTerrainSpawn = gameData.MaxTerrainSpawn;
                        MaxAnimalFirstPersonSpawn = gameData.MaxAnimalFirstPersonSpawn;
                        FlintlockAmmo = gameData.FlintlockAmmo;
                        RepeaterAmmo = gameData.RepeaterAmmo;
                        PistolAmmo = gameData.PistolAmmo;
                        SmallGame = gameData.SmallGame;
                        MediumGame = gameData.MediumGame;
                        MediumLargeGame = gameData.MediumLargeGame;
                        LargeGame = gameData.LargeGame;
                    }
                }
                else
                {
                    GD.Print("No Saved Data");
                    TopDownBulletSpeed = 130;
                    TopDownPlayerSpeed = 80;
                    MaxAnimalSpawn = 3;
                    MaxTerrainSpawn = 6;
                    MaxAnimalFirstPersonSpawn = 10;
                    FlintlockAmmo = 20;
                    SmallGame = 4;
                    MediumGame = 90;
                    MediumLargeGame = 200;
                    LargeGame = 400;
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