using Godot;

public class HuntingPlayer : KinematicBody2D
{
    [Signal]
    public delegate void ShotBullet();

    public int Speed = 75;
    public Vector2 _lookDirection;

    private PackedScene Bullet;
    private AnimationTree AnimationTree;
    private AnimationNodeStateMachinePlayback AnimationNode;
    private Position2D RiflePosition;

    private bool _isMoving = false;
    private Vector2 _moveDirection;

    public override void _Ready()
    {
        AnimationTree = GetNode<AnimationTree>("AnimationTree");
        AnimationNode = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/playback");
        Bullet = (PackedScene)ResourceLoader.Load("res://Scenes/Bullet.tscn");
        RiflePosition = GetNode<Position2D>("Position2D");
        _lookDirection = new Vector2(-1, 0);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!_isMoving)
        {
            Speed = 0;
            AnimationTree.Set("parameters/Idle/blend_position", _lookDirection);
            AnimationNode.Travel("Idle");
        }
        else
        {
            Speed = 60;
            _moveDirection = _moveDirection.Normalized();
            AnimationTree.Set("parameters/Walk/blend_position", _moveDirection);
            AnimationTree.Set("parameters/Idle/blend_position", _moveDirection);
            AnimationNode.Travel("Walk");
            MoveAndSlide(_moveDirection * Speed);
        }
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("ui_up"))
        {
            _lookDirection = Vector2.Zero;
            _lookDirection.y = -1;
            if (_isMoving)
            {
                _moveDirection = Vector2.Zero;
                _moveDirection.y = -1;
            }
        }
        if (inputEvent.IsActionPressed("ui_down"))
        {
            _lookDirection = Vector2.Zero;
            _lookDirection.y = 1;
            if (_isMoving)
            {
                _moveDirection = Vector2.Zero;
                _moveDirection.y = 1;
            }
        }
        if (inputEvent.IsActionPressed("ui_left"))
        {
            _lookDirection = Vector2.Zero;
            _lookDirection.x = -1;
            if (_isMoving)
            {
                _moveDirection = Vector2.Zero;
                _moveDirection.x = -1;
            }
        }
        if (inputEvent.IsActionPressed("ui_right"))
        {
            _lookDirection = Vector2.Zero;
            _lookDirection.x = 1;
            if (_isMoving)
            {
                _moveDirection = Vector2.Zero;
                _moveDirection.x = 1;
            }
        }
        if (inputEvent.IsActionPressed("ui_upright"))
        {
            _lookDirection = Vector2.Zero;
            _lookDirection.x = (float)0.8;
            _lookDirection.y = (float)-0.6;
            if (_isMoving)
            {
                _moveDirection = Vector2.Zero;
                _moveDirection.x = (float)0.8;
                _moveDirection.y = (float)-0.6;
            }
        }
        if (inputEvent.IsActionPressed("ui_upleft"))
        {
            _lookDirection = Vector2.Zero;
            _lookDirection.x = (float)-0.8;
            _lookDirection.y = (float)-0.6;
            if (_isMoving)
            {
                _moveDirection = Vector2.Zero;
                _moveDirection.x = (float)-0.8;
                _moveDirection.y = (float)-0.6;
            }
        }
        if (inputEvent.IsActionPressed("ui_bottomleft"))
        {
            _lookDirection = Vector2.Zero;
            _lookDirection.x = (float)-0.8;
            _lookDirection.y = (float)0.6;
            if (_isMoving)
            {
                _moveDirection = Vector2.Zero;
                _moveDirection.x = (float)-0.8;
                _moveDirection.y = (float)0.6;
            }
        }
        if (inputEvent.IsActionPressed("ui_bottomright"))
        {
            _lookDirection = Vector2.Zero;
            _lookDirection.x = (float)0.8;
            _lookDirection.y = (float)0.6;
            if (_isMoving)
            {
                _moveDirection = Vector2.Zero;
                _moveDirection.x = (float)0.8;
                _moveDirection.y = (float)0.6;
            }
        }
        if (inputEvent.IsActionPressed("walk"))
        {
            _isMoving = !_isMoving;
            _moveDirection = _isMoving ? _lookDirection : Vector2.Zero;
        }
        if (inputEvent.IsActionPressed("shoot"))
        {
            if (_isMoving)
                return;

            var bullet = (KinematicBody2D)Bullet.Instance();
            GetParent().AddChild(bullet);
            bullet.GlobalPosition = RiflePosition.GlobalPosition;
            bullet.Call("SetDirection", _lookDirection);
            EmitSignal(nameof(ShotBullet));
        }
    }
}