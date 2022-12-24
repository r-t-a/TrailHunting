using TrailHunting.Scripts.Managers;

namespace TrailHunting.Scripts.Models
{
    public class PlayerData
    {
        public bool IsFirstPersonStyle;
        public int FirstPersonHighScore;
        public int TopDownHighScore;

        public static PlayerData ToPlayerData(PlayerManager state)
        {
            return new PlayerData()
            {
                IsFirstPersonStyle = state.IsFirstPersonStyle,
                FirstPersonHighScore = state.FirstPersonHighScore,
                TopDownHighScore = state.TopDownHighScore,
            };
        }
    }
}
