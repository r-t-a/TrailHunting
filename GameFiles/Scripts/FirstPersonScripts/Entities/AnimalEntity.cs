using Godot;
using System;
using TrailHunting.Scripts.Enums;

namespace TrailHunting.Scripts.FirstPersonScripts.Entities
{
    public abstract partial class AnimalEntity : KinematicBody2D
    {
        public Viewport Tree { get; set; }
        public float Delta { get; protected set; }
        public AnimatedSprite AnimatedSprite { get; set; }
        public Timer Timer { get; set; }
        public int HP { get; set; }
        public int Speed { get; set; }
        public MoveDirection MoveDirection { get; set; }
        public Vector2 Motion { get; set; }

        public virtual void Init() { }
        public virtual void UpdatePhysics() { }

        public sealed override void _Ready()
        {
            Tree = GetTree().Root;
            AnimatedSprite = GetNodeOrNull<AnimatedSprite>("AnimatedSprite");
            AnimatedSprite.Playing = true;
            Timer = GetNodeOrNull<Timer>("MoveTimer");
            AddToGroup("Animals");
            Init();
        }

        public sealed override void _PhysicsProcess(float delta)
        {
            Delta = (float)delta;
            MoveAndCollide(Motion * Speed * Delta);
            UpdatePhysics();
        }

        public void SetMotion(Vector2 motion)
        {
            Motion = motion;
            if (motion == Vector2.Zero)
            {
                AnimatedSprite.Play("Idle");
            }
            if (Motion == Vector2.Left || Motion == Vector2.Right)
            {
                AnimatedSprite.Play("Walking");
                if (Motion == Vector2.Left)
                {
                    AnimatedSprite.FlipH = true;
                }
                else
                {
                    AnimatedSprite.FlipH = false;
                }
            }
        }

        public void SetAirMotion(Vector2 motion)
        {
            Motion = motion;
            if (Motion == Vector2.Left || Motion == Vector2.Right)
            {
                AnimatedSprite.Play("Flying");
                if (Motion == Vector2.Left)
                {
                    AnimatedSprite.FlipH = true;
                }
            }
        }

        public static Vector2 RandomizeMovement()
        {
            var result = new Random().Next(3);
            switch (result)
            {
                default:
                case 0:
                    return Vector2.Zero;
                case 1:
                    return Vector2.Left;
                case 2:
                    return Vector2.Right;
            }
        }
    }
}
