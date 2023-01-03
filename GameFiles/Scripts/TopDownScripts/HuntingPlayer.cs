using Godot;

public class HuntingPlayer : KinematicBody2D
{
    [Signal]
    public delegate void ShotBullet();

    [Export]
    public int Speed = 80;

    public PackedScene Bullet;
    public AnimationTree AnimationTree;
    public AnimationNodeStateMachinePlayback AnimationNode;
    public Position2D RiflePosition;
    public AudioStreamPlayer2D ShotSfx;

    private Vector2 moveDirection;
    private Vector2 lookDirection;
    private bool isMoving = false;

    public override void _Ready()
    {
        Bullet = (PackedScene)ResourceLoader.Load("res://Scenes/Bullet.tscn");

        AnimationTree = GetNodeOrNull<AnimationTree>("AnimationTree");
        AnimationNode = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/playback");
        RiflePosition = GetNodeOrNull<Position2D>("Position2D");
        ShotSfx = GetNodeOrNull<AudioStreamPlayer2D>("Shot");
        lookDirection = new Vector2(-1, 0);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!isMoving)
        {
            AnimationTree.Set("parameters/Idle/blend_position", lookDirection);
            AnimationNode.Travel("Idle");
        }
        else
        {
            moveDirection = moveDirection.Normalized();
            AnimationTree.Set("parameters/Walk/blend_position", moveDirection);
            AnimationTree.Set("parameters/Idle/blend_position", moveDirection);
            AnimationNode.Travel("Walk");
            MoveAndSlide(moveDirection * Speed);
        }
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("ui_up"))
        {
            lookDirection = Vector2.Zero;
            lookDirection.y = -1;
            if (isMoving)
            {
                moveDirection = Vector2.Zero;
                moveDirection.y = -1;
            }
        }
        if (inputEvent.IsActionPressed("ui_down"))
        {
            lookDirection = Vector2.Zero;
            lookDirection.y = 1;
            if (isMoving)
            {
                moveDirection = Vector2.Zero;
                moveDirection.y = 1;
            }
        }
        if (inputEvent.IsActionPressed("ui_left"))
        {
            lookDirection = Vector2.Zero;
            lookDirection.x = -1;
            if (isMoving)
            {
                moveDirection = Vector2.Zero;
                moveDirection.x = -1;
            }
        }
        if (inputEvent.IsActionPressed("ui_right"))
        {
            lookDirection = Vector2.Zero;
            lookDirection.x = 1;
            if (isMoving)
            {
                moveDirection = Vector2.Zero;
                moveDirection.x = 1;
            }
        }
        if (inputEvent.IsActionPressed("ui_upright"))
        {
            lookDirection = Vector2.Zero;
            lookDirection.x = (float)0.8;
            lookDirection.y = (float)-0.6;
            if (isMoving)
            {
                moveDirection = Vector2.Zero;
                moveDirection.x = (float)0.8;
                moveDirection.y = (float)-0.6;
            }
        }
        if (inputEvent.IsActionPressed("ui_upleft"))
        {
            lookDirection = Vector2.Zero;
            lookDirection.x = (float)-0.8;
            lookDirection.y = (float)-0.6;
            if (isMoving)
            {
                moveDirection = Vector2.Zero;
                moveDirection.x = (float)-0.8;
                moveDirection.y = (float)-0.6;
            }
        }
        if (inputEvent.IsActionPressed("ui_bottomleft"))
        {
            lookDirection = Vector2.Zero;
            lookDirection.x = (float)-0.8;
            lookDirection.y = (float)0.6;
            if (isMoving)
            {
                moveDirection = Vector2.Zero;
                moveDirection.x = (float)-0.8;
                moveDirection.y = (float)0.6;
            }
        }
        if (inputEvent.IsActionPressed("ui_bottomright"))
        {
            lookDirection = Vector2.Zero;
            lookDirection.x = (float)0.8;
            lookDirection.y = (float)0.6;
            if (isMoving)
            {
                moveDirection = Vector2.Zero;
                moveDirection.x = (float)0.8;
                moveDirection.y = (float)0.6;
            }
        }
        if (inputEvent.IsActionPressed("walk"))
        {
            isMoving = !isMoving;
            moveDirection = isMoving ? lookDirection : Vector2.Zero;
        }
        if (inputEvent.IsActionPressed("shoot"))
        {
            if (isMoving)
            {
                return;
            }

            ShotSfx.Play();
            var bullet = (KinematicBody2D)Bullet.Instance();
            GetParent().AddChild(bullet);
            bullet.GlobalPosition = RiflePosition.GlobalPosition;
            bullet.Call("SetDirection", lookDirection);
            EmitSignal(nameof(ShotBullet));
        }
    }
}
