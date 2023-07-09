using Godot;
using System;

public partial class Bomb : Node2D
{
    double speed;
    [Export] int bombDamage;

    [Export] Area2D hitArea;
    [Export] PackedScene bombParticlesScene;

    Main main;


    public override void _Ready()
    {
        main = GetNode<Main>("/root/main");

        hitArea.AreaEntered += OnHitAreaAreaEntered;
        hitArea.BodyEntered += OnHitAreaBodyEntered;

        speed = GD.RandRange(0.5, 1.5);
    }

    public override void _Process(double delta)
    {
        GlobalPosition += new Vector2(0, (float)speed);
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
    }
    private void OnHitAreaBodyEntered(Node2D _)
    {
        GpuParticles2D newBombParticles = (GpuParticles2D)bombParticlesScene.Instantiate();
        GetParent().AddChild(newBombParticles);
        newBombParticles.GlobalPosition = GlobalPosition;
        newBombParticles.Emitting = true;
        newBombParticles.GetNode<GpuParticles2D>("bomb-sparkles").Emitting = true;
        newBombParticles.GetNode<GpuParticles2D>("bomb-bubbles").Emitting = true;

        QueueFree();
    }
}
