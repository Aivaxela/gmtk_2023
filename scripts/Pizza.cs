using Godot;
using System;

public partial class Pizza : Enemy
{
    bool hit = false;


    public override void _Ready()
    {
        hitArea.AreaEntered += OnHitAreaAreaEntered;
    }

    public override void _Process(double delta)
    {
        if (hit != true)
        {
            if (leftSpawn == true)
            {
                GlobalPosition += new Vector2(speed, 0);
            }
            if (rightSpawn == true)
            {
                GlobalPosition += new Vector2(-speed, 0);
            }
        }

        if (GlobalPosition.X < -128 || GlobalPosition.X > 1088)
        {
            QueueFree();
        }
    }

    private void OnHitAreaAreaEntered(Area2D areaHit)
    {
        hit = true;
        hitArea.CollisionMask = 0;
        if (areaHit.GetParent().GetType() == typeof(Tentacle))
        {
            Tentacle tentacleHit = (Tentacle)areaHit.GetParent();
            tentacleHit.speedReductionFromEnemy = resistance;
            tentacleHit.GetNode<Area2D>("Area2D").QueueFree();
            CallDeferred("MoveMe", tentacleHit);
            GetNode<AnimationPlayer>("AnimationPlayer").Stop();
        }
    }

    private void MoveMe(Tentacle tentacleHit)
    {
        GetParent().RemoveChild(this);
        tentacleHit.AddChild(this);
        GlobalPosition = tentacleHit.GlobalPosition;
    }
}
