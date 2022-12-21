using Godot;
using System;
using System.Collections.Generic;
using TrailHunting.Scripts.Enums;

namespace TrailHunting.Scripts.Managers
{
    public class GameManager
    {
        public static Linker Linker { get; private set; }
        public static bool IsTypeA { get; private set; }

        public GameManager(Linker linker)
        {
            Linker = linker;
        }

        public static void End() 
        {
            //show results
            Linker.GetTree().ChangeScene(Constants.MainMenu);
        }

        public static void SetGameType()
        {
            IsTypeA = !IsTypeA;
        }

        public static (MapType, List<int>) BuildTopDownLevel()
        {
            var getMapType = (MapType)new Random().Next(0, 5);
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
            var spawnPoint = new Random().Next(1, 13);
            return $"{Constants.GroundSpawn}{spawnPoint}";
        }
    }
}
