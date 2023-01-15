using Godot;
using TrailHunting.Scripts;
using TrailHunting.Scripts.Managers;

public class Bullet : KinematicBody2D
{
    private Vector2 bulletMovement;

    public override void _Ready()
    {
        CollisionLayer = 2;
        CollisionMask = 2;
    }

    public override void _PhysicsProcess(float delta)
    {
        var didCollide = MoveAndCollide(bulletMovement * GameManager.TopDownBulletSpeed * delta);
        if (didCollide != null)
        {
            if (didCollide.Collider is KinematicBody2D collider)
            {
                collider.Call(Constants.WasShot);
                QueueFree();
            }
            else
            {
                QueueFree();
            }
        }
    }

    private void _on_VisibilityNotifier2D_screen_exited()
    {
        QueueFree();
    }

    public void SetDirection(Vector2 direction)
    {
        bulletMovement = direction;
    }
}
