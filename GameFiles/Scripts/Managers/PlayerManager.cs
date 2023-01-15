using TrailHunting.Scripts.Enums;

namespace TrailHunting.Scripts.Managers
{
    public class PlayerManager
    {
        public bool IsFirstPersonStyle { get; set; }
        public bool IsEndless { get; set; }
        public FirearmsType FirearmType { get; set; }
        public int FirstPersonTotal { get; set; }
        public int TopDownTotal { get; set; }
        public int SquirrelTotal { get; set; }
        public int RabbitTotal { get; set; }
        public int DoeTotal { get; set; }
        public int BuckTotal { get; set; }
        public int CaribouTotal { get; set; }
        public int ElkTotal { get; set; }
        public int BearTotal { get; set; }
        public int BuffaloTotal { get; set; }
        public int DuckTotal { get; set; }
        public int GooseTotal { get; set; }
        public int FlintlockTotal { get; set; }
        public int RepeaterTotal { get; set; }
        public int PistolTotal { get; set; }

        public bool NeedsToReload { get; set; }
        public bool HasAmmo { get; set; }
        public bool CanShoot()
        {
            return !NeedsToReload && HasAmmo;
        }
    }
}