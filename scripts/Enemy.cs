using Godot;
using System;

public partial class Enemy : Node2D
{
    [Export] public float speed;
    [Export] public float resistance;
    [Export] float pointWorth;
    [Export] bool dropsBombs;
    [Export] bool firesTorpedos;
    [Export] bool throwsJavelins;

    [Export] public Area2D hitArea;
    [Export] Sprite2D sprite;
    [Export] PackedScene bombScene;
    [Export] PackedScene torpedoScene;
    [Export] PackedScene javelinScene;
    [Export] Timer bombDropTimer;
    [Export] Timer torpedoDropTimer;
    [Export] Timer javelinDropTimer;
    [Export] Label nextBombLabel;
    [Export] Label nextTorpedoLabel;
    [Export] Label nextJavelinLabel;

    public bool leftSpawn = false;
    public bool rightSpawn = false;

    bool hit = false;

    Main main;


    public override void _Ready()
    {
        main = GetNode<Main>("/root/main");

        hitArea.AreaEntered += OnHitAreaAreaEntered;

        if (dropsBombs)
        {
            bombDropTimer.Timeout += OnBombDropTimerTimeout;
        }
        if (firesTorpedos)
        {
            torpedoDropTimer.Timeout += OnTorpedoDropTimerTimeout;
        }
        if (throwsJavelins)
        {
            javelinDropTimer.Timeout += OnJavelinDropTimerTimeout;
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
        if (firesTorpedos)
        {
            nextTorpedoLabel.Text = torpedoDropTimer.TimeLeft.ToString("0.0");
        }
        if (throwsJavelins)
        {
            nextJavelinLabel.Text = javelinDropTimer.TimeLeft.ToString("0.0");
        }
    }

    private void OnHitAreaAreaEntered(Area2D areaHit)
    {
        hit = true;
        hitArea.CollisionMask = 0;

        if (areaHit.GetParent().GetType() == typeof(Tentacle))
        {
            main.sfxPlayer.Stream = main.enemyDeathSounds[GD.RandRange(0, main.enemyDeathSounds.Length - 1)];
            main.sfxPlayer.PitchScale = (float)GD.RandRange(0.8, 1.2);
            main.sfxPlayer.Play();
            Tentacle tentacleHit = (Tentacle)areaHit.GetParent();
            tentacleHit.speedReductionFromEnemy = resistance;
            tentacleHit.pointsGiven = pointWorth;
            tentacleHit.GetNode<Area2D>("Area2D").CollisionLayer = 0;
            CallDeferred("MoveMe", tentacleHit);
            GetNode<AnimationPlayer>("AnimationPlayer").Stop();
            dropsBombs = false;
            firesTorpedos = false;
            throwsJavelins = false;
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
        if (dropsBombs)
        {
            Bomb newBomb = (Bomb)bombScene.Instantiate();
            GetParent().AddChild(newBomb);
            newBomb.GlobalPosition = GlobalPosition;
            bombDropTimer.WaitTime = GD.RandRange(2.2, 10.2);
        }

    }

    private void OnTorpedoDropTimerTimeout()
    {
        if (firesTorpedos)
        {
            Torpedo newTorpedo = (Torpedo)torpedoScene.Instantiate();
            GetParent().AddChild(newTorpedo);
            newTorpedo.GlobalPosition = GlobalPosition;
            torpedoDropTimer.WaitTime = GD.RandRange(2.2, 10.2);
        }
    }

    private void OnJavelinDropTimerTimeout()
    {
        if (throwsJavelins)
        {
            Javelin newJavelin = (Javelin)javelinScene.Instantiate();
            newJavelin.GlobalPosition = GlobalPosition;
            GetParent().AddChild(newJavelin);
            javelinDropTimer.WaitTime = GD.RandRange(2.2, 10.2);
        }
    }
}
