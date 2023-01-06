using Godot;
using TrailHunting.Scripts;

public class HuntingPlayer : KinematicBody2D
{
    #region Signals
    [Signal]
    public delegate void ShotBullet();
    #endregion

    #region Exports
    [Export]
    protected NodePath AnimationTreeNodePath;
    [Export] 
    protected NodePath RifleNodePath;
    [Export]
    protected NodePath SoundEffectsNodePath;
    #endregion

    #region Properties
    private PackedScene bullet;
    private AnimationTree animationTree;
    private AnimationNodeStateMachinePlayback animationNode;
    private Position2D riflePosition;
    private AudioStreamPlayer2D shotSfx;

    private int speed = 80;
    private Vector2 moveDirection;
    private Vector2 lookDirection;
    private bool isMoving = false;
    #endregion

    #region Overrides
    public override void _Ready()
    {
        bullet = (PackedScene)ResourceLoader.Load(Constants.Bullet);
        animationTree = GetNodeOrNull<AnimationTree>(AnimationTreeNodePath);
        riflePosition = GetNodeOrNull<Position2D>(RifleNodePath);
        shotSfx = GetNodeOrNull<AudioStreamPlayer2D>(SoundEffectsNodePath);
        animationNode = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
        lookDirection = new Vector2(-1, 0);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!isMoving)
        {
            animationTree.Set("parameters/Idle/blend_position", lookDirection);
            animationNode.Travel(Constants.Idle);
        }
        else
        {
            moveDirection = moveDirection.Normalized();
            animationTree.Set("parameters/Walk/blend_position", moveDirection);
            animationTree.Set("parameters/Idle/blend_position", moveDirection);
            animationNode.Travel(Constants.Walk);
            MoveAndSlide(moveDirection * speed);
        }
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed(Constants.Up))
        {
            lookDirection = Vector2.Zero;
            lookDirection.y = -1;
            if (isMoving)
            {
                moveDirection = Vector2.Zero;
                moveDirection.y = -1;
            }
        }
        if (inputEvent.IsActionPressed(Constants.Down))
        {
            lookDirection = Vector2.Zero;
            lookDirection.y = 1;
            if (isMoving)
            {
                moveDirection = Vector2.Zero;
                moveDirection.y = 1;
            }
        }
        if (inputEvent.IsActionPressed(Constants.Left))
        {
            lookDirection = Vector2.Zero;
            lookDirection.x = -1;
            if (isMoving)
            {
                moveDirection = Vector2.Zero;
                moveDirection.x = -1;
            }
        }
        if (inputEvent.IsActionPressed(Constants.Right))
        {
            lookDirection = Vector2.Zero;
            lookDirection.x = 1;
            if (isMoving)
            {
                moveDirection = Vector2.Zero;
                moveDirection.x = 1;
            }
        }
        if (inputEvent.IsActionPressed(Constants.UpRight))
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
        if (inputEvent.IsActionPressed(Constants.UpLeft))
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
        if (inputEvent.IsActionPressed(Constants.BottomLeft))
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
        if (inputEvent.IsActionPressed(Constants.BottomRight))
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
        if (inputEvent.IsActionPressed(Constants.WalkControl))
        {
            isMoving = !isMoving;
            moveDirection = isMoving ? lookDirection : Vector2.Zero;
        }
        if (inputEvent.IsActionPressed(Constants.Shoot))
        {
            if (isMoving)
            {
                return;
            }

            shotSfx.Play();
            var bulletSpawn = (KinematicBody2D)bullet.Instance();
            GetParent().AddChild(bulletSpawn);
            bulletSpawn.GlobalPosition = riflePosition.GlobalPosition;
            bulletSpawn.Call(Constants.SetDirection, lookDirection);
            EmitSignal(nameof(ShotBullet));
        }
    }
    #endregion
}
