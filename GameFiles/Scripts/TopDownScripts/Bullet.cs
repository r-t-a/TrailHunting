using Godot;

public class Bullet : KinematicBody2D
{
    private Vector2 bulletMovement;
    private int speed = 180;

    public override void _Ready()
    {
        CollisionLayer = 2;
        CollisionMask = 2;
    }

    public override void _PhysicsProcess(float delta)
    {
        var didCollide = MoveAndCollide(bulletMovement * speed * delta);
        if (didCollide != null)
        {
            if (didCollide.Collider is KinematicBody2D)
            {
                var collider = (KinematicBody2D)didCollide.Collider;
                collider.Call("WasShot");
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
