using Godot;
using TrailHunting.Scripts.Enums;

namespace TrailHunting.Scripts.Helpers
{
    public static class Extensions
    {
        public static (Vector2 movement, MoveDirection direction) MovementBasedOnSpawn(this SpawnQuadrant spawnQuadrant)
        {
            switch (spawnQuadrant)
            {
                case SpawnQuadrant.Left:
                    return (new Vector2(1, 0), MoveDirection.Right);
                case SpawnQuadrant.Top:
                    return (new Vector2(0, 1), MoveDirection.Up);
                case SpawnQuadrant.Right:
                    return (new Vector2(-1, 0), MoveDirection.Left);
                case SpawnQuadrant.Bottom:
                    return (new Vector2(0, -1), MoveDirection.Down);
                default:
                    return (Vector2.Zero, MoveDirection.Left);
            }
        }

        public static MoveDirection VectorToMoveDirection(this Vector2 vector)
        {
            if (vector == Vector2.Zero || vector == Vector2.Left)
            {
                return MoveDirection.Left;
            }
            if (vector == Vector2.Right)
            {
                return MoveDirection.Right;
            }
            if (vector == Vector2.Up)
            {
                return MoveDirection.Up;
            }
            if (vector == Vector2.Down)
            {
                return MoveDirection.Down;
            }
            if (vector == new Vector2(-1, 1))
            {
                return MoveDirection.UpLeft;
            }
            if (vector == new Vector2(1, 1))
            {
                return MoveDirection.UpRight;
            }
            if (vector == new Vector2(-1, -1))
            {
                return MoveDirection.DownLeft;
            }
            if (vector == new Vector2(1, -1))
            {
                return MoveDirection.DownRight;
            }
            return MoveDirection.Left;
        }

        public static int FirearmTypeToAmmo(this FirearmsType firearmsType)
        {
            switch (firearmsType)
            {
                default:
                case FirearmsType.Flintlock:
                    return 20;
                case FirearmsType.Repeating:
                    return 15;
                case FirearmsType.Pistol:
                    return 10;
            }
        }
    }
}
