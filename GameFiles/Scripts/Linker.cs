using Godot;
using TrailHunting.Scripts.Managers;

namespace TrailHunting.Scripts
{
    public class Linker : Node
    {
        private GameManager gameManager;

        public override void _Ready()
        {
            gameManager = new GameManager(this);
        }
    }
}