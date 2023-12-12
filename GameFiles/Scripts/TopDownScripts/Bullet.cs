using Godot;
using TrailHunting.Scripts.Managers;
using TrailHunting.Scripts.TopDownScripts.Entities;

public class Bullet : KinematicBody2D
{
    [Signal]
    public delegate void CountHit();

    private Vector2 bulletMovement;

    public override void _Ready()
    {
        CollisionLayer = 2;
        CollisionMask = 2;
        Connect(nameof(CountHit), GetTree().Root.GetNode(GetTree().CurrentScene.Name), "onBulletHit");
    }

    public override void _PhysicsProcess(float delta)
    {
        var didCollide = MoveAndCollide(bulletMovement * GameManager.TopDownBulletSpeed * delta);
        if (didCollide != null)
        {
            if (didCollide.Collider is AnimalEntity animal)
            {
                if (animal.HP > 0) EmitSignal(nameof(CountHit));
                animal.WasShot();
                QueueFree();
            }
            else
            {
                QueueFree();
            }
        }
    }

    private void _on_VisibilityNotifier2D_screen_exited() => QueueFree();

    public void SetDirection(Vector2 direction) => bulletMovement = direction;
}