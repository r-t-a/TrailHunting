using Godot;
using System;
using TrailHunting.Scripts.Enums;
using TrailHunting.Scripts.Helpers;

namespace TrailHunting.Scripts.TopDownScripts.Entities
{
    public abstract partial class AnimalEntity : KinematicBody2D
    {
        public Viewport Tree { get; set; }
        public float Delta { get; protected set; }
        public AnimatedSprite AnimatedSprite { get; set; }
        public int HP { get; set; }
        public int WalkSpeed { get; set; }
        public int RunSpeed { get; set; }
        public MoveDirection MoveDirection { get; set; }
        public Vector2 Motion { get; set; }

        public virtual void Init() { }
        public virtual void UpdatePhysics() { }
        public virtual void WasShot() { }

        public sealed override void _Ready()
        {
            Tree = GetTree().Root;
            CollisionLayer = 2;
            CollisionMask = 2;
            AnimatedSprite = GetNodeOrNull<AnimatedSprite>("AnimatedSprite");
            AnimatedSprite.Play(Constants.Idle);
            AddToGroup(Constants.Animals);
            Init();
        }

        public sealed override void _PhysicsProcess(float delta)
        {
            Delta = (float)delta;

            if (HP != 0)
            {
                if (MoveDirection == MoveDirection.Left || MoveDirection == MoveDirection.UpLeft || MoveDirection == MoveDirection.DownLeft || MoveDirection == MoveDirection.Up)
                {
                    AnimatedSprite.Play(Constants.Run);
                    AnimatedSprite.FlipH = true;
                }
                if (MoveDirection == MoveDirection.Right || MoveDirection == MoveDirection.UpRight || MoveDirection == MoveDirection.DownRight || MoveDirection == MoveDirection.Down)
                {
                    AnimatedSprite.Play(Constants.Run);
                    AnimatedSprite.FlipH = false;
                }
                MoveAndCollide(Motion * RunSpeed * Delta);
            }
            else
            {
                AnimatedSprite.Play(Constants.Dead);
                MoveAndCollide(Vector2.Zero);
                // Remove from group to continue spawning alive Animals
                if (IsInGroup(Constants.Animals))
                {
                    RemoveFromGroup(Constants.Animals);
                }
            }
            UpdatePhysics();
        }

        public void SetSpawn(int spawn)
        {
            var spawnQuadrant = (SpawnQuadrant)spawn;
            (Motion, MoveDirection) = spawnQuadrant.MovementBasedOnSpawn();
        }

        public static (Vector2 motion, MoveDirection direction) RandomizeMovement(Random rnd)
        {
            Vector2 motion;
            var movement = (MoveDirection)rnd.Next(0, 8);
            switch (movement)
            {
                case MoveDirection.Left:
                    motion = new Vector2(-1, 0);
                    break;
                case MoveDirection.Up:
                    motion = new Vector2(0, 1);
                    break;
                case MoveDirection.Right:
                    motion = new Vector2(1, 0);
                    break;
                case MoveDirection.Down:
                    motion = new Vector2(0, -1);
                    break;
                case MoveDirection.UpLeft:
                    motion = new Vector2(-1, 1);
                    break;
                case MoveDirection.UpRight:
                    motion = new Vector2(1, 1);
                    break;
                case MoveDirection.DownLeft:
                    motion = new Vector2(-1, -1);
                    break;
                case MoveDirection.DownRight:
                    motion = new Vector2(1, -1);
                    break;
                default:
                    motion = Vector2.Zero;
                    break;
            }
            return (motion, movement);
        }
    }
}
