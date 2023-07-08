using Godot;
using System;

public partial class Bomb : Node2D
{
    [Export] float speed;
    [Export] int bombDamage;

    [Export] Area2D hitArea;
    [Export] PackedScene bombParticlesScene;

    Main main;


    public override void _Ready()
    {
        main = GetNode<Main>("/root/main");

        hitArea.AreaEntered += OnHitAreaAreaEntered;
    }

    public override void _Process(double delta)
    {
        GlobalPosition += new Vector2(0, speed);
    }

    private void OnHitAreaAreaEntered(Area2D _)
    {
        GpuParticles2D newBombParticles = (GpuParticles2D)bombParticlesScene.Instantiate();
        GetParent().AddChild(newBombParticles);
        newBombParticles.GlobalPosition = GlobalPosition;
        newBombParticles.Emitting = true;
        newBombParticles.GetNode<GpuParticles2D>("bomb-sparkles").Emitting = true;
        newBombParticles.GetNode<GpuParticles2D>("bomb-bubbles").Emitting = true;

        main.krakenHP -= bombDamage;
        QueueFree();

        GD.Print("Kraken HP: " + main.krakenHP);
    }
}
