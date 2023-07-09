using Godot;
using System;

public partial class LevelEnd : CanvasLayer
{
    [Export] Button nextLevelButton;
    [Export] Button krakenSpeedUpgradeButton;
    [Export] Button krakenHealthUpgradeButton;
    [Export] Button tentacleAdditionUpgradeButton;
    [Export] Label speedUpgradeCostLabel;
    [Export] Label healthUpgradeCostLabel;
    [Export] Label tentacleAdditionCostLabel;

    int speedUpgradePointsNeeded = 5;
    int healthUpgradePointsNeeded = 10;
    int tentacleAdditionPointsNeeded = 30;

    Main main;
    Kraken kraken;


    public override void _Ready()
    {
        main = GetNode<Main>("/root/main");
        kraken = GetNode<Kraken>("/root/main/kraken");

        nextLevelButton.Pressed += OnNextLevelButtonPressed;
        krakenSpeedUpgradeButton.Pressed += OnKrakenSpeedUpgradeButtonPressed;
        krakenHealthUpgradeButton.Pressed += OnKrakenHealthUpgradeButtonPressed;
        tentacleAdditionUpgradeButton.Pressed += OnTentacleAdditionUpgradeButtonPressed;
    }

    public override void _Process(double delta)
    {
        speedUpgradeCostLabel.Text = speedUpgradePointsNeeded.ToString();
        healthUpgradeCostLabel.Text = healthUpgradePointsNeeded.ToString();
        tentacleAdditionCostLabel.Text = tentacleAdditionPointsNeeded.ToString();
    }

    private void OnNextLevelButtonPressed()
    {
        main.currentLevel++;
        main.killsAccumulated = 0;
        main.spawnTimer.Start();
        main.levelEnded = false;
        this.Visible = false;
    }

    private void OnKrakenSpeedUpgradeButtonPressed()
    {
        if (main.soulsAccumulated >= speedUpgradePointsNeeded)
        {
            main.soulsAccumulated -= speedUpgradePointsNeeded;
            kraken.speed += 25;
            speedUpgradePointsNeeded += 5;
        }
    }

    private void OnKrakenHealthUpgradeButtonPressed()
    {
        if (main.soulsAccumulated >= healthUpgradePointsNeeded)
        {
            main.soulsAccumulated -= healthUpgradePointsNeeded;
            main.krakenHP += 5;
            healthUpgradePointsNeeded += 5;
        }
    }

    private void OnTentacleAdditionUpgradeButtonPressed()
    {
        if (main.soulsAccumulated >= tentacleAdditionPointsNeeded)
        {
            main.soulsAccumulated -= tentacleAdditionPointsNeeded;
            main.tentacleCount++;
            tentacleAdditionPointsNeeded += 20;
        }
    }
}
