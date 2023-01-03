using Godot;

public class BulletDisplay : ColorRect
{
    public bool IsAvailable { get; set; } = true;

    public AnimatedSprite Bullet;

    public override void _Ready()
    {
        Bullet = GetNodeOrNull<AnimatedSprite>("Bullet");
    }

    public bool GetAmmoAvailable()
    {
        return IsAvailable;
    }

    public void BulletUsed()
    {
        Bullet.Frame = 1;
        IsAvailable = false;
    }
}
