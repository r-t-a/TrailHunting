using TrailHunting.Scripts.Enums;
using TrailHunting.Scripts.Managers;

namespace TrailHunting.Scripts.Models
{
    public class PlayerData
    {
        public bool IsFirstPersonStyle;
        public bool IsEndless;
        public FirearmsType FirearmType;
        public int FirstPersonTotal;
        public int TopDownTotal;
        public int SquirrelTotal;
        public int RabbitTotal;
        public int DoeTotal;
        public int BuckTotal;
        public int CaribouTotal;
        public int ElkTotal;
        public int BearTotal;
        public int BuffaloTotal;
        public int DuckTotal;
        public int GooseTotal;

        public static PlayerData ToPlayerData(PlayerManager state)
        {
            return new PlayerData()
            {
                IsFirstPersonStyle = state.IsFirstPersonStyle,
                IsEndless = state.IsEndless,
                FirearmType = state.FirearmType,
                FirstPersonTotal = state.FirstPersonTotal,
                TopDownTotal = state.TopDownTotal,
                SquirrelTotal = state.SquirrelTotal,
                RabbitTotal = state.RabbitTotal,
                DoeTotal = state.DoeTotal,
                BuckTotal = state.BuckTotal,
                CaribouTotal = state.CaribouTotal,
                ElkTotal = state.ElkTotal,
                BearTotal = state.BearTotal,
                BuffaloTotal = state.BuffaloTotal,
                DuckTotal = state.DuckTotal,
                GooseTotal = state.GooseTotal,
            };
        }
    }
}
