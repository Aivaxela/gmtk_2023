using Godot;
using System;

public partial class Main : Node
{
    [Export] public Timer spawnTimer;
    [Export] Label tentacleCountLabel;
    [Export] Label pointsAccumulatedLabel;
    [Export] Label soulsAccumulatedLabel;
    [Export] Label healthLabel;
    [Export] public Label shieldReadyLabel;

    [Export] PackedScene tentacleScene;
    [Export] PackedScene[] enemyScenes;
    [Export] public AudioStream[] enemyDeathSounds;
    [Export] Marker2D leftSpawn;
    [Export] Marker2D rightSpawn;
    [Export] CanvasLayer levelEnd;
    [Export] public AudioStreamPlayer2D sfxPlayer;

    int spawnLoc;
    public int tentacleCount = 2;
    public float killsAccumulated = 0;
    public float soulsAccumulated = 0;
    public float killsNeeded = 50;
    public int krakenHP = 10;
    public int currentLevel = 1;
    public bool levelEnded = false;
    public int speedBoostFromUpgrades = 0;

    int canoeSpawnThreshold;
    int bomberSpawnThreshold;
    int torpedoSpawnThreshold;
    int javelinSpawnThreshold;
    int titanicSpawnThreshold;
    int pizzaSpawnThreshold;


    public override void _Ready()
    {
        spawnTimer.Timeout += OnSpawnTimerTimeout;

        levelEnd.Visible = false;
    }

    public override void _Process(double delta)
    {
        SetEnemySpawnRates();
        PauseOnLevelEnd();

        if (Input.IsActionJustPressed("tentacle-attack") && tentacleCount > 0 && !levelEnded)
        {
            Tentacle newTentacle = (Tentacle)tentacleScene.Instantiate();
            AddChild(newTentacle);
            newTentacle.GlobalPosition = new Vector2(newTentacle.GetGlobalMousePosition().X, 530);
            newTentacle.MoveTentacle(newTentacle.GetGlobalMousePosition());
            tentacleCount--;
        }

        tentacleCountLabel.Text = "Tentacles: " + tentacleCount;
        pointsAccumulatedLabel.Text = "Kills: " + killsAccumulated + " / " + killsNeeded;
        soulsAccumulatedLabel.Text = "Souls: " + soulsAccumulated;
        healthLabel.Text = "Health: " + krakenHP;
    }

    private void OnSpawnTimerTimeout()
    {
        spawnLoc = GD.RandRange(0, 1);
        SpawnEnemy(spawnLoc);
    }

    private void SpawnEnemy(int spawn)
    {
        int enemyToSpawnRoll = GD.RandRange(0, 100);
        int enemyToSpawn = 0;

        if (enemyToSpawnRoll < canoeSpawnThreshold)
        {
            GD.Print("Rolled a " + enemyToSpawnRoll + ", spawned enemy canoe");
            enemyToSpawn = 0;
        }
        else if (enemyToSpawnRoll <= bomberSpawnThreshold)
        {
            GD.Print("Rolled a " + enemyToSpawnRoll + ", spawned enemy bomber");
            enemyToSpawn = 1;
        }
        else if (enemyToSpawnRoll <= torpedoSpawnThreshold)
        {
            GD.Print("Rolled a " + enemyToSpawnRoll + ", spawned enemy torpedo");
            enemyToSpawn = 2;
        }
        else if (enemyToSpawnRoll <= javelinSpawnThreshold)
        {
            GD.Print("Rolled a " + enemyToSpawnRoll + ", spawned enemy javelin");
            enemyToSpawn = 3;
        }
        else if (enemyToSpawnRoll <= titanicSpawnThreshold)
        {
            GD.Print("Rolled a " + enemyToSpawnRoll + ", spawned enemy titanic");
            enemyToSpawn = 4;
        }
        else if (enemyToSpawnRoll <= pizzaSpawnThreshold)
        {
            GD.Print("Rolled a " + enemyToSpawnRoll + ", spawned pizza");
            enemyToSpawn = 5;
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

    private void PauseOnLevelEnd()
    {
        if (killsAccumulated >= killsNeeded)
        {
            levelEnd.Visible = true;
            levelEnded = true;
            spawnTimer.Stop();

            foreach (Node enemy in GetTree().GetNodesInGroup("enemy"))
            {
                enemy.QueueFree();
            }
        }
    }

    private void SetEnemySpawnRates()
    {
        if (currentLevel == 1)
        {
            canoeSpawnThreshold = 100;
            bomberSpawnThreshold = -1;
            torpedoSpawnThreshold = -1;
            javelinSpawnThreshold = -1;
            titanicSpawnThreshold = -1;
            pizzaSpawnThreshold = -1;

            canoeSpawnThreshold = 10;
            bomberSpawnThreshold = 40;
            torpedoSpawnThreshold = 55;
            javelinSpawnThreshold = 90;
            titanicSpawnThreshold = 95;
            pizzaSpawnThreshold = 100;
        }
        if (currentLevel == 2)
        {
            canoeSpawnThreshold = 70;
            bomberSpawnThreshold = 95;
            torpedoSpawnThreshold = -1;
            javelinSpawnThreshold = -1;
            titanicSpawnThreshold = -1;
            pizzaSpawnThreshold = 100;
        }
        if (currentLevel == 3)
        {
            canoeSpawnThreshold = 40;
            bomberSpawnThreshold = 70;
            torpedoSpawnThreshold = 95;
            javelinSpawnThreshold = -1;
            titanicSpawnThreshold = -1;
            pizzaSpawnThreshold = 100;
        }
        if (currentLevel == 5)
        {
            canoeSpawnThreshold = 20;
            bomberSpawnThreshold = 50;
            torpedoSpawnThreshold = 75;
            javelinSpawnThreshold = 95;
            titanicSpawnThreshold = -1;
            pizzaSpawnThreshold = 100;
        }
        if (currentLevel == 5)
        {
            canoeSpawnThreshold = 10;
            bomberSpawnThreshold = 40;
            torpedoSpawnThreshold = 55;
            javelinSpawnThreshold = 85;
            titanicSpawnThreshold = 95;
            pizzaSpawnThreshold = 100;
        }
    }
}
