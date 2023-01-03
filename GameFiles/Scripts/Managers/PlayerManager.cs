namespace TrailHunting.Scripts.Managers
{
    public class PlayerManager
    {
        public bool IsFirstPersonStyle { get; set; }
        public bool IsEndless { get; set; }
        public int FirstPersonHighScore { get; set; }
        public int TopDownHighScore { get; set; }

        public bool NeedsToReload { get; set; }
        public bool CanShoot()
        {
            return !NeedsToReload;
        }
    }
}