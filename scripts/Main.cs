using Godot;
using System;

public partial class Main : Node
{
    [Export] public int currentLevel = 1;
    [Export] public Timer spawnTimer;
    [Export] public Timer invulnerabilityTimer;
    [Export] Label tentacleCountLabel;
    [Export] Label pointsAccumulatedLabel;
    [Export] Label soulsAccumulatedLabel;
    [Export] Label healthLabel;
    [Export] Label levelLabel;
    [Export] public Label shieldReadyLabel;

    [Export] PackedScene tentacleScene;
    [Export] PackedScene[] enemyScenes;
    [Export] public AudioStream[] enemyDeathSounds;
    [Export] Marker2D leftSpawn;
    [Export] Marker2D rightSpawn;
    [Export] CanvasLayer levelEnd;
    [Export] public AudioStreamPlayer2D sfxPlayer;

    int spawnLoc;
    public int tentacleCount = 1;
    public float killsAccumulated = 0;
    public float soulsAccumulated = 0;
    public float killsNeeded = 5;
    public int krakenHP = 10;
    public bool levelEnded = false;
    public int speedBoostFromUpgrades = 0;
    public bool isInvulnerable = false;

    int canoeSpawnThreshold;
    int bomberSpawnThreshold;
    int torpedoSpawnThreshold;
    int javelinSpawnThreshold;
    int titanicSpawnThreshold;
    int pizzaSpawnThreshold;


    public override void _Ready()
    {
        spawnTimer.Timeout += OnSpawnTimerTimeout;
        invulnerabilityTimer.Timeout += OnInvulnerabilityTimerTimeout;

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
        levelLabel.Text = "Level: " + currentLevel;

        if (krakenHP <= 0)
        {
            ClearEnemies();
            GetTree().ChangeSceneToFile("res://scenes/game-over.tscn");
        }
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

            ClearEnemies();
        }
    }

    private void ClearEnemies()
    {
        foreach (Node enemy in GetTree().GetNodesInGroup("enemy"))
        {
            if (enemy.IsInGroup("tentacle"))
            {
                tentacleCount++;
            }
            enemy.QueueFree();
        }
    }

    private void OnInvulnerabilityTimerTimeout()
    {
        isInvulnerable = false;
    }

    private void SetEnemySpawnRates()
    {
        if (currentLevel == 1)
        {
            spawnTimer.WaitTime = GD.RandRange(1, 3);
            killsNeeded = 10;

            canoeSpawnThreshold = 100;
            bomberSpawnThreshold = -1;
            torpedoSpawnThreshold = -1;
            javelinSpawnThreshold = -1;
            titanicSpawnThreshold = -1;
            pizzaSpawnThreshold = -1;
        }
        if (currentLevel == 2)
        {
            spawnTimer.WaitTime = GD.RandRange(1, 3);
            killsNeeded = 20;

            canoeSpawnThreshold = 30;
            bomberSpawnThreshold = 97;
            torpedoSpawnThreshold = -1;
            javelinSpawnThreshold = -1;
            titanicSpawnThreshold = -1;
            pizzaSpawnThreshold = 100;
        }
        if (currentLevel == 3)
        {
            spawnTimer.WaitTime = GD.RandRange(1, 3);
            killsNeeded = 40;

            canoeSpawnThreshold = 40;
            bomberSpawnThreshold = 70;
            torpedoSpawnThreshold = 97;
            javelinSpawnThreshold = -1;
            titanicSpawnThreshold = -1;
            pizzaSpawnThreshold = 100;
        }
        if (currentLevel == 4)
        {
            spawnTimer.WaitTime = GD.RandRange(1, 2);
            killsNeeded = 50;

            canoeSpawnThreshold = 20;
            bomberSpawnThreshold = 50;
            torpedoSpawnThreshold = 75;
            javelinSpawnThreshold = 97;
            titanicSpawnThreshold = -1;
            pizzaSpawnThreshold = 100;
        }
        if (currentLevel == 5)
        {
            spawnTimer.WaitTime = GD.RandRange(1, 2);
            killsNeeded = 75;

            canoeSpawnThreshold = 10;
            bomberSpawnThreshold = 40;
            torpedoSpawnThreshold = 55;
            javelinSpawnThreshold = 93;
            titanicSpawnThreshold = 97;
            pizzaSpawnThreshold = 100;
        }
        if (currentLevel == 6)
        {
            spawnTimer.WaitTime = GD.RandRange(1, 2);
            killsNeeded = 100;

            canoeSpawnThreshold = 5;
            bomberSpawnThreshold = 30;
            torpedoSpawnThreshold = 50;
            javelinSpawnThreshold = 90;
            titanicSpawnThreshold = 97;
            pizzaSpawnThreshold = 100;
        }
        if (currentLevel == 7)
        {
            spawnTimer.WaitTime = GD.RandRange(1, 2);
            killsNeeded = 120;

            canoeSpawnThreshold = 5;
            bomberSpawnThreshold = 20;
            torpedoSpawnThreshold = 50;
            javelinSpawnThreshold = 85;
            titanicSpawnThreshold = 97;
            pizzaSpawnThreshold = 100;
        }
        if (currentLevel == 8)
        {
            spawnTimer.WaitTime = GD.RandRange(0.5, 3);
            killsNeeded = 140;

            canoeSpawnThreshold = 5;
            bomberSpawnThreshold = 10;
            torpedoSpawnThreshold = 40;
            javelinSpawnThreshold = 82;
            titanicSpawnThreshold = 97;
            pizzaSpawnThreshold = 100;
        }
        if (currentLevel == 9)
        {
            spawnTimer.WaitTime = GD.RandRange(0.5, 3);
            killsNeeded = 150;

            canoeSpawnThreshold = 3;
            bomberSpawnThreshold = 5;
            torpedoSpawnThreshold = 35;
            javelinSpawnThreshold = 93;
            titanicSpawnThreshold = 80;
            pizzaSpawnThreshold = 100;
        }
        if (currentLevel == 10)
        {
            spawnTimer.WaitTime = GD.RandRange(0.5, 2);
            killsNeeded = 200;

            canoeSpawnThreshold = 3;
            bomberSpawnThreshold = 5;
            torpedoSpawnThreshold = 30;
            javelinSpawnThreshold = 70;
            titanicSpawnThreshold = 97;
            pizzaSpawnThreshold = 100;
        }
        if (currentLevel > 10)
        {
            GetTree().ChangeSceneToFile("res://scenes/win.tscn");
        }
    }
}
