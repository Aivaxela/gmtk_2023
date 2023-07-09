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
    [Export] bool rapidFires;

    [Export] public Area2D hitArea;
    [Export] Sprite2D sprite;
    [Export] PackedScene bombScene;
    [Export] PackedScene torpedoScene;
    [Export] PackedScene javelinScene;
    [Export] PackedScene rapidBombScene;
    [Export] Timer bombDropTimer;
    [Export] Timer torpedoDropTimer;
    [Export] Timer javelinDropTimer;
    [Export] Timer rapidBombDropTimer;

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
            bombDropTimer.WaitTime = GD.RandRange(1, 2.5);
            bombDropTimer.Start();
        }
        if (firesTorpedos)
        {
            torpedoDropTimer.Timeout += OnTorpedoDropTimerTimeout;
            torpedoDropTimer.WaitTime = GD.RandRange(1, 3);
            torpedoDropTimer.Start();
        }
        if (throwsJavelins)
        {
            javelinDropTimer.Timeout += OnJavelinDropTimerTimeout;
            javelinDropTimer.WaitTime = GD.RandRange(0.1, 5);
            javelinDropTimer.Start();
        }
        if (rapidFires)
        {
            rapidBombDropTimer.Timeout += OnRapidBombDropTimerTimeout;
            rapidBombDropTimer.WaitTime = GD.RandRange(0.5, 0.5);
            rapidBombDropTimer.Start();
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
            rapidFires = false;
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
            bombDropTimer.WaitTime = GD.RandRange(1, 2.5);

        }
    }

    private void OnTorpedoDropTimerTimeout()
    {
        if (firesTorpedos)
        {
            Torpedo newTorpedo = (Torpedo)torpedoScene.Instantiate();
            newTorpedo.GlobalPosition = GlobalPosition;
            GetParent().AddChild(newTorpedo);
            torpedoDropTimer.WaitTime = GD.RandRange(1, 3);

        }
    }

    private void OnJavelinDropTimerTimeout()
    {
        if (throwsJavelins)
        {
            Javelin newJavelin = (Javelin)javelinScene.Instantiate();
            newJavelin.GlobalPosition = GlobalPosition;
            GetParent().AddChild(newJavelin);
            javelinDropTimer.WaitTime = GD.RandRange(0.1, 5);
        }
    }

    private void OnRapidBombDropTimerTimeout()
    {
        if (rapidFires)
        {
            RapidBomb newRapidBomb = (RapidBomb)rapidBombScene.Instantiate();
            newRapidBomb.GlobalPosition = GlobalPosition;
            GetParent().AddChild(newRapidBomb);
            rapidBombDropTimer.WaitTime = GD.RandRange(0.5, 0.5);
        }
    }
}
