using Godot;
using System;

public partial class Main : Node
{
    [Export] Timer spawnTimer;
    [Export] Label tentacleCountLabel;
    [Export] Label pointsAccumulatedLabel;
    [Export] Label healthLabel;

    [Export] PackedScene tentacleScene;
    [Export] PackedScene[] enemyScenes;
    [Export] Marker2D leftSpawn;
    [Export] Marker2D rightSpawn;

    int spawnLoc;
    public int tentacleCount = 2;
    public float pointsAccumulated = 0;
    public int krakenHP = 10;


    public override void _Ready()
    {
        spawnTimer.Timeout += OnSpawnTimerTimeout;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("tentacle-attack") && tentacleCount > 0)
        {
            Tentacle newTentacle = (Tentacle)tentacleScene.Instantiate();
            AddChild(newTentacle);
            newTentacle.GlobalPosition = new Vector2(newTentacle.GetGlobalMousePosition().X, 530);
            newTentacle.MoveTentacle(newTentacle.GetGlobalMousePosition());
            tentacleCount--;
        }

        tentacleCountLabel.Text = "Tentacles: " + tentacleCount;
        pointsAccumulatedLabel.Text = "Souls: " + pointsAccumulated;
        healthLabel.Text = "Health: " + krakenHP;
    }

    private void OnSpawnTimerTimeout()
    {
        spawnLoc = GD.RandRange(0, 1);
        SpawnEnemy(spawnLoc);

        spawnTimer.WaitTime = GD.RandRange(1, 3);
    }

    private void SpawnEnemy(int spawn)
    {
        int enemyToSpawnRoll = GD.RandRange(0, 100);
        int enemyToSpawn = 0;

        if (enemyToSpawnRoll < 70)
        {
            GD.Print("Rolled a " + enemyToSpawnRoll + ", spawned enemy canoe");
            enemyToSpawn = 0;
        }
        else if (enemyToSpawnRoll < 101)
        {
            GD.Print("Rolled a " + enemyToSpawnRoll + ", spawned enemy bomber");
            enemyToSpawn = 1;
        }

        Enemy newEnemy = (Enemy)enemyScenes[enemyToSpawn].Instantiate();
        GetParent().AddChild(newEnemy);

        if (spawn == 0)
        {
            newEnemy.GlobalPosition = leftSpawn.GlobalPosition;
            newEnemy.leftSpawn = true;
        }
        if (spawn == 1)
        {
            newEnemy.GlobalPosition = rightSpawn.GlobalPosition;
            newEnemy.rightSpawn = true;
        }
    }
}
