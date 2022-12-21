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
        public int HP { get; set; }
        public int Speed { get; set; }
        public MoveDirection MoveDirection { get; set; }
        public Vector2 Motion { get; set; }

        public virtual void Init() { }
        public virtual void UpdatePhysics() { }

        public sealed override void _Ready()
        {
            Tree = GetTree().Root;
            CollisionLayer = 2;
            CollisionMask = 2;
            ZIndex = 1;
            AnimatedSprite = GetNodeOrNull<AnimatedSprite>("AnimatedSprite");
            AnimatedSprite.Playing = true;
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
        }
    }
}
