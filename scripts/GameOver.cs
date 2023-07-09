using Godot;
using System;

public partial class GameOver : Node
{
    [Export] Button restartButton;

    public override void _Ready()
    {
        restartButton.Pressed += OnRestartButtonPressed;
    }

    private void OnRestartButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/main.tscn");
    }
}
