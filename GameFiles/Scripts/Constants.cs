namespace TrailHunting.Scripts
{
    public static class Constants
    {
        // Files
        public const string SavedPlayerFileName = "user://player-save.json";
        public const string GameDataFileName = "res://Scripts/Data/gamedata.json";

        // Main Scenes
        public static string MainMenu = "res://Scenes/MainMenu.tscn";
        public static string TopDownStart = "res://Scenes/TopDownStart.tscn";
        public static string FirstPersonStart = "res://Scenes/FirstPersonStart.tscn";

        // Packed Scenes
        public static string HuntingPlayer = "res://Scenes/HuntingPlayer.tscn";
        public static string Bullet = "res://Scenes/Bullet.tscn";
        public static string TopDownDeer = "res://Scenes/Animals/TopDown/Deer.tscn";
        public static string TopDownBuck = "res://Scenes/Animals/TopDown/Buck.tscn";
        public static string TopDownSquirrel = "res://Scenes/Animals/TopDown/Squirrel.tscn";
        public static string TopDownRabbit = "res://Scenes/Animals/TopDown/Rabbit.tscn";
        public static string TopDownBear = "res://Scenes/Animals/TopDown/Bear.tscn";
        public static string TopDownBuffalo = "res://Scenes/Animals/TopDown/Buffalo.tscn";
        public static string FirstPersonGoose = "res://Scenes/Animals/FirstPerson/Goose.tscn";
        public static string FirstPersonDuck = "res://Scenes/Animals/FirstPerson/Duck.tscn";
        public static string FirstPersonBear = "res://Scenes/Animals/FirstPerson/Bear.tscn";
        public static string FirstPersonBuffalo = "res://Scenes/Animals/FirstPerson/Buffalo.tscn";
        public static string FirstPersonElk = "res://Scenes/Animals/FirstPerson/Elk.tscn";
        public static string FirstPersonCaribou = "res://Scenes/Animals/FirstPerson/Caribou.tscn";
        public static string FirstPersonDeer = "res://Scenes/Animals/FirstPerson/Deer.tscn";
        public static string FirstPersonRabbit = "res://Scenes/Animals/FirstPerson/Rabbit.tscn";
        public static string FirstPersonSquirrel = "res://Scenes/Animals/FirstPerson/Squirrel.tscn";

        public static string Pine = "res://Scenes/Foliage/Pine.tscn";
        public static string Tree = "res://Scenes/Foliage/Tree.tscn";
        public static string Bush = "res://Scenes/Foliage/Bush.tscn";
        public static string Cactus = "res://Scenes/Foliage/Cactus.tscn";
        public static string RedFlower = "res://Scenes/Foliage/RedFlower.tscn";
        public static string PurpleFlower = "res://Scenes/Foliage/PurpleFlower.tscn";
        public static string Rock = "res://Scenes/Foliage/Rock.tscn";

        public static string Desert = "res://Scenes/Levels/Desert.tscn";
        public static string Plains = "res://Scenes/Levels/Plains.tscn";
        public static string River = "res://Scenes/Levels/River.tscn";
        public static string Woods = "res://Scenes/Levels/Woods.tscn";

        // Spawn Nodes
        public static string Spawn = "Spawn";
        public static string AirSpawn = "AirSpawn";
        public static string GroundSpawn = "GroundSpawn";
        public static string TimedSpawn = "TimedSpawn";

        // Controls
        public static string Cancel = "ui_cancel";
        public static string Up = "ui_up";
        public static string Down = "ui_down";
        public static string Left = "ui_left";
        public static string Right = "ui_right";
        public static string UpRight = "ui_upright";
        public static string UpLeft = "ui_upleft";
        public static string BottomRight = "ui_bottomright";
        public static string BottomLeft = "ui_bottomleft";
        public static string WalkControl = "walk";
        public static string Shoot = "shoot";
        public static string Pause = "pause";

        // Animations
        public static string Idle = "Idle";
        public static string Run = "Run";
        public static string Start = "Start";
        public static string End = "End";
        public static string EndIdle = "EndIdle";
        public static string PrintDir = "PrintDir";
        public static string Walk = "Walk";
        public static string WoodsFrame = "Woods";
        public static string RiverFrame = "River";
        public static string PopIn = "PopIn";
        public static string Default = "Default";
        public static string Open = "Open";
        public static string Close = "Close";
        public static string Dead = "Dead";

        // Groups
        public static string Animals = "Animals";

        // Call Methods
        public static string SetSpawn = "SetSpawn";
        public static string GetAmmoAvailable = "GetAmmoAvailable";
        public static string BulletUsed = "BulletUsed";
        public static string SetAirMotion = "SetAirMotion";
        public static string SetMotion = "SetMotion";
        public static string SetDirection = "SetDirection";
        public static string WasShot = "WasShot";
    }
}
