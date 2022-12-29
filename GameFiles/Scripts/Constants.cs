namespace TrailHunting.Scripts
{
    public static class Constants
    {
        // Files
        public const string SavedPlayerFileName = "user://player-save.json";

        // Levels
        public static string MainMenu = "res://Scenes/MainMenu.tscn";
        public static string TopDownStart = "res://Scenes/TopDownStart.tscn";
        public static string FirstPersonStart = "res://Scenes/FirstPersonStart.tscn";

        // Spawn Nodes
        public static string Spawn = "Spawn";
        public static string AirSpawn = "AirSpawn";
        public static string GroundSpawn = "GroundSpawn";
        public static string TimedSpawn = "TimedSpawn";

        // Game Variables
        public static int MaxAnimalSpawn = 3;
        public static int MaxTerrainSpawn = 5;
        public static int MaxAnimalFirstPersonSpawn = 10;
    }
}
