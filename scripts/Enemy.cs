using Godot;
using System;

public partial class Enemy : Node2D
{
    [Export] float speed;
    [Export] float resistance;
    [Export] float pointWorth;
    [Export] bool dropsBombs;

    [Export] Area2D hitArea;
    [Export] Sprite2D sprite;
    [Export] PackedScene bombScene;
    [Export] Timer bombDropTimer;
    [Export] Label nextBombLabel;

    public bool leftSpawn = false;
    public bool rightSpawn = false;

    bool hit = false;


    public override void _Ready()
    {
        hitArea.AreaEntered += OnHitAreaAreaEntered;

        if (dropsBombs)
        {
            bombDropTimer.Timeout += OnBombDropTimerTimeout;
        }
    }

    public override void _Process(double delta)
    {
        if (hit != true)
        {
            if (leftSpawn == true)
            {
                GlobalPosition += new Vector2(speed, 0);
                sprite.FlipH = true;
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

        if (dropsBombs)
        {
            nextBombLabel.Text = bombDropTimer.TimeLeft.ToString("0.0");
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
            tentacleHit.pointsGiven = pointWorth;
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

    private void OnBombDropTimerTimeout()
    {
        Bomb newBomb = (Bomb)bombScene.Instantiate();
        GetParent().AddChild(newBomb);
        newBomb.GlobalPosition = GlobalPosition;
        bombDropTimer.WaitTime = GD.RandRange(1, 8);
    }
}
